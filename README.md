# Katarite
大原侑祐　担当箇所
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
