using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ユーザ名入力欄（InputField）の制御
public class InputFieldName : MonoBehaviour
{

    private InputField name;
    private int userNumber;
    bool test;

    void Start()
    {
        name = this.gameObject.GetComponent<InputField>();
        name.ActivateInputField();
        name.onEndEdit.AddListener(
            delegate{
                name.ActivateInputField();
            }
        );
        //初期状態は（Gest＋体験者数）
        userNumber =  PlayerPrefs.GetInt("guestNum",1);
        name.text  = "Guest" + userNumber;
    }

    void Update()
    {
        // Altキーでテストモードに切り替え
        Debug.Log("test");
        if(Input.GetKeyDown(KeyCode.RightAlt)){
            test = true;
            name.text  = "TEST";
        }
        if(Input.GetKeyDown(KeyCode.LeftAlt)){
            name.text  = "Guest" + userNumber;
            test = false;
        }
    }
}
