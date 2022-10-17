using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(JoyconData))]

public class skiing_movement : MonoBehaviour
{
    // public float speed = 6.0f;
    [SerializeField] private float jumpSpeed = 1f;
    [SerializeField] private float gravity = 20.0f;
    private float x = 0.0f; //x方向の坂によるスピード
    private float z = 0.0f; //z方向の坂によるスピード
    [SerializeField] private float x_accele = 20.0f;
    [SerializeField] private float z_break = 20.0f;
    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float direction_to = 0.0f;

    //joyCon使うかどうか
    [SerializeField]
    private bool joyConUse;

    
    //加速中の残り時間保存
    [SerializeField]
    private float accelTime;

    //2本のレイにより角度を計測するかどうか
    [SerializeField]
    private bool UseDualRay;

    //2本のレイの中央からのずれ
    [SerializeField]
    private Vector3 rayPosDistance;

    //raycast時地面判定に使うレイヤー
    [SerializeField]
    private LayerMask layerMask;

    //レイの中心座標
    [SerializeField]
    private Vector3 rayCenterPos;

    private float before_direction_to = 0.0f;
    private float normalvect = 0.0f;
    private float accele = 0.0f;
    private float z_speed = 0.0f;

    private Vector3 normalVector = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private bool raybool;
    public Transform myTransform;

    private JoyconData _joyconD;//joyconのデータ取得

    public Animator skiing_animation;

    private Example EXP;//Exampleのデータを持ってくる

    float tmp = 0.0f;//joyconのデータを一旦格納するための変数

    private Example Hundleflag_switch;//Hundleflagのフラグ
    private Example Wiperflag_switch;//wiperflagのフラグ

    private ParticleSystem Jump_Action_Particle; //ジャンプアクションのパーティクル
    private ParticleSystem Jump_Action_Particle_success; //ジャンプアクションのパーティクル
    private ParticleSystem Jump_Action_Particle_good_success; //ジャンプアクションのパーティクル
    private ParticleSystem Jump_Action_Particle_turn; //ターンアクションのパーティクル
    private ParticleSystem Jump_Action_Particle_good_success_special; //ジャンプアクションのパーティクル

    public GameObject Jump_Action_Effect;//ジャンプ時のパーティクルを出すgameObject
    public GameObject Jump_Action_Effect_success;//ジャンプ時のパーティクルを出すgameObject
    public GameObject Jump_Action_Effect_good_success;//ジャンプ時のパーティクルを出すgameObject
    public GameObject Jump_Action_Effect_turn;//ジャンプ時のパーティクルを出すgameObject
    public GameObject Jump_Action_Effect_good_success_special;//ジャンプ時のパーティクルを出すgameObject

    //public int point = 0;//ポイント
    public GameObject flag_get_system;//旗を通過したときのparticleを出すgameObject
    //public AudioClip flag_get_sound_effect;//旗を通過したときの効果音
    private ParticleSystem flag_get_particle;//旗を通過したときのparticle
    //private AudioSource get_audio;//旗を通過したときのオーディオソース
    private bool isAccel = false;//加速しているかどうか
    private int animation_stateInfo = 1808796868;//アニメーション「NormalJump」Stateのハッシュ値
    public bool isjump = false;//ジャンプ可能状態であるか
    public bool isgoodjump = false;//グッドジャンプ状態であるか
    public bool forjump = false;//ジャンプしている間
    public bool isStart = false;//スタートしているかどうか
    public float Turn_cooltime = 0.0f;//横回転のクールタイム
    public float Jumpping_time_total = 0.0f;//ジャンプ滞空時間の合計
    public bool isramp_on = false;//ジャンプ台にいるかどうか
    public bool isjump_Judge = false;//ジャンプ挙動をしたかどうか
    public int Turn_judge_on_ramp = 0;
    public RawImage Ski_Concentration;
    private Material Ski_Conce_Judge;

    private ScoreRecorde_Skiing point_system;
    public AudioSource[] Ski_Audios;
    private GoalTeapMove End_Audio_mute;

    public GameObject goal;
    private float speedDownTimer = 0;
    

