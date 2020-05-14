
using UnityEngine;

public class InMorterRange : MonoBehaviour
{
    private MorterController controller;

    private void Start()
    {
        controller = transform.parent.GetComponent<MorterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            controller.EnemyInRange(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            controller.EnemyOutOfRange();
        }
    }
}
