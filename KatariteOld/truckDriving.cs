using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class truckDriving : MonoBehaviour
{
       public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;

    public GameObject playerVehicle;
    //Vector3 targetPoint;
    public Transform firstTarget;

    private Rigidbody rb;

    private float nowSpeed;

    public float highSpeed;

    public float throwDistance;

    private int throwCount;

    public int throwFlame;

    private float maxSpeed;

    private bool ballThrow;

    public GameObject enemy;

    private Animator animator;

    public GameObject colorBallObj;

    private ColorBallMove cbm;

    private bool runFlg;
    private void Start()
    {
        //targetPoint = firstTarget.position;
        rb = GetComponent<Rigidbody>();
        maxSpeed = highSpeed;
        ballThrow = false;
        throwCount = 0;
        animator = enemy.GetComponent<Animator>();
        cbm = colorBallObj.GetComponent<ColorBallMove>();
        runFlg = false;
    }

    public void FixedUpdate()
    {
        if(!runFlg){
            if(playerVehicle.transform.position.z > (this.transform.position.z + 10)){
                runFlg = true;
                this.transform.position = new Vector3((-playerVehicle.transform.position.x+transform.position.x)>=4 ? playerVehicle.transform.position.x + 4 : transform.position.x, playerVehicle.transform.position.y + 4, this.transform.position.z);
                GetComponent<Rigidbody>().isKinematic = false;
            }
        }else{
            float motor = maxMotorTorque;
            //�ړI�n�������擾あ
            //Vector3 dir = targetPoint - transform.position;
            Vector3 carForward = transform.forward;
            //dir.y = 0; carForward.y = 0;
            //float angle = Mathf.Rad2Deg * Vector3.SignedAngle(carForward, dir, new Vector3(0, 1, 0));
            float steering = 0; 

            //if (Mathf.Abs(angle) > maxSteeringAngle)
            //{
                //steering = angle * maxSteeringAngle / Mathf.Abs(angle);
            //}
            //else
            //{
                //steering = angle;
            //}
            
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

            nowSpeed = rb.velocity.magnitude;

            if(!ballThrow && transform.position.z > playerVehicle.transform.position.z + throwDistance){
                maxSpeed = 5;
                throwCount++;
                rb.AddForce(transform.forward * 1000 * (playerVehicle.transform.position.z + throwDistance - transform.position.z));
            }else{
                maxSpeed = highSpeed;
                throwCount = 0;
            }
            //Debug.Log(" count : " + throwCount);

            if(!ballThrow && throwCount > throwFlame){
                animator.SetBool("throwObj", true);
                cbm.throwStart();
                
            }
            if(!ballThrow && throwCount > throwFlame + 60){
                ballThrow = true;
            }
            
            if(nowSpeed<maxSpeed){
                rb.AddForce(transform.forward * (8000));
            }

            

            if(nowSpeed > maxSpeed){
                rb.AddForce(transform.forward * (-8000));
            }

            if((-playerVehicle.transform.position.z + transform.position.z) >= 30)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "checkPoint")
        {
            Destroy(this.gameObject);
        }
    }
    

}
