using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRotation : MonoBehaviour
{
    public float maxAngles;
    public float speed;
    public Transform model;
    private float baseLocalRotation;

    private void Start()
    {
        baseLocalRotation = Mathf.Abs(model.localEulerAngles.y);
    }
    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(model.localEulerAngles.y) > maxAngles + baseLocalRotation)
            speed *= -1;
        model.Rotate(0, Time.deltaTime * speed, 0, Space.Self);
    }
}
