# Katarite

## 作品概要
Katariteは、手の不自由な人でも楽しめることをコンセプトにした、両肩を使って操作するゲームです。
体験者は、両肩にJoy-Conを装着し、肩を上げる、肩をひねるなど肩を動かして遊びます。
このゲームは、「ShoulderDrive」と「ShoulderSki」の2つの種類があります。<br>

### ゲームで使用する肩の動き
![肩の動かし方](https://user-images.githubusercontent.com/66463926/201526243-a13d9840-34a3-4563-b028-b975482a6e1e.png)

### ShoulderDrive
運転席からの視点を見ながら、障害物を避け、ゴールを目指すカーゲームです。Easy,Normal,Hardの3つの難易度があります。<br><br>
<肩の動きとそれに対応する操作方法><br>
・片方の肩を上げる→ハンドル操作<br>
・両肩を上げる→ジャンプ<br>
・肩をひねる→ワイパー<br>

### ShoulderSki
スキープレイヤーを操作し、2つのモードを遊ぶことができるスキーゲームです。2つのフラッグの間を通り抜けていくスラロームモード、ジャンプの滞空時間とジャンプアクションの芸術点を競うエアリアルモードの2つのモードがあります。<br><br>
<肩の動きとそれに対応する操作方法><br>
・片方の肩を上げる→プレイヤーの向きを変える<br>
・両肩を上げる→ジャンプ<br>
・肩をひねる→ジャンプ中の回転<br>
・しゃがむ→加速<br>

### HP
https://www.notion.so/Katarite-42af3c63a37c43d1a6bea9b3f09fe200

## 大原侑祐　担当箇所  
・EndApp.cs ：　Escapeキーでアプリ終了  
・Mouse.cs　：　マウスカーソルの表示制御  
・Tut/Tutrial.cs　：　ShoulderDriveのチュートリアル  
・Title/InputFieldName.cs　：　ユーザ名入力欄（InputField）の制御　（タイトル画面）  
・Title/JoyconD.cs　：　Joy-Con未接続時に状態提示（タイトル画面）  
・Title/PvScene.cs　：　PVシーン（タイトル画面で一定時間入力がない場合にPV再生）  
・Title/SelectMode.cs　：　タイトル画面制御（ユーザ名入力～モード選択）  
・ShoulderJudge/Calibration.cs　：　キャリブレーション  
・ShoulderJudge/FlyEnemy.cs　：　FlyEnemmyの挙動（ShoulderDriveカラーボールイベント）  
・ShoulderJudge/GimmickManager.cs　：　難易度によるギミック（樽，ジャンプ台，ダッシュアイテム，FlyEnemmy）の変更（ShoulderDrive）  
・ShoulderJudge/Respown.cs　：　ゲーム中，車が進行不能時に最も近いセーブポイントに戻る（ShoulderDrive）  
・ShoulderJudge/TimerManager.cs　：　プレイ画面に表示するタイマーの制御（ShoulderDrive）  
・SceneMove/ImgMove.cs　：　画像の移動（タイトル画面）  
・SceneMove/JoyDis.cs　：　Joy-Conの接続状況の提示（ゲームシーン）  
・SceneMove/SceneMove.cs　：　シーン遷移　右Shiftでシーンリロード  
・JoyconLib_scripts/Example.cs(FixedUpdate関数,Load関数)　：　＜FixedUpdate＞Joy-Conの各センサーの計測値の操作への反映、＜Load＞キャリブレーションで保存した計測値の取得  