    void Start()
    {
        controller = GetComponent<CharacterController>();
        _joyconD = GetComponent<JoyconData>();
        skiing_animation = GetComponent<Animator>();
        accelTime = 0.0f;
        EXP = GetComponent<Example>();
        Hundleflag_switch = GetComponent<Example>();
        Wiperflag_switch = GetComponent<Example>();
        EXP.Load();
        Jump_Action_Particle = Jump_Action_Effect.GetComponentInChildren<ParticleSystem>();
        Jump_Action_Particle_success = Jump_Action_Effect_success.GetComponentInChildren<ParticleSystem>();
        Jump_Action_Particle_good_success = Jump_Action_Effect_good_success.GetComponentInChildren<ParticleSystem>();
        Jump_Action_Particle_turn = Jump_Action_Effect_turn.GetComponentInChildren<ParticleSystem>();
        Jump_Action_Particle_good_success_special = Jump_Action_Effect_good_success_special.GetComponentInChildren<ParticleSystem>();
        flag_get_particle = flag_get_system.GetComponentInChildren<ParticleSystem>();
        Ski_Audios = GetComponents<AudioSource>();
        point_system = GetComponent<ScoreRecorde_Skiing>();
        Ski_Conce_Judge = Ski_Concentration.material;
        Ski_Conce_Judge.SetFloat("_Judge_Jump", 0.0f);
        End_Audio_mute = goal.GetComponent<GoalTeapMove>();
    }

