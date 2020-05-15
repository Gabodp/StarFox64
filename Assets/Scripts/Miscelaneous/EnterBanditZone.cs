using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBanditZone : MonoBehaviour
{
    public List<GameObject> bandits;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for(int i=0; i<bandits.Count; i++)
            {
                bandits[i].GetComponent<BanditController>().ActivateBandit(other.gameObject);
            }
        }
    }
}
