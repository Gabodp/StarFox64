using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSpawner : MonoBehaviour
{
    public float spawnRate;
    public GameObject fighterPrefab; 
    public GameObject[] spawnList;

    private bool isSpawning = false;

    public IEnumerator SpawnFighters()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            foreach (GameObject spawn in spawnList)
            {
                GameObject s_fighter = Instantiate(fighterPrefab, spawn.transform.position, spawn.transform.rotation) as GameObject;
                s_fighter.GetComponent<FighterController>().SetDirection(spawn.transform.forward);

                yield return new WaitForSeconds(spawnRate);
            }
            isSpawning = false;
        }
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(SpawnFighters());
        }   
    }
}
