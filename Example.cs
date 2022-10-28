using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum GameType{
    ShoulderDrive,
    ShoulderSki
}

[RequireComponent(typeof(JoyconData))]
public class Example : MonoBehaviour
{
    //private static readonly Joycon.Button[] m_buttons =
        //Enum.GetValues( typeof( Joycon.Button ) ) as Joycon.Button[];

    private List<Joycon>    m_joycons;
    private Joycon          m_joyconL;
    private Joycon          m_joyconR;
    private Joycon.Button?  m_pressedButtonL;
    private Joycon.Button?  m_pressedButtonR;
    public float max_x, max_y, max_z, min_x, min_y, min_z, maxR_x, maxR_y, maxR_z, minR_x, minR_y, minR_z, maxL_x, maxL_y, maxL_z, minL_x, minL_y, minL_z;
    public float normalR_x, normalR_y, normalR_z, normalL_x, normalL_y, normalL_z;
    float accel_R_x, accel_R_y, accel_R_z, accel_L_x, accel_L_y, accel_L_z, x_R, y_R, z_R, x_L, y_L, z_L, gyroR_y, gyroR_z, gyroL_y, gyroL_z;

    public bool jumpF;

    public bool hundleflag = false;
    
    //Gyro用関数
    [SerializeField] private float yLawBorder;
    [SerializeField] private float yChangeBorder;

    [SerializeField] private float zLawBorder;
    [SerializeField] private float zChangeBorder;
    [SerializeField] private float zZeroBorder;
    private int gyroJudge = 0;
    [SerializeField] private float change = 0.002f;

    //前回のキャリブレーションデータ使用(デバッグ用)
    [SerializeField]
    private bool useOldDataForDebug;


    private Queue<float>[,] lists = new Queue<float>[2,2];



    private float[,] y_state = new float[,]{{0,-1,-1},{0,-1,-1}};
    private float[,] z_state = new float[,]{{0,-1,-1},{0,-1,-1}};

    private int[,] trend = new int[,]{{0,0},{0,0}};

    private int[,] z_change = new int[2, 5];

    private int countFrame = 0;

    public bool allowRumble;

    //値保存用クラス (編集者 飯泉)
    private JoyconData joyconData;
    ////////////////////////////////

    ShoulderDriveSimple sds;
    skiing_movement skm;

    [SerializeField]
    GameType gameType;

    // public GameObject SMG;
    JoyDis joydis;

    public float wiperCoolTime;
    public bool wiperUp; 
    [SerializeField]private float wiperLNowTime, wiperRNowTime;
    public float wiperJudge;

    public float dush;
    public float dushTimer;
    private bool dushFlag;
    public bool dushEnable;

    public bool wiperFlag;


