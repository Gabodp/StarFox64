using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Basic Parameters")]
    public int lifePoints;
    public int scoreValue;
    public bool targetable;
    public GameObject explosion;

    private EnemyShootingSystem s_system;
    private TrackingSystem t_system;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        s_system = GetComponentInChildren<EnemyShootingSystem>();
        t_system = GetComponentInChildren<TrackingSystem>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (lifePoints <= 0)
        {
            Explode();
        }
            
    }

    protected void Explode()
    {
        GameObject obj = Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(this.gameObject,0.2f);
        Destroy(obj, 1.4f);
    }

    //Las funciones de abajo son llamadas por el script In Range de Turret
    public void EnemyInRange(GameObject p_target)
    {
            t_system.SetTarget(p_target);
            s_system.SetTarget(p_target);
    }

    public void EnemyOutOfRange()
    {
            t_system.SetTarget(null);
            s_system.SetTarget(null);
            s_system.RemoveLastProjectiles();//ENCARGADO DE BORRAR GAMEOBJECTS BALAS
    }
}
