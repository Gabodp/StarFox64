using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseShootingSystem : MonoBehaviour
{
    [Header("Parameters")]
    public float fireRate;
    public int damage;
    public GameObject projectile;//Projectile Prefab specified in Unity Editor
    public List<GameObject> projectileSpawns;

    protected GameObject target;
    protected float fireTimer = 0.0f;
    protected List<GameObject> m_lastProjectiles = new List<GameObject>();

    public abstract void SpawnProjectiles();
}
