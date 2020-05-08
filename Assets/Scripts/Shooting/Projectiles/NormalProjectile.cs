using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalProjectile : BaseProjectile
{
    Vector3 direction;
    bool fire;

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
            //direction = (target.transform.position - launcher.transform.position).normalized;
            direction = launcher.transform.forward;
            fire = true;
        }
    }

}