    private void Start()
    {
        joydis = GetComponent<JoyDis>();
        jumpF = false;
        for(int j=0;j<4;j++){
            lists[j/2,j%2] = new Queue<float>();
        }
        m_joycons = JoyconManager.Instance.j;

        if ( m_joycons == null || m_joycons.Count <= 0 ) return;

        m_joyconL = m_joycons.Find( c =>  c.isLeft );
        m_joyconR = m_joycons.Find( c => !c.isLeft );

        switch(gameType){
            case GameType.ShoulderDrive :
                sds = this.GetComponent<ShoulderDriveSimple>();
                break;
            case GameType.ShoulderSki : 
                skm = this.GetComponent<skiing_movement>();
                break;
        }
        

        //値保存用クラス定義 (編集者 飯泉)
        joyconData = GetComponent<JoyconData>();
        ////////////////////////////////
        // text = textObj.GetComponent<Text>();
        // Load();

        // wiperCoolTime = 0;
        wiperUp = false;
        wiperFlag = false;
        wiperLNowTime = 0;
        wiperRNowTime = 0;

        dushTimer = 0;
        dushFlag = false;

        //Gyro用
        for(int i=0;i<100;i++){
            for(int j=0;j<4;j++){
                lists[j/2,j%2].Enqueue(0);
            }
        }

        if(useOldDataForDebug) Load();
}

private void FixedUpdate()
    {
        // m_pressedButtonL = null;
        // m_pressedButtonR = null;

        // if ( m_joycons == null || m_joycons.Count <= 0 ) return;

        // if ( Input.GetKeyDown( KeyCode.Z ) )
        // {
        //     m_joyconL.SetRumble( 160, 320, 0.6f, 200 );
        // }
        // if ( Input.GetKeyDown( KeyCode.X ) )
        // {
        //     m_joyconR.SetRumble( 160, 320, 0.6f, 200 );
        // }
        // 右肩の値を1.0～0.0、左肩の値を0.0～-1.0の範囲で小数点以下２桁にしています。桁数変えたかったら、下の100の桁を変えてください
        // 肩の上げ下げに関しては、x_R,x_Lの値を用いれば大丈夫だと思います
        // max_R,min_R,max_L,max_Lにそれぞれキャリブレーションで測定したものを設定する
        if(joydis.joyConnect){

            


        var accel_R = m_joyconR.GetAccel();
        accel_R_x = accel_R.x - normalR_x;
        accel_R_y = accel_R.y - normalR_y;
        accel_R_z = accel_R.z - normalR_z;
        var accel_L = m_joyconL.GetAccel();
        accel_L_x = accel_L.x - normalL_x;
        accel_L_y = accel_L.y - normalL_y;
        accel_L_z = accel_L.z - normalL_z;

        var gyro_R = m_joyconR.GetGyro();
        gyroR_y = gyro_R.y;
        gyroR_z = gyro_R.z;
        var gyro_L = m_joyconL.GetGyro();
        gyroL_y = gyro_L.y;
        gyroL_z = gyro_L.z;

        

        float frameTime = Time.deltaTime;
        if(wiperLNowTime > 0) wiperLNowTime -= frameTime;
        if(wiperRNowTime > 0) wiperRNowTime -= frameTime;

        if (accel_R_x >= 0)
        {
            x_R = (Mathf.Floor((accel_R_x / (maxR_x - normalR_x)) * 100)) / 100;
        }
        else
        {
            x_R = 0;
        }
        if (accel_R_y >= 0)
        {
            y_R = (Mathf.Floor((accel_R_y / (maxR_y - normalR_y)) * 100)) / 100;
        }
        else
        {
            y_R = 0;
        }
        if (accel_R_z >= 0)
        {
            z_R = (Mathf.Floor((accel_R_z / (maxR_z - normalR_z)) * 100)) / 100;
        }
        else
        {
            z_R = (Mathf.Floor((accel_R_z / (maxL_z - normalL_z)) * 100)) / 100;
        }

            
        if (accel_L_x >= 0)
        {
            x_L = (Mathf.Floor((accel_L_x / (maxL_x - normalL_x)) * 100)) / 100;
        }
        else
        {
            x_L = 0;
        }
        if (accel_L_y >= 0)
        {
            y_L = (Mathf.Floor((accel_L_y / (maxL_y - normalL_y)) * 100)) / 100;
        }
        else
        {
            y_L = 0;
        }
        if (accel_L_z >= 0)
        {
            z_L = -(Mathf.Floor((accel_L_z / (maxL_z - normalL_z)) * 100)) / 100;
            }
        else
        {
            z_L = -(Mathf.Floor((accel_L_z / (minL_z - normalL_z)) * 100)) / 100;
        }

        // text.text = "y_L : "+ accel_L + "x : " + accel_R ;
        //値保存用クラスへデータ登録(編集者 飯泉)
        if(!hundleflag){
            joyconData.setHundle(0, 0, 0, 0);
        }else{
            joyconData.setHundle(x_R, x_L, y_R, y_L);
        }
            
            ////////////////////////

        if(Math.Abs(x_L) >= 0.4 && Math.Abs(x_R) >= 0.4){
            jumpF = true;
            switch(gameType){
                case GameType.ShoulderDrive :
                    sds.CarJump();
                    break;
                case GameType.ShoulderSki : 
                    skm.SkiiJump();
                break;
            }
            
        }else{
            jumpF = false;
        }
            // if(Input.GetKey(KeyCode.I)){
            //     ShiftZChange(0, 1);
            // }
            //Debug.Log("z_change : "+ z_change[0,0] + ", " + z_change[0,1] +", "+  z_change[0,2] + ", " + z_change[0,3] + ", " + z_change[0,4] + ", ");
            
        if(Input.GetKeyDown(KeyCode.I)){
            manLoad();
        }
        if(Input.GetKeyDown(KeyCode.U)){
            womanLoad();
        }

        if(Input.GetKeyDown(KeyCode.W)){
            normalR_x = accel_R.x;
            normalR_y = accel_R.y;
            normalR_z = accel_R.z;
            normalL_x = accel_L.x;
            normalL_y = accel_L.x;
            normalL_z = accel_L.x;
        }
        if(Input.GetKeyDown(KeyCode.E)){
            maxR_x = accel_R.x;
            maxR_y = accel_R.y;
            maxR_z = accel_R.z;
            maxL_x = accel_L.x;
            maxL_y = accel_L.x;
            maxL_z = accel_L.x;
        }
        }

        // Debug.Log("gyroR" + (-gyroR_z-gyroR_y) + "," + "gyroL" +(gyroL_z-gyroL_y));
        // Debug.Log("gyroL" +(gyroL_z-gyroL_y));

        if((gyroR_z+gyroR_y) > wiperJudge && (gyroL_z-gyroL_y) > wiperJudge && wiperFlag){
            switch(gameType){
                case GameType.ShoulderDrive :
                    if(wiperRNowTime <= 0 && wiperLNowTime <= 0 && wiperUp){
                        //Debug.Log("wR");
                        wiperRNowTime = wiperCoolTime;
                        sds.wiperRight();
                        wiperUp = false;
                    }
                    break;
                case GameType.ShoulderSki : 
                    if(wiperRNowTime <= 0 && wiperLNowTime <= 0){
                        //Debug.Log("wR");
                        wiperRNowTime = wiperCoolTime;
                        skm.TurnRight();
                    }
                break;
            }
            
        }
        if((gyroR_z+gyroR_y) < -wiperJudge && (gyroL_z-gyroL_y) < -wiperJudge && wiperFlag){
            switch(gameType){
                case GameType.ShoulderDrive :
                    if(wiperLNowTime <= 0 && wiperRNowTime <= 0 && !wiperUp){
                        //Debug.Log("wL");
                        wiperLNowTime = wiperCoolTime;
                        sds.wiperLeft();
                        wiperUp = true;
                    }
                    break;
                case GameType.ShoulderSki : 
                    if(wiperLNowTime <= 0 && wiperRNowTime <= 0){
                        //Debug.Log("wL");
                        wiperLNowTime = wiperCoolTime;
                        skm.TurnLeft();
                    }
                break;
            }
            
        }

        if(dushEnable){
            //Debug.Log("accelLY" + accel_L_y +"," + "accelRY" + accel_R_y);
            if(accel_R_y > (maxR_y-normalR_y)*dush && accel_L_y < (minL_y - normalL_y)*dush){
                dushTimer += Time.deltaTime;
                if(dushTimer >= 0.3){
                    dushFlag = true;
                }
            }else{
                dushTimer = 0;
                dushFlag = false;
            }
        
            if(dushFlag){
                switch(gameType){
                    case GameType.ShoulderDrive :
                        sds.accelByTime(0.1f);
                        break;
                    case GameType.ShoulderSki : 
                        skm.AccelByTime(0.1f);
                        break;
                }
                
            }   
        }
        
        
    }

