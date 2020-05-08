using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    public int lifeTime;

    public abstract void DoAction();
    public abstract void DoAnimation();

}
