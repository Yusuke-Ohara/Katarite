using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// タイヤの回転制御
public class WheelRotate : MonoBehaviour
{
    [SerializeField] private float wheelRadius = 1.0f; // タイヤの半径
    [SerializeField] private Transform carTransform; // 車本体のTransform
    private Vector3 beforePos; // 前のポジションを格納
    private float rotationSign; // 回転方向の符号

    void Start()
    {
        // タイヤの順回転・逆回転を決める
        // タイヤのX軸が車本体のX軸に向くよう取り付けられていれば1、逆向きなら-1
        rotationSign = Mathf.Sign(Vector3.Dot(carTransform.right, transform.right));

        // beforePosの初期値を設定
        beforePos = transform.position;
    }

    void Update()
    {
        wheelRotate();
    }

    private void wheelRotate()
    {
        // ワールド空間における位置変化を計算
        var worldDeltaPos = transform.position - beforePos;

        // 車本体のローカル空間に修正
        var localDeltaPos = carTransform.InverseTransformDirection(worldDeltaPos);
        
        // Z成分から回転量を計算
        transform.Rotate(rotationSign * Mathf.Rad2Deg * localDeltaPos.z / wheelRadius, 0, 0);

        // beforePosを更新
        beforePos = transform.position;
    }
}