    //

    //車側が価要求時、joyconクラスを渡す関数(編集者: 飯泉)
    public JoyconData GetJoyconData()
    {
        return joyconData;
    }

    public void Load(){
        //Debug.Log("Load!!");
        maxR_x = PlayerPrefs.GetFloat("MaxR_x",0);
        maxR_y = PlayerPrefs.GetFloat("MaxR_y",0);
        maxR_z = PlayerPrefs.GetFloat("MaxR_z",0);
        minR_x = PlayerPrefs.GetFloat("MinR_x",0);
        minR_y = PlayerPrefs.GetFloat("MinR_y",-0.28f);
        minR_z = PlayerPrefs.GetFloat("MinR_z",0);
        maxL_x = PlayerPrefs.GetFloat("MaxL_x",0.2555f);
        maxL_y = PlayerPrefs.GetFloat("MaxL_y", 0);
        maxL_z = PlayerPrefs.GetFloat("MaxL_z",0);
        minL_x = PlayerPrefs.GetFloat("MinL_x",-0.35175f);
        minL_y = PlayerPrefs.GetFloat("MinL_y",0);
        minL_z = PlayerPrefs.GetFloat("MinL_z",0);
        normalR_x = PlayerPrefs.GetFloat("avenormalR_x", -0.086f);
        normalR_y = PlayerPrefs.GetFloat("avenormalR_y", 0);
        normalR_z = PlayerPrefs.GetFloat("avenormalR_z", 0);
        normalL_x = PlayerPrefs.GetFloat("avenormalL_x", -0.05f);
        normalL_y = PlayerPrefs.GetFloat("avenormalL_y", 0);
        normalL_z = PlayerPrefs.GetFloat("avenormalL_z", 0);
        //Debug.Log("R_x max : "+ maxR_x + "R_x max : "+ minR_x +"R_x normal : "+ normalR_x);
        //Debug.Log("L_x max : "+ maxL_x + "L_x min : "+ minL_x +"L_x normal : "+ normalL_x);
    }