    void Update()
    {
        //肩の傾き
        if (joyConUse) {
            tmp = (float)(-_joyconD.readXR() + _joyconD.readXL());
            if(isAccel == true)
            {
                if (tmp > 0.4f) direction_to = 0.4f;
                else if (tmp < -0.4f) direction_to = -0.4f;
                else direction_to = tmp;
            }else if(isAccel == false)
            {
                if (tmp > 1) direction_to = 1;
                else if (tmp < -1) direction_to = -1;
                else direction_to = tmp;
            }

            
            
        }

        if(UseDualRay){

            Ray ray1 = new Ray(transform.TransformPoint(rayCenterPos + rayPosDistance), -transform.up);
            Ray ray2 = new Ray(transform.TransformPoint(rayCenterPos - rayPosDistance), -transform.up);
            RaycastHit hit1, hit2;
            raybool = Physics.Raycast(ray1, out hit1,2.0f, layerMask);
            raybool = raybool & Physics.Raycast(ray2, out hit2,2.0f, layerMask);
            Vector3 slopeVec = hit1.point - hit2.point;
            
            //Debug.DrawRay(ray1.origin, ray1.direction,Color.green,0);
            //Debug.DrawRay(ray2.origin, ray2.direction,Color.red,0);
            if(raybool)normalVector = (new Vector3(0, slopeVec.z, -slopeVec.y)).normalized;
            else normalVector = Vector3.up;

            //Debug.Log("normalVec" + normalVector);

        }else{
            Vector3 rayPosition = transform.position + new Vector3(0.0f, 0.0f, 0.0f);
            Ray ray = new Ray(rayPosition,Vector3.down);
            //Debug.DrawRay(ray.origin, ray.direction,Color.red,1.0f);
            RaycastHit hit;
            raybool = Physics.Raycast(ray, out hit,1.0f);
            //Debug.Log(raybool);
            normalVector = hit.normal;
            //Debug.Log(hit.normal);
            //Debug.Log("法線座標"+normalVector.x);

            
        }


        before_direction_to = direction_to;
        //Debug.Log("before_direction_to" + before_direction_to);

        //方向制御（キー）
        // if(Input.GetKey(KeyCode.L)){direction_to += Time.deltaTime;
        //     if(direction_to > 1){direction_to = 1.0f;}}
        // else if(Input.GetKey(KeyCode.J) ){direction_to -= Time.deltaTime;
        //     if(direction_to < -1){direction_to = -1.0f;}}
        // else if(direction_to > 0){direction_to -= Time.deltaTime;}
        // else if(direction_to < 0){direction_to += Time.deltaTime;}

        if (direction_to == 1.0 && before_direction_to == 1.0 || direction_to == -1.0 && before_direction_to == -1.0) { before_direction_to = 0; }

        skiing_animation.SetFloat("Blend", (direction_to + 1.0f) / 2.0f);

        //速度制御
        // x =  Mathf.Sign(direction_to) * gravity *normalVector.x * Mathf.Cos(Mathf.PI/2 *  (1 - Mathf.Abs(direction_to)))* Mathf.Cos(Mathf.PI/2 *  (1 - Mathf.Abs(direction_to)))
        //     +  Mathf.Sign(direction_to) * gravity *normalVector.z * Mathf.Cos(Mathf.PI/2 *  (1 - Mathf.Abs(direction_to))) * Mathf.Cos(Mathf.PI/2 *  (1 - Mathf.Abs(direction_to)));
        // Debug.Log("法線x"+ x);
        z = gravity * normalVector.z;
        // z = Mathf.Sign(direction_to) * gravity *normalVector.x * Mathf.Cos(Mathf.PI/2 *  Mathf.Abs(direction_to))* Mathf.Cos(Mathf.PI/2 *  Mathf.Abs(direction_to))
        //     +  Mathf.Sign(direction_to) * gravity *normalVector.z * Mathf.Cos(Mathf.PI/2 * Mathf.Abs(direction_to)) * Mathf.Cos(Mathf.PI/2 *  Mathf.Abs(direction_to));
        //gravity * normalVector.z;
        //     moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        //     moveDirection = transform.TransformDirection(moveDirection);
        //     moveDirection = moveDirection * speed;
        // moveDirection.y = 0;

        //ジャンプ
        //if (Input.GetButton("Jump"))
        //{
        //    moveDirection.y = jumpSpeed;
        //}
        //if (Input.GetKey(KeyCode.P))
        //{
        //    moveDirection.y = moveDirection.z*0.5f;
        //}
        //加速

        if (Input.GetKey(KeyCode.K)) AccelByTime(0.1f);

        if(accelTime>0){
            skiing_animation.SetBool("SpeedUp", true);
            if(forjump == false&&isStart == true)
            {
                Ski_Conce_Judge.SetFloat("_Judge_Jump", 1);
            }
            accelTime -= Time.deltaTime;
            isAccel = true;
            if (accele < 5.0f){
                accele += 0.1f;
            }
        }
        else if(accele > 0){
            skiing_animation.SetBool("SpeedUp", false);
            Ski_Conce_Judge.SetFloat("_Judge_Jump", 0);
            isAccel = false;
            accele -= 0.1f;
        }


        //Debug.Log("y" + moveDirection.y);
        //Debug.Log("x" + moveDirection.x);
        //Debug.Log("direction_to" + direction_to);
        //Debug.Log("before_direction_to" + before_direction_to);
        if (raybool == false&&isramp_on == false) {//接地
            moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
            Hundleflag_switch.hundleflag = false;
            //if (direction_to > 0)
            //{
            //    TurnRight();
            //    Debug.Log("TurnRight");
            //}
            //else if (direction_to < 0)
            //{
            //    TurnLeft();
            //    Debug.Log("TurnLeft");
            //}
        }

        moveDirection.x = x_accele * direction_to + accele * Mathf.Sin(Mathf.PI / 2 * direction_to);//横方向の移動
        if(forjump == false)
        {
            if (accele > 0)
            {
                if (direction_to != 1.0 && direction_to != -1.0)
                {
                    z_speed = moveDirection.z - accele * Mathf.Cos(Mathf.PI / 2 * before_direction_to);
                }
                else
                {
                    z_speed = moveDirection.z - accele * Mathf.Cos(Mathf.PI / 2);
                }

                if (z_speed >= 0 && z_speed < 3)
                {
                    z_speed = z_speed + (z * Time.deltaTime);
                    if (z_speed < 0) { z_speed = 0; }
                }
                else if (z_speed >= 3)
                {
                    z_speed = z_speed + (z * Time.deltaTime - z_break * Mathf.Abs(direction_to) + z_break * Mathf.Abs(before_direction_to));
                    if (z_speed < 3) { z_speed = 3; }
                }
                if (z_speed > 20) { z_speed = 20; }
                moveDirection.z = z_speed + accele * Mathf.Cos(Mathf.PI / 2 * direction_to);
            }
            else
            {
                if (moveDirection.z >= 0 && moveDirection.z < 3)
                {
                    moveDirection.z = moveDirection.z + (z * Time.deltaTime);
                    if (moveDirection.z < 0) { moveDirection.z = 0; }
                }
                else if (moveDirection.z >= 3)
                {
                    moveDirection.z = moveDirection.z + (z * Time.deltaTime - z_break * Mathf.Abs(direction_to) + z_break * Mathf.Abs(before_direction_to));
                    if (moveDirection.z < 3) { moveDirection.z = 3; }
                }
                if (moveDirection.z > 20) { moveDirection.z = 20; }
            }
        }
        
        //Debug.Log("z" + moveDirection.z);
        //Debug.Log("z_accel" + z);
        if(speedDownTimer<=0)controller.Move(moveDirection * Time.deltaTime);
        else{
            controller.Move(new Vector3(moveDirection.x, moveDirection.y, 10.0f) * Time.deltaTime);
            speedDownTimer -= Time.deltaTime;
        }


        Vector3 localAngle = myTransform.localEulerAngles;//アバターを地面に垂直に立たせる
        if (raybool == true) {
            localAngle.x = Vector3.SignedAngle(Vector3.up, normalVector, Vector3.right);
            if(isramp_on == false)
            {
                skiing_animation.SetBool("jump", false);
                skiing_animation.SetBool("GoodJump", false);
                forjump = false;
                
                skiing_animation.SetFloat("JumpBlend", 0.0f);
                isjump = false;
                isgoodjump = false;
                Turn_judge_on_ramp = 0;
                
            }
                
        }
        //Debug.Log("回転" + localAngle.x + localAngle.y + localAngle.z);

        myTransform.localEulerAngles = localAngle;

        if(Input.GetKey(KeyCode.J))
        {
            isStart = true;
            Invoke("player_start", 0.5f);
            gravity =20f;
            moveDirection.y -= 15f;
            moveDirection.z +=10f;
            Ski_Audios[1].Play();
        }
        
        if(forjump == false&&isStart == true&&isramp_on == false)
        {
            Hundleflag_switch.hundleflag = true;
        }
        if (isramp_on == true)
        {
            Hundleflag_switch.hundleflag = false;
        }
        if (Turn_cooltime > 0)
        {
            if(Turn_cooltime>0 && Turn_cooltime - Time.deltaTime <= 0)
            {
                skiing_animation.SetBool("TurnTrigger", false);
            }
            Turn_cooltime -= Time.deltaTime;
        }
        if(forjump == true)
        {
            Jumpping_time_total += Time.deltaTime;
        }else if(forjump == false)
        {
            Ski_Audios[1].mute = false;
            isjump_Judge = false;
            point_system.JumpEnd();
        }
        if(isramp_on == true)
        {
            Ski_Audios[1].mute = true;
        }
        if(accelTime > 0)
        {
            Ski_Audios[1].volume = 0.15f + 0.5f*Mathf.Abs(direction_to);
        }
        else
        {
            Ski_Audios[1].volume = 0.1f + 0.25f * Mathf.Abs(direction_to);
        }
        if(End_Audio_mute.Give_endFlg() == true)
        {
            Ski_Audios[1].mute = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("jumpArea"))
        {
            isjump = true;
            //skiing_animation.SetBool("jump", true);
        }
        if(other.CompareTag("goodjumpArea"))
        {
            //isjump = true;
            isgoodjump = true;
           // skiing_animation.SetBool("GoodJump", true);
        }
        if(other.CompareTag("snowman"))
        {
            //moveDirection.z = 3f;
            //z_speed = 3f;
            speedDownTimer = 0.5f;
        }

        if (other.CompareTag("flags"))
        {
            point_system.flagPassage();
            flag_get_particle.Play();
            Ski_Audios[0].Play();
            //Debug.Log("ポイント" + point);
        }
        if(other.CompareTag("rampArea"))
        {
            isramp_on = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("jumpArea"))
        {
            
            if (isjump_Judge == false )
            {
                //Debug.Log("ジャンプ中");
                forjump = true;
                point_system.JumpStart();
                skiing_animation.SetBool("jump", true);
                moveDirection.y = 0.5f * moveDirection.z; ;
                Jump_Action_Particle.Play();
            }
            //isjump = false;
            //isgoodjump = false;
        }
        if (other.CompareTag("rampArea"))
        {
            isramp_on = false;
            isjump_Judge = false;
        }

    }
    void player_start()
    {
        //joyConUse = true;
        Hundleflag_switch.hundleflag = true;
        Wiperflag_switch.wiperFlag = true;
    }

