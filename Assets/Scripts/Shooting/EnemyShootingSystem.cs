using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingSystem : BaseShootingSystem
{

    // Update is called once per frame
    void Update()
    {
        if (!target)
            return;
        else
        {
            fireTimer += Time.deltaTime;

            if (fireTimer >= fireRate)
            {
                SpawnProjectiles();

                fireTimer = 0.0f;
            }
        }
    }

    public override void SpawnProjectiles()
    {
        if (!projectile)
        {
            return;
        }

        m_lastProjectiles.Clear();

        for (int i = 0; i < projectileSpawns.Count; i++)
        {
            if (projectileSpawns[i])
            {
                //Quaternion.Euler(projectileSpawns[i].transform.forward)
                GameObject proj = Instantiate(projectile, projectileSpawns[i].transform.position, projectileSpawns[i].transform.rotation) as GameObject;
                proj.GetComponent<BaseProjectile>().FireProjectile(projectileSpawns[i], target, damage, fireRate);

                m_lastProjectiles.Add(proj);
            }
        }
    }


    public void RemoveLastProjectiles()
    {
        while (m_lastProjectiles.Count > 0)
        {
            Destroy(m_lastProjectiles[0]);
            m_lastProjectiles.RemoveAt(0);
        }
    }

    public bool SetTarget(GameObject target)
    {
        this.target = target;

        return true;
    }


}
