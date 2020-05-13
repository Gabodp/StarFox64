using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRange : MonoBehaviour
{
    private TurretController controller;

    private void Start()
    {
        controller = transform.parent.GetComponent<TurretController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            controller.EnemyInRange(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            controller.EnemyOutOfRange();
        }
    }
}