    //飯泉データ
    public void manLoad(){
        maxR_x = 0.0443f;
        maxR_y = 0.033f;
        maxR_z = -0.635f;
        minR_x = -0.7098f;
        minR_y = -0.534f;
        minR_z = -1.337f;
        maxL_x = 0.0463f;
        maxL_y = 1.0688f;
        maxL_z = -0.5478f;
        minL_x = -0.671f;
        minL_y = 0.231f;
        minL_z = -1.065f;
        normalR_x = -0.3132f;
        normalR_y = -0.1613f;
        normalR_z = -0.9584f;
        normalL_x = -0.392f;
        normalL_y = 0.444f;
        normalL_z = -0.852f;
    }

    //竹中データ
    public void womanLoad(){
        maxR_x = 0.176f;
        maxR_y = -0.090f;
        maxR_z = -0.625f;
        minR_x = -0.74f;
        minR_y = -1.018f;
        minR_z = -1.339f;
        maxL_x = 0.322f;
        maxL_y = 1.109f;
        maxL_z = -0.600f;
        minL_x = -0.730f;
        minL_y = 0.265f;
        minL_z = -1.338f;
        normalR_x = -0.349f;
        normalR_y = -0.502f;
        normalR_z = -0.824f;
        normalL_x = -0.366f;
        normalL_y = 0.5866f;
        normalL_z = -0.762f;
    }





    //判定取得用jump関数
    public bool GetJumpFlag(){
        return jumpF;
    }

    //初回jump取得
    public bool GetJumpFirstFlag(){
        return (Math.Abs(x_L) >= 0.2 && Math.Abs(x_R) >= 0.2);
    }

    //JoyCon振動用
    public void JoyConRumble(char dir){
        if(dir=='L') m_joyconL.SetRumble(160,320,0.6f, 200);
        else if(dir=='R') m_joyconR.SetRumble(160,320,0.6f, 200);
        else{
            m_joyconL.SetRumble(160,320,0.6f, 200);
            m_joyconR.SetRumble(160,320,0.6f, 200);
        }
    }

