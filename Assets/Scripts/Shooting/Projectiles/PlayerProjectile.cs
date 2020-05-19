using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : BaseProjectile
{

    Vector3 direction;
    public bool fire;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (fire)
        {
            transform.position += direction * (speed * Time.deltaTime);
        }
    }

    public override void FireProjectile(GameObject launcher, GameObject target, int damage, float shootSpeed)
    {
        if (launcher)
        {
            AudioManager.PlaySound(shotSound);
            direction = launcher.transform.forward;
            fire = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("Enemy"))
        {
            print("Choco con enemigo");
            other.gameObject.GetComponentInParent<EnemyController>().ReceiveDamage(damage);
            Destroy(this.gameObject);
        }
        
    }

}
