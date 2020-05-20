using System.Collections;
using UnityEngine;

public class TurretController : EnemyController
{
    private TrackingSystem t_system;
    public GameObject laserObject;

    protected override void Start()
    {
        base.Start();
        t_system = GetComponentInChildren<TrackingSystem>();
    }

    //Las funciones de abajo son llamadas por el script In Range de Turret
    public void EnemyInRange(GameObject p_target)
    {
        StartCoroutine(ChargingTurret(p_target));
        t_system.SetTarget(p_target);
        laserObject.SetActive(true);
    }

    public void EnemyOutOfRange()
    {
        t_system.SetTarget(null);
        s_system.SetTarget(null);
        laserObject.SetActive(false);
    }


    //Sirve para agregar un delay al primer disparo
    IEnumerator ChargingTurret(GameObject target)
    {
        AudioManager.PlaySound(AudioManager.Sound.TurretTargeting);
        yield return new WaitForSeconds(2.0f);
        s_system.SetTarget(target);
        
    }
}
