using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Basic Parameters")]
    public int lifePoints;
    public int scoreValue;
    //public bool targetable;
    public GameObject explosion;

    protected EnemyShootingSystem s_system;

    private int baseLifePoints;
    
    protected virtual void Start()
    {
        s_system = GetComponent<EnemyShootingSystem>(); 
        baseLifePoints = lifePoints;
    }

    protected virtual void ExplodeSequence()
    {
        GameObject obj = Instantiate(explosion, transform.position, Quaternion.identity);
        AudioManager.PlaySound(AudioManager.Sound.DestroyExplosion,transform.position,10.0f);
        Destroy(this.gameObject,0.2f);
        Destroy(obj, 1.5f);
    }


    public void ReceiveDamage(int damage)
    {
        if (lifePoints > 0)
        {
            lifePoints -= damage;
            lifePoints = Mathf.Clamp(lifePoints, 0, baseLifePoints);
        }
        
        if(lifePoints == 0) 
            ExplodeSequence();
      
    }
}
