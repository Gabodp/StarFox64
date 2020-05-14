
using UnityEngine;

public class InTurretRange : MonoBehaviour
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
