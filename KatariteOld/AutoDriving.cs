using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDriving : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    Vector3 targetPoint;
    public Transform firstTarget;

    private void Start()
    {
        targetPoint = firstTarget.position;
    }
    public void FixedUpdate()
    {
        float motor = maxMotorTorque;
        //–Ú“I’n•ûŒü‚ðŽæ“¾
        Vector3 dir = targetPoint - transform.position;
        Vector3 carForward = transform.forward;
        dir.y = 0; carForward.y = 0;
        float angle = Mathf.Rad2Deg * Vector3.SignedAngle(carForward, dir, new Vector3(0, 1, 0));
        float steering;

        if (Mathf.Abs(angle) > maxSteeringAngle)
        {
            steering = angle * maxSteeringAngle / Mathf.Abs(angle);
        }
        else
        {
            steering = angle;
        }
        
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "checkPoint")
        {
            targetPoint = other.gameObject.GetComponent<CheckPoint>().nextCheckPoint.transform.position;
        }
    }
}

