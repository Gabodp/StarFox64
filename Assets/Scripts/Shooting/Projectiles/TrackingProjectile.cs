using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingProjectile : BaseProjectile
{

    private GameObject target;
    private float timer;
    private float timestamp;
    private Vector3 direction;
    private Quaternion lookRotation;
    private float rotateSpeed;

    public GameObject explosion;

    private void Start()
    {
        timestamp = 0.0f;
        timer = 6.0f;
        rotateSpeed = 6.0f;
    }

    private void Update()
    {
        
        if (target)
        {
            timestamp += Time.deltaTime;

            direction = (target.transform.position - transform.position).normalized;

            lookRotation = Quaternion.LookRotation(direction);
            
            if (timestamp >= 1.2f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

                if (timestamp >= timer)
                    Explode();
            }
            else
            {
                transform.position += Vector3.up * speed * 0.6f * Time.deltaTime;
            }
            
        }
        
        
    }

    public override void FireProjectile(GameObject launcher, GameObject target, int damage, float shootingSpeed)
    {
        if (target)
        {
            this.target = target;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Entro");
            Explode();
            target = null;
        }
            

    }
    private void Explode()
    {
        GameObject explosionObject = Instantiate(explosion, transform.position, Quaternion.identity);

        GameController.Instance.ShakeCamera(4.0f, 1f);
        Destroy(gameObject);
        Destroy(explosionObject, 1.5f);
    }

}
