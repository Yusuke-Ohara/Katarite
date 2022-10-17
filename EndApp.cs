using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Escapeキーでアプリ終了
public class EndApp : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }
    }
}
