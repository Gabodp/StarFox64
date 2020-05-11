using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingProjectile : BaseProjectile
{

    private GameObject target;
    private float timer;
    private float timestamp;

    public GameObject explosion;

    private Vector3 direction;
    private Quaternion lookRotation;

    private void Start()
    {
        timestamp = 0.0f;
        timer = 6.0f;
    }

    private void Update()
    {
        
        if (target)
        {
            timestamp += Time.deltaTime;

            direction = (target.transform.position - transform.position);
            lookRotation = Quaternion.LookRotation(direction);

            if (timestamp >= 1.2f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 10.0f * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            }
            else
            {
                transform.position += Vector3.up * speed * 0.6f * Time.deltaTime;
            }
        }
        
        if (timestamp >= timer)
            Explode();
    }

    public override void FireProjectile(GameObject launcher, GameObject target, int damage, float shootingSpeed)
    {
        if (target)
        {
            this.target = target;
        }
    }

    private void Explode()
    {
        GameObject explosionObject = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(explosionObject, 1.5f);
    }

}
