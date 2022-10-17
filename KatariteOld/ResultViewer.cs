using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum ScoreType{
    driveEasy=0,
    driveNormal=1,
    driveHard=2,
    skiSlalom=3,
    skiAirial=4
}

public class ResultViewer : MonoBehaviour
{
    int myTime;
    int penartyCount;
    int skiingFlagScore;
    int skiingFlagCount;
    int skiingJumpScore;
    float jumpTimeNum;
    int myScore;
    public string myName = "guest";
    private int countNum;

    private int myRank;

    public Text myTimeTex;
    public Text myPenartyTex;
    public Text myScoreTex;

    [SerializeField] private Text myTimeRabelTex;
    [SerializeField] private Text myPenartyRabelTex;
    [SerializeField] private Text myScoreRabelTex;

    //ランキング内のランク外欄順位
    public Text[] myRankTex;

    private string rankingCSVPath;
    private List<string> rankingNames = new List<string>();
    private List<int> rankingScores = new List<int>();
    public bool setMyScore;

    public List<Text> rankNameTexts = new List<Text>();
    public List<Text> rankScoreTexts = new List<Text>();

    //ランキング赤ラベル
    public List<Image> rankRedLabel = new List<Image>();

    //ランクtext
    public List<Text> rankAlphabetText= new List<Text>();
    public Text rankAlphabetShadow;

    //ランクボーダー
    private int[] SSborder = {550, 550, 550, 350, 550};
    private int[] Sborder = {500, 500, 500, 320, 500};
    private int[] Aborder = {400, 400, 400, 270, 400};
    private int[] Bborder = {300, 300, 300, 230, 300};

    //表示するランクワードテキスト
    private Text nowRankText;

    public Text timeLimitText;

    private float time = 1000;
    public float timeLim = 5;
    //強制的に最初に戻る時間
    public float endTime;
    //時間保存
    [SerializeField] private float endTimeCounter = -1;
    
    //数字変化の時間
    [SerializeField] private float showTime;

    //リザルト開始からリザルトが動き出すまでの時間
    [SerializeField] private float UIFirstBreak;

    //難易度
    [SerializeField] private ScoreType difficulty;




