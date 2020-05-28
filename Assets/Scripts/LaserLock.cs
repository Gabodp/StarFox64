using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLock : MonoBehaviour
{
    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();    
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.forward, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                line.SetPosition(1, new Vector3(0, 0, hit.distance));
            }
            else
            {
                line.SetPosition(1, new Vector3(0, 0, 140));
            }
        }
    }
}