    public float direction_into_particle(){
        //Debug.Log("direction_to:" + direction_to);    
        return direction_to;
        }
    public float speed_to_z()
    {
      return moveDirection.z;
    }

    public bool ground_judge()
    {
        return raybool;
    }

    public bool IsNormalJump()
    {
        AnimatorStateInfo CurrentInfo =  skiing_animation.GetCurrentAnimatorStateInfo(0);
        //Debug.Log("IsNormalJump"+CurrentInfo.nameHash);
        return (CurrentInfo.nameHash == animation_stateInfo); 
    }

    public void AccelByTime(float time){
        accelTime = time;
    }

    public void Ski_Start()
    {
        isStart = true;
        Invoke("player_start", 0.5f);
        gravity = 20f;
        moveDirection.y -= 15f;
        moveDirection.z += 10f;
        Ski_Audios[1].Play();
    }

    public void Gravity_zero()
    {
        gravity = 0.0f;
        moveDirection.x = 0.0f;
        moveDirection.z = 0.0f;
    }

    public void SkiiJump(){
        //Debug.Log("ジャンプしてるよ");
        if(forjump == false)
        {
            isjump_Judge = true;
            if (isjump == true&&isgoodjump == false)
                {
                 point_system.NormalJump();
                 skiing_animation.SetBool("jump", true);
                 moveDirection.y = 5f + moveDirection.z;
                 Jump_Action_Particle_success.Play();
                Ski_Audios[2].Play();
                 forjump = true;
                 point_system.JumpStart();
                 skiing_animation.SetFloat("JumpBlend", 0.0f);
                 isjump_Judge = false;
                }
                else if (isgoodjump == true&&moveDirection.z >=10f)
                {
                    if(isramp_on ==true&&(Turn_judge_on_ramp == 1|| Turn_judge_on_ramp == -1))
                {
                    Jump_Action_Particle_good_success_special.Play();
                    point_system.HorizontalSpinWithJump();
                }
                else
                {
                    Jump_Action_Particle_good_success.Play();
                    point_system.VerticalSpinWithJump();
                }
                    skiing_animation.SetFloat("JumpBlend", Turn_judge_on_ramp);
                    //Debug.Log("GoodJump");
                    Ski_Audios[2].Play();
                    //isjump_Judge = true;
                    skiing_animation.SetBool("GoodJump", true);
                    skiing_animation.SetBool("jump", true);
                    moveDirection.y =8f + moveDirection.z;
                    forjump = true;
                    point_system.JumpStart();
                    isjump_Judge = false;
            }
     
        }
    }

    public void TurnRight(){
        //Debug.Log("Judge_TurnRight");
        if (IsNormalJump() == true && Turn_cooltime<= 0)
        {
            point_system.SpinAfterJump();
            Turn_cooltime = 0.5f;
            //Debug.Log("Judge_TurnRight");
            skiing_animation.SetBool("TurnTrigger", true);
            skiing_animation.SetFloat("JumpBlend", 1.0f);
            Jump_Action_Particle_turn.Play();
            Ski_Audios[2].Play();
        }
        if(isramp_on == true)
        {
            Turn_judge_on_ramp = 1;
        }
    }

    public void TurnLeft(){
        //Debug.Log("Judge_TurnLeft");
        if (IsNormalJump() == true && Turn_cooltime <= 0)
        {
            point_system.SpinAfterJump();
            Turn_cooltime = 0.5f;
            //Debug.Log("Judge_TurnRight");
            skiing_animation.SetBool("TurnTrigger", true);
            skiing_animation.SetFloat("JumpBlend", -1.0f);
            Jump_Action_Particle_turn.Play();
            Ski_Audios[2].Play();
        }
        if(isramp_on==true)
        {
            Turn_judge_on_ramp = -1;
        }
    } 

}