    void OnEnable(){
        difficulty = (ScoreType)PlayerPrefs.GetInt("Drive_level", 0);
    }
    // Start is called before the first frame update
    public void ShowResult()
    {
        
        switch(difficulty){
            case ScoreType.driveEasy:
                rankingCSVPath = Application.dataPath + "/driveRankingEasy.csv";
                break;
            case ScoreType.driveNormal:
                rankingCSVPath = Application.dataPath + "/driveRankingNormal.csv";
                break;
            case ScoreType.driveHard:
                rankingCSVPath = Application.dataPath + "/driveRankingHard.csv";
                break;

            case ScoreType.skiSlalom:
                rankingCSVPath = Application.dataPath + "/skiSlalomRanking.csv";
                break;

            case ScoreType.skiAirial:
                rankingCSVPath = Application.dataPath + "/skiAirialRanking.csv";
                break;
            
            default:
                break;

        }
        
        setMyScore = false;
        countNum = 0;

        //得点計算、各項目表示
        switch(difficulty){
            case ScoreType.driveEasy:
            case ScoreType.driveNormal:
            case ScoreType.driveHard:
                Debug.Log("drive");
                myTime = (int)PlayerPrefs.GetFloat("time", 0);
                penartyCount = PlayerPrefs.GetInt("boomNum", 0);
                myScore = 340 + 30 * (int)difficulty + (360 - myTime) - 20 * penartyCount;
                if(myTime%60<10) myTimeTex.text = (myTime / 60) + " : 0" + (myTime % 60);
                else myTimeTex.text = (myTime / 60) + " : " + (myTime % 60);
                myPenartyTex.text = penartyCount +"回";
                myScoreTex.text = myScore + "";
                break;

            case ScoreType.skiSlalom:
                Debug.Log("slalom");
                myTimeRabelTex.text = "滑走タイム";
                myPenartyRabelTex.text = "フラッグ通過数";
                myTime = (int)PlayerPrefs.GetFloat("timeSkiing",0);
                skiingFlagScore = PlayerPrefs.GetInt("flagsPoint",0);
                //skiingJumpScore = PlayerPrefs.GetInt("flagsPoint",0);
                //jumpTimeNum = (int)PlayerPrefs.GetFloat("jumpTimeNum",0);
                skiingFlagCount = PlayerPrefs.GetInt("flagCount",0);
                myScore = (120 - myTime) + skiingFlagScore * 10;
                if(myTime%60<10) myTimeTex.text = (myTime / 60) + " : 0" + (myTime % 60);
                else myTimeTex.text = (myTime / 60) + " : " + (myTime % 60);
                myPenartyTex.text = skiingFlagScore + "/" + 30 +"回";
                myScoreTex.text = myScore + "";
                break;

            case ScoreType.skiAirial:
                myTimeRabelTex.text = "総ジャンプ時間";
                myPenartyRabelTex.text = "技術点";
                skiingJumpScore = PlayerPrefs.GetInt("jumpPoint",0);
                jumpTimeNum = PlayerPrefs.GetFloat("jumpTimeNum",0);
                myScore = skiingJumpScore + (int)jumpTimeNum;
                myTimeTex.text = jumpTimeNum.ToString("f2");
                myPenartyTex.text = skiingJumpScore + "点";
                myScoreTex.text = myScore + "";
                break;
        }

        //名前取得
        myName = PlayerPrefs.GetString("UserName", "Guest");

        //ランク設定

        if(myScore >= SSborder[(int)difficulty]){
            nowRankText = rankAlphabetText[0];
            rankAlphabetShadow.text = "SS";
        }else if(myScore >= Sborder[(int)difficulty]){
            nowRankText = rankAlphabetText[1];
            rankAlphabetShadow.text = "S";
        }else if(myScore >= Aborder[(int)difficulty]){
            nowRankText = rankAlphabetText[2];
            rankAlphabetShadow.text = "A";
        }else if(myScore >= Bborder[(int)difficulty]){
            nowRankText = rankAlphabetText[3];
            rankAlphabetShadow.text = "B";
        }else{
            nowRankText = rankAlphabetText[4];
            rankAlphabetShadow.text = "C";
        }
        //rankAlphabetShadow.enabled = false;

        //ランキング
        StreamReader sr = new StreamReader(rankingCSVPath);
        while(sr.Peek() != -1){
            string[] st = sr.ReadLine().Split(',');
            int score = int.Parse(st[1]);
            if(!setMyScore && myScore > score){
                setMyScore = true;
                rankingNames.Add(myName);
                rankingScores.Add(myScore);
                myRank = countNum + 1;
                if(countNum<5)rankRedLabel[countNum].enabled = true;
            }
            rankingNames.Add(st[0]);
            rankingScores.Add(score);
            countNum++;
        }

        //ランキングの最後まで自分がいなかったら最後に追加
        if(!setMyScore){
            rankingNames.Add(myName);
            rankingScores.Add(myScore);
            if(countNum<5)rankRedLabel[countNum].enabled = true;
            myRank = countNum + 1;
        }

        //リストからランキングをUIに登録
        for(int i=0; i<5; i++){
            if(rankingNames.Count > i){
                rankNameTexts[i].text = rankingNames[i];
                rankScoreTexts[i].text = rankingScores[i] + "";
            }else{
                rankNameTexts[i].text = "None";
                rankScoreTexts[i].text = "0";
            }

        }

        rankNameTexts[5].text = myName + "";
        rankScoreTexts[5].text = myScore + "";
        myRankTex[0].text = myRank + ".";
        myRankTex[1].text = myRank + ".";


        sr.Close();
        
        endTimeCounter = 0;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(endTimeCounter >= 0){
            endTimeCounter += Time.deltaTime;
            timeLimitText.text = "あと"+((int)(endTime - endTimeCounter)) + "秒でタイトルに戻ります";

            nowRankText.enabled = true;
            rankAlphabetShadow.enabled = true;

            //終了時処理
            if(endTimeCounter>= endTime){
                EndResult();
                SceneManager.LoadScene("StartScene");
            }
        }
    }

    public void EndResult(){
        if(myName == "TEST")return;
        //CSVファイルに書き込むときに使うEncoding
        System.Text.Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");
        StreamWriter sw = new StreamWriter(rankingCSVPath, false, enc);
        for(int i=0; i< rankingNames.Count; i++){
            if(i+1 == myRank) sw.WriteLine(myName + ',' + rankingScores[i]);
            else sw.WriteLine(rankingNames[i] + ',' + rankingScores[i]);
        }
        sw.Close();
    }

    void ShowTimeResult(){
        
    }

}
