using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(JoyconData))]

public class ShoulderDriveSimple : MonoBehaviour
{
    //public GameObject steerSliderObj;
    //private Slider steerSlider;
    public bool steerBySlider;
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    //上限
    public float maxSteeringAngle;
    //曲がり度合い(rate : 1でこの角度に)
    public float SteeringAngleRate;
    public GameObject steerObj;
    private Rigidbody rb;
    public float forceVal;
    //public GameObject speedTexObj;
    //private Text speedTex;
    private Vector3 originalPos;
    public float maxSpeed;
    private bool slowRun = false;
    public float nowSpeed;
    public float loughSpeed;
    private bool jumpA = false;

    private JoyconData _joyconD;
    public float jumpTime;
    private float jumpNowTime;
    public GameObject wiperObj;
    Animator wiperAnimator;

    public GameObject fWindowObj;

    private Renderer windowRenderer;

    [SerializeField]
    private float paintAlpha;

    public float cleanSpeed;

    public GameObject shieldR;

    public GameObject shieldL;

    private Animator shieldRAnim, shieldLAnim;

    public RawImage LAlertLabel, RAlertLabel;

    public RawImage Concentration;

    public bool LAlertFlg, RAlertFlg;

    public float penartyTime;

    public float penartySpeed;

    private float alertTimer;

    public bool wiperIsUp = false;

    private AssistViewer assist;

    private Material Conce_Judge;

    public float dashtime;

    private float dTime;

    public AudioClip jumpSound;
    public AudioSource SESource;

    private GameObject par;

    public bool jumpCom;
    public int wiperUp;

    public bool cbFlag;
    public GameObject FlyEnemy_Object;
    FlyEnemy fe;

    private void Start()
    {
        //steerSlider = steerSliderObj.GetComponent<Slider>();
        _joyconD = GetComponent<JoyconData>();
        rb = this.transform.GetComponent<Rigidbody>();
        //speedTex = speedTexObj.GetComponent<Text>();
        originalPos = transform.position;
        jumpNowTime = jumpTime + 1;
        wiperAnimator = wiperObj.GetComponent<Animator>();
        paintAlpha = 0.0f;
        windowRenderer = fWindowObj.GetComponent<Renderer>();
        shieldLAnim = shieldL.GetComponent<Animator>();
        shieldRAnim = shieldR.GetComponent<Animator>();
        LAlertFlg = RAlertFlg = false;
        LAlertLabel.color = new Color(255, 255, 255, 0);
        RAlertLabel.color = new Color(255, 255, 255, 0);
        alertTimer = 0;
        assist = this.gameObject.GetComponent<AssistViewer>();
        Conce_Judge = Concentration.material;
        jumpCom = false;
        wiperUp = 0;
        cbFlag = false;
        fe = FlyEnemy_Object.GetComponent<FlyEnemy>();

        //OVRカメラ周辺視野の解像度を落とす設定
        //OVRManager.fixedFoveatedRenderingLevel = OVRManager.FixedFoveatedRenderingLevel.High;
        //OVRManager.useDynamicFixedFoveatedRendering = true;
    }

    public void FixedUpdate()
    {
        dTime = Time.deltaTime;
        //��ɃA�N�Z���d�l
        float motor = maxMotorTorque * 1;
        //float steeringRate = (!steerBySlider) ? (float)(-_joyconD.readXR() + _joyconD.readXL()) : steerSlider.value;
        float steeringRate = (float)(-_joyconD.readXR() + _joyconD.readXL());
        if(steeringRate > 1) steeringRate = 1;
        else if(steeringRate < -1) steeringRate = -1;
        //SteerByKey
        //if (steerBySlider)
        //{
            if (Input.GetKey(KeyCode.J)) steeringRate = -0.8f;
            else if (Input.GetKey(KeyCode.L)) steeringRate = 0.8f;
            
        //}
        steerObj.transform.localRotation = Quaternion.Euler(0, 0, -90f * steeringRate);
        float steering = SteeringAngleRate * steeringRate;
        if(steering > maxSteeringAngle) steeringRate = maxSteeringAngle;
        else if(steeringRate < -maxSteeringAngle) steeringRate = -maxSteeringAngle;
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
        if (Input.GetKey(KeyCode.Z))
        {
            //rb.AddForce(transform.forward * forceVal);
            //rb.velocity= transform.forward * forceVal;
            FlyEnemy_Object.SetActive(true);
            fe.ShotAction();
        }

        if(Input.GetKey(KeyCode.M)){
            rb.AddForce(-transform.forward * forceVal);
        }

        // if(Input.GetKeyDown(KeyCode.Escape)){
        //     Application.Quit();
        // }

        //�W�����v����
        if (jumpA && Input.GetKeyDown(KeyCode.X))
        {
            accelByTime(jumpTime);
        }
        if(jumpNowTime < jumpTime)
        {
            jumpNowTime += dTime;
            rb.AddForce(transform.forward * forceVal);
            Conce_Judge.SetFloat("_Judge_Jump", 1);
        }else{
            Conce_Judge.SetFloat("_Judge_Jump", 0);
        }

        nowSpeed = rb.velocity.magnitude;
        //speedTex.text = "Speed : " + nowSpeed;
        
        
        //�������x
        if(nowSpeed >= maxSpeed)
        {
            rb.AddForce(transform.forward * (-5000));
        }

        //���t�G���A����
        if(slowRun && nowSpeed > loughSpeed)
        {
            rb.AddForce(transform.forward * (-15000));
        }

        //penarty処理
        if(penartyTime > 0){
            if(nowSpeed > penartySpeed)rb.AddForce(transform.forward * (-15000));
            penartyTime -= dTime;
        }

        // Debug.Log("" + slowRun);
        //�X���C�_�[�ݒ�
        //if(!steerBySlider)steerSlider.value = steeringRate;

        //wiper����
        if (Input.GetKey(KeyCode.C))
        {
            wiperLeft();
        }

        if (Input.GetKey(KeyCode.V))
        {
            wiperRight();
        }


        //alertMove

        if (LAlertFlg || RAlertFlg) alertTimer += Time.deltaTime;
        if (LAlertFlg)
        {
            LAlertLabel.color = new Color(255, 255, 255, (1 - Mathf.Cos(alertTimer * 6)) / 2);
        }
        if (RAlertFlg)
        {
            RAlertLabel.color = new Color(255, 255, 255, (1 - Mathf.Cos(alertTimer * 6)) / 2);
        }

        if(cbFlag){
            if(paintAlpha > 0){
                assist.UseAssistMovie("wiper", false);
                windowRenderer.material.color -= new Color(0,0,0,0.03f);
                paintAlpha -= 0.03f;
            }
        }
        
    }

