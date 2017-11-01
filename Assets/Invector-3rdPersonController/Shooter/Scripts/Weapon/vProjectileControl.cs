using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Invector;
using Invector.EventSystems;

public class vProjectileControl : MonoBehaviour
{    
    public vDamage damage;
    public float forceMultiplier = 1;
    public bool destroyOnCast = true;
    public ProjectilePassDamage onPassDamage;
    public ProjectileCastColliderEvent onCastCollider;

    [HideInInspector]
    public bool damageByDistance;
    [HideInInspector]
    public int minDamage;
    [HideInInspector]
    public int maxDamage;
    [HideInInspector]
    public float DropOffStart = 8f;
    [HideInInspector]
    public float velocity = 580;
    [HideInInspector]
    public float DropOffEnd = 50f;
    [HideInInspector]
    public Vector3 startPosition;
    [HideInInspector]
    public LayerMask hitLayer = -1;   
    [HideInInspector]
    public List<string> ignoreTags;
    [HideInInspector]
    public Transform shooterTransform;

    protected Vector3 previousPosition;

    protected virtual void Start()
    {
        startPosition = transform.position;
        previousPosition = transform.position;
    }

    protected virtual void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Linecast(previousPosition, transform.position + transform.forward * 0.1f, out hitInfo, hitLayer))
        {           
            if (!hitInfo.collider)
                return;
            if (ignoreTags.Contains(hitInfo.collider.gameObject.tag) || (shooterTransform != null && shooterTransform.IsChildOf(hitInfo.collider.transform))) return;
            onCastCollider.Invoke(hitInfo);
            
            damage.damageValue = maxDamage; 
            if(damageByDistance)
            {
                var dist = Vector3.Distance(startPosition, transform.position);
                var result = 0f;
                var damageDifence = maxDamage - minDamage;

                //Calc damage per distance
                if (dist - DropOffStart >= 0)
                {
                    int percentComplete = (int)Math.Round((double)(100 * (dist - DropOffStart)) / (DropOffEnd - DropOffStart));
                    result = Mathf.Clamp(percentComplete * 0.01f, 0, 1f);
                    damage.damageValue = maxDamage - (int)(damageDifence * result);

                }
                else
                    damage.damageValue = maxDamage;
            }
            damage.hitPosition = hitInfo.point;
            damage.receiver = hitInfo.collider.transform;

            if (damage.damageValue > 0 && hitInfo.collider.gameObject.IsAMeleeFighter())
            {
                onPassDamage.Invoke(damage);
                hitInfo.collider.gameObject.GetMeleeFighter().OnReceiveAttack(damage, damage.sender.GetComponent<vIMeleeFighter>());
            }
            else if (damage.damageValue > 0)
            {
                onPassDamage.Invoke(damage);
                hitInfo.collider.gameObject.ApplyDamage(damage);
            }


            var rigb = hitInfo.collider.gameObject.GetComponent<Rigidbody>();
            if (rigb && !hitInfo.collider.gameObject.isStatic)
            {
                rigb.AddForce(transform.forward * damage.damageValue * forceMultiplier, ForceMode.Impulse);
            }
            if (destroyOnCast)
                Destroy(gameObject);
        }
        previousPosition = transform.position;
    }

    [System.Serializable]
    public class ProjectileCastColliderEvent : UnityEngine.Events.UnityEvent<RaycastHit> { }
    [System.Serializable]
    public class ProjectilePassDamage : UnityEngine.Events.UnityEvent<vDamage> { }

}

