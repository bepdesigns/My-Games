using UnityEngine;
using System.Collections;

public class vProjectile : MonoBehaviour
{
    public GameObject particleOnCollision;
    public float speed = 10f;

    void Start()
    {
        var coll = GetComponent<Collider>();
        //yield return new WaitForSeconds(0.1f);
        coll.isTrigger = false;
    }
   
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        if (particleOnCollision != null)
        {
            GameObject impactParticle = Instantiate(particleOnCollision, transform.position, transform.rotation) as GameObject;
        }

        Destroy(gameObject);
    }
}