using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingSystem : MonoBehaviour
{
    [Header("Parameters")]
    public float speed;

    [Header("Axis Rotation Field of View")]
    public float xAxisFOV;
    public float yAxisFOV;
    public float zAxisFOV;
    
    private Vector3 lastKnownPosition;
    private Quaternion target_lookAtRotation;
    private Vector3 angles;

    private GameObject target;
    private Quaternion baseRotation;


    private void Start()
    {
        lastKnownPosition = Vector3.zero;
        baseRotation = transform.rotation;
        angles = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {

            //Follow Movement
            if(lastKnownPosition != target.transform.position)
            {
                lastKnownPosition = target.transform.position;
                target_lookAtRotation = Quaternion.LookRotation( lastKnownPosition - transform.position);
            }

            //Apply Rotation
            if(transform.rotation != target_lookAtRotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target_lookAtRotation, speed * Time.deltaTime);
                angles = transform.localEulerAngles;

                //Rotation Restrictions
                //angles.x = Mathf.Clamp(angles.x,0.0f, 45.0f);
                //angles.x = 0.0f;
                angles.x = Mathf.Clamp(angles.x, transform.localEulerAngles.x - xAxisFOV/2, transform.localEulerAngles.x + xAxisFOV/2);
                angles.y = Mathf.Clamp(angles.y, transform.localEulerAngles.y - yAxisFOV/2, transform.localEulerAngles.y + yAxisFOV/2);
                angles.z = Mathf.Clamp(angles.z, transform.localEulerAngles.z - zAxisFOV/2, transform.localEulerAngles.z + zAxisFOV/2);

                transform.localEulerAngles = angles;
                
            }

        }
        else
        {

            if(transform.rotation != baseRotation)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, baseRotation, speed * Time.deltaTime);
        }
    }

    public bool SetTarget(GameObject target)
    {

        this.target = target;

        return true;
    }

}
