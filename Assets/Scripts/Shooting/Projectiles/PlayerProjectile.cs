using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : BaseProjectile
{

    Vector3 direction;
    public bool fire;

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
            direction = launcher.transform.forward;
            fire = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "Enemy")
        {
            print("Choco con enemigo");
            other.gameObject.GetComponentInParent<EnemyController>().lifePoints -= 10;
            Destroy(this.gameObject);
        }
        
    }

}
