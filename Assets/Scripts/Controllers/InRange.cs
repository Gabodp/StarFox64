using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRange : MonoBehaviour
{
    private EnemyController controller;

    private void Start()
    {
        controller = transform.parent.GetComponent<EnemyController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            controller.enemyInRange(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            controller.enemyOutOfRange();
        }
    }
}
