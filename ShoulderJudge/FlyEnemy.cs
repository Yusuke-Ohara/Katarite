using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// FlyEnemmyの挙動（ShoulderDriveカラーボールイベント）
public class FlyEnemy : MonoBehaviour
{
    public GameObject player; //Player
    ShoulderDriveSimple sds;
    public Vector3 startPos; //FlyEnemmy初期位置
    public Vector3 goalPos; //FlyEnemmy移動先の位置
    public float bodyRot;
    public GameObject colorBall; //カラーボール
    ColorBallMove cbm;
    private Sequence sequence;
    ScoreRecorder sr;

    Transform cbTransform; //カラーボールの位置・回転情報
    [SerializeField]
    Vector3 cbStartPos; //カラーボールの位置情報


    void Start()
    {
        sds = player.GetComponent<ShoulderDriveSimple>();
        cbm = colorBall.GetComponent<ColorBallMove>();
        sr = player.GetComponent<ScoreRecorder>();
        cbTransform = colorBall.transform;
        cbStartPos = cbTransform.localPosition;
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity:200, sequencesCapacity:50);
    }

    void Update()
    {
    }

    // イベント開始時FlyEnemy移動（移動先への移動）
    public void FlyEnemyDown(){
        transform.DOLocalMove( goalPos, 1f).OnComplete(() =>
        {
            transform.DOLocalRotate(new Vector3(bodyRot, 0, 0), 1f );
        });
    }

    // イベント終了時FlyEnemmy移動（初期位置に戻る）
    public void FlyEnemyUp(){
        transform.DOLocalMove(
            startPos, // 移動終了地点
            1f                    // 演出時間
        );
        transform.DOLocalRotate(
            new Vector3(0, 0, 0), // 終了時のRotation
            1f                     // 演出時間
        ).OnComplete(()=> {
            sequence.Kill();
        });
    }

    // FlyEnemmyカラーボール発射
    public void FlyEnemyShootAction(){
        transform.DOLocalMove(
            new Vector3(goalPos.x,goalPos.y + 1f, goalPos.z + 1f), 1f).SetEase(Ease.InCubic)                    // 演出時間
        .OnComplete(() =>
        {
            cbm.throwStart();
            transform.DOLocalMove(goalPos, .5f).SetEase(Ease.OutBounce)
            .OnComplete(() => 
            {
                FlyEnemyUp();
            });
        });
    }

    // イベント時の挙動（下降→発射→上昇）
    public void ShotAction(){
        colorBall.SetActive(true);
        Vector3 localPos = cbTransform.localPosition;
        localPos.x = cbStartPos.x;
        localPos.y = cbStartPos.y;
        localPos.z = cbStartPos.z;
        cbTransform.localPosition = localPos;
        sequence = DOTween.Sequence()
        .OnStart(()=>
            {FlyEnemyDown();}
        )
        .PrependCallback(() => 
        {
            FlyEnemyShootAction();
        })
        .PrependInterval(2f);
        sequence.Play();
    }

    public void Enemy_Destroy(){
        Destroy(this.gameObject);
    }
}
