using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingSystem : BaseShootingSystem

{
    public bool shootLasers;
    public bool shootRockets;
    public GameObject rocket;

    private float rocketRate;
    private float rocketTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        rocketTimer = 0.0f;
        rocketRate = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;
        rocketTimer += Time.deltaTime;

        if (!shootLasers && !shootRockets)
            return;
        else if (shootLasers)
        {
            
            if (fireTimer >= fireRate)
            {
                SpawnProjectiles();

                fireTimer = 0.0f;
            }
        }
        else if (shootRockets)
        {
            if (target && rocketTimer >= rocketRate)
            {
                LaunchRocket();

                rocketTimer = 0.0f;
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
                GameObject proj = Instantiate(projectile, projectileSpawns[i].transform.position, transform.rotation) as GameObject;
                proj.GetComponent<BaseProjectile>().FireProjectile(projectileSpawns[i], target, damage, fireRate);
                AudioManager.PlaySound(AudioManager.Sound.PlayerLaser);
                
                m_lastProjectiles.Add(proj);
            }
        }
    }

    public void LaunchRocket()
    {
        if (!rocket)
            return;
        m_lastProjectiles.Clear();

        for (int i = 0; i < projectileSpawns.Count; i++)
        {
            if (projectileSpawns[i])
            {
                GameObject proj = Instantiate(rocket, projectileSpawns[i].transform.position, Quaternion.Euler(projectileSpawns[i].transform.forward)) as GameObject;
                proj.GetComponent<BaseProjectile>().FireProjectile(projectileSpawns[i], target, damage, fireRate);

                m_lastProjectiles.Add(proj);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            target = null;
            RemoveLastProjectiles();//DEBERIA MOVERSE A OTRO LADO


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
}