    public void CarJump(){
        //Debug.Log("jump");
        if(jumpA){
            jumpCom = true;
            accelByTime(jumpTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "roughArea")
        {
            slowRun = true;
        }else if(other.gameObject.tag == "jumpArea")
        {
            jumpA = true;
        }else if(other.gameObject.tag == "dashArea"){
            accelByTime(dashtime);
            par = other.transform.parent.gameObject;
            par.SetActive(false);
        }if(other.gameObject.tag == "colorBallArea"){
            FlyEnemy_Object.SetActive(true);
            fe.ShotAction();
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "roughArea")
        {
            slowRun = false;
        }else if (other.gameObject.tag == "jumpArea")
        {
            jumpA = false;
        }else if(other.gameObject.tag == "dashArea"){
            jumpA = false;
        }if(other.gameObject.tag == "colorBallArea"){
            cbFlag = true;
            FlyEnemy_Object.SetActive(false);
        }  
    }

    public void paintHit(){
        paintAlpha = 1.0f;
        windowRenderer.material.color += new Color(0,0,0,1 - windowRenderer.material.color.a);
    }

    public void paintHit_Tutorial(){
        paintAlpha = 0.5f;
        windowRenderer.material.color += new Color(0,0,0,0.5f - windowRenderer.material.color.a);
    }

    public void wiperLeft(){
        //Debug.Log("WLL");
        wiperIsUp = true;
        wiperAnimator.SetBool("wiperUp", true);
        wiperUp = 1;
        if(paintAlpha > 0){
            windowRenderer.material.color -= new Color(0,0,0,cleanSpeed);
            if(paintAlpha == cleanSpeed) assist.UseAssistMovie("wiper", false);
            else assist.ChangeAssistDir("wiper", true);
            paintAlpha -= cleanSpeed;
        }
    }

    public void wiperRight(){
        //Debug.Log("WRR");
        wiperAnimator.SetBool("wiperUp", false);
        wiperIsUp = false;
        wiperUp = -1;
        if(paintAlpha > 0){
            windowRenderer.material.color -= new Color(0,0,0,cleanSpeed);
            if(paintAlpha == cleanSpeed) assist.UseAssistMovie("wiper", false);
            else assist.ChangeAssistDir("wiper", false);
            paintAlpha -= cleanSpeed;
        }
    }

    public void ColorBallWindow_Clean(){
        windowRenderer.material.color -= new Color(0,0,0,0.01f);

    }

    public void accelByTime(float time){
        if(jumpNowTime>=jumpTime) SESource.PlayOneShot(jumpSound);
        jumpNowTime = jumpTime - time;
    }

    public void guardLeft(){
        shieldLAnim.SetBool("guardDown", true);
    }

    public void guardLUp(){
        shieldLAnim.SetBool("guardDown", false);
    }

    public void guardRight(){
        shieldRAnim.SetBool("guardDown", true);
    }

    public void guardRUp(){
        shieldRAnim.SetBool("guardDown", false);
    }

    public void setLAlertFlg(bool b)
    {
        LAlertFlg = b;
        alertTimer = 0;
        LAlertLabel.color = new Color(255, 255, 255, 0);

    }

    public void setRAlertFlg(bool b)
    {
        RAlertFlg = b;
        alertTimer = 0;
        RAlertLabel.color = new Color(255, 255, 255, 0);
    }
}
