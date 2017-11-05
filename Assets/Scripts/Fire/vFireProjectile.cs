using UnityEngine;
using System.Collections;

public class vFireProjectile : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject fireProjectile;
    private vMeleeManager manager;

    void Start()
    {
        manager = GetComponentInParent<vMeleeManager>();
    }

    public void Fire()
    {
        //Debug.Log("Do Effect");
        GameObject projectile = Instantiate(fireProjectile, spawnPoint.position, manager.transform.rotation) as GameObject;
        Transform currentTarget = Camera.main.GetComponent<vLockOnTargetControl>().currentTarget;
        if (currentTarget) {
            var targetHeight = currentTarget.GetComponent<CapsuleCollider>().height / 2;
            Vector3 midPosition = new Vector3(currentTarget.position.x, currentTarget.position.y + targetHeight, currentTarget.position.z);
            projectile.transform.LookAt(midPosition);
        }
    }
}