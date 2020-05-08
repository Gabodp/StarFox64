using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public abstract void FireProjectile(GameObject launcher, GameObject target, int damage,float shootSpeed);

}