    /*
    private void judge(int jc){
        judgeY(jc);
        judgeZ(jc);
        if(trend[0,0] == -1 && trend[0,1] == -1 && trend[1,0] == -1 && trend[1,1] == 1){
            gyroJudge = 3;
            Debug.Log("gyroJudge : " + gyroJudge);
        }
        if(trend[0,0] == 1 && trend[0,1] == 1 && trend[1,0] == 1 && trend[1,1] == -1){
            gyroJudge = 4;
            Debug.Log("gyroJudge : " + gyroJudge);
        }
        if(ZChangeMatch(jc,new int[5]{0, 1, -1, 1,-1}) || ZChangeMatch(jc,new int[5]{0, -1, 1, -1, 1}) || ZChangeMatch(jc,new int[5]{-1, 1, -1, 1, 0}) || ZChangeMatch(jc,new int[5]{1, -1, 1, -1, 0})){
        //if z_change[jc] == [0, 1, -1, 1] or z_change[jc] == [0, -1, 1, -1] or z_change[jc] == [ 1, -1, 1, 0] or z_change[jc] == [ -1, 1, -1, 0]:
            if(gyroJudge == 1){
                gyroJudge = 5;
                Debug.Log("gyroJudge : " + gyroJudge);
            }
            gyroJudge = jc + 1;
            Debug.Log("gyroJudge : " + gyroJudge);
        }
            

    }

    private void judgeY(int jc){
        float list_1 = 0;
        float list_2 = 0;
        int tmpCount=0;
        foreach(var e in lists[jc,0]){
            
            if(tmpCount==0){
                list_1 = e;
            }
            else if(tmpCount==1){
                list_2 = e;
                break;
            }
            tmpCount++;
        }

        if (list_1 < list_2 && y_state[jc,1] == -1){
            y_state[jc,0] = list_1;
            y_state[jc,1] = 1;
            y_state[jc,2] = -1;
            trend[jc,0] = 0;
        }

        if (list_1 - y_state[jc,0] > yChangeBorder && list_1 > yLawBorder){
            trend[jc,0] = 1;
        }
        if (list_1 > list_2 && y_state[jc,2] == -1){
            y_state[jc,0] = list_1;
            y_state[jc,1] = -1;
            y_state[jc,2] = 1;
            trend[jc,0] = 0;
        }

        if(y_state[jc,0] - list_1 > yChangeBorder && list_1 < -yLawBorder){
            trend[jc,0] = -1;
        }

        if(list_1 == list_2){
            trend[jc,0] = 0;
            if (y_state[jc,1] == 1 || y_state[jc,2] == 1){
                y_state[jc,0] = 0;
                y_state[jc,1] = -1;
                y_state[jc,2] = -1;
            }
        }
    }

    private void judgeZ(int jc){
        Boolean zFlg = false;
        float list_1 = 0;
        float list_2 = 0;
        int tmpCount=0;
        foreach(var e in lists[jc,1]){
            
            if(tmpCount==0){
                list_1 = e;
            }
            else if(tmpCount==1){
                list_2 = e;
                break;
            }
            tmpCount++;
        }
        if(list_1 < list_2 && z_state[jc,1] == -1){
            float temp = list_2 - z_state[jc,0];
            if(temp > change && z_change[jc,4] != 1){
                ShiftZChange(jc, 1);
                zFlg = true;
            }
            else if(temp < -change && z_change[jc,4] != -1){
                ShiftZChange(jc, -1);
                zFlg = true;
            }
            z_state[jc,0] = list_1;
            z_state[jc,1] = 1;
            z_state[jc,2] = -1;
            trend[jc,1] = 0;
        }

        if(list_1 - z_state[jc,0] > zChangeBorder && list_1 > zLawBorder){
            trend[jc,1] = 1;
        }
        if(list_1 > list_2 && z_state[jc,2] == -1){
            float temp = list_2 - z_state[jc,0];
            if(temp > change && z_change[jc,4] != 1){
                ShiftZChange(jc,1);
                zFlg = true;
            }

            else if(temp < -change && z_change[jc,4] != -1){
                ShiftZChange(jc, -1);
                zFlg = true;
            }

            z_state[jc,0] = list_1;
            z_state[jc,1] = -1;
            z_state[jc,2] = 1;
            trend[jc,1] = 0;
        }
        if(z_state[jc,0] - list_1 > zChangeBorder && list_1 < -zLawBorder){
            trend[jc,1] = -1;
        }

        if(Mathf.Abs(list_1 - list_2) < zZeroBorder && !zFlg){
            trend[jc,1] = 0;
            ShiftZChange(jc, 0);
            if(z_state[jc,1] == 1 || z_state[jc,2] == 1){
                z_state[jc,0] = 0;
                z_state[jc,1] = -1;
                z_state[jc,2] = -1;
            }
        }

    }

    private void ShiftZChange(int jc, int p){
        for(int i = 1; i<5; i++) z_change[jc,i-1] = z_change[jc,i];
        z_change[jc, 4] = p;
    }

    private Boolean ZChangeMatch(int jc, int[] n){
        for(int i=0; i<5; i++){
            if(z_change[jc, i] != n[i]){
                return false;
            }
        }
        return true;
    }
    
    */
}