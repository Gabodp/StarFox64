using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSpawner : MonoBehaviour
{
    public float spawnRate;
    public int maxFighters;
    public GameObject fighterPrefab;

    private float spawnTimestamp;
    private int currentFighters;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimestamp = 0.0f;
        currentFighters = 0;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimestamp += Time.deltaTime;
        if(spawnTimestamp >= spawnRate && currentFighters < maxFighters)
        {          
            spawnTimestamp = 0.0f;
            SpawnFighter();
        }
    }

    private void SpawnFighter()
    {
        currentFighters++;
        GameObject s_fighter = Instantiate(fighterPrefab, transform.position, transform.rotation) as GameObject;
        s_fighter.GetComponent<FighterController>().SetDirection(transform.forward);
    }

}
