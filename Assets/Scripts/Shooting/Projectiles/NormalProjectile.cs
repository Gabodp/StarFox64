using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalProjectile : BaseProjectile
{
    private Vector3 direction;
    private bool fire;

    
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

    public override void FireProjectile(GameObject launcher, GameObject target, int damage,float shootSpeed)
    {
        if(launcher && target)
        {
            AudioManager.PlaySound(shotSound);
            direction = launcher.transform.forward;
            fire = true;
        }
    }

}
