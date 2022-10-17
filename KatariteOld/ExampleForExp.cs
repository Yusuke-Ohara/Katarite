using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ExampleForExp : MonoBehaviour
{
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;
    public float max_x, max_y, max_z, min_x, min_y, min_z, maxR_x, maxR_y, maxR_z, minR_x, minR_y, minR_z, maxL_x, maxL_y, maxL_z, minL_x, minL_y, minL_z;
    public float normalR_x, normalR_y, normalR_z, normalL_x, normalL_y, normalL_z;
    float accel_R_x, accel_R_y, accel_R_z, accel_L_x, accel_L_y, accel_L_z, x_R, y_R, z_R, x_L, y_L, z_L;

    public Text expText;

    public Toggle JumpT;
    public float Jumpx;
    //�l�ۑ��p�N���X (�ҏW�� �ѐ�)
    private JoyconData joyconData;
    ////////////////////////////////




    private void Start()
    {
        m_joycons = JoyconManager.Instance.j;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);


        //�l�ۑ��p�N���X��` (�ҏW�� �ѐ�)
        joyconData = GetComponent<JoyconData>();
        ////////////////////////////////
        
        Load();
    }

    private void Update()
    {
        m_pressedButtonL = null;
        m_pressedButtonR = null;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        // foreach (var button in m_buttons)
        // {
        //     if (m_joyconL.GetButton(button))
        //     {
        //         m_pressedButtonL = button;
        //     }
        //     if (m_joyconR.GetButton(button))
        //     {
        //         m_pressedButtonR = button;
        //     }
        // }

        // if (Input.GetKeyDown(KeyCode.Z))
        // {
        //     m_joyconL.SetRumble(160, 320, 0.6f, 200);
        // }
        // if (Input.GetKeyDown(KeyCode.X))
        // {
        //     m_joyconR.SetRumble(160, 320, 0.6f, 200);
        // }

        // �E���̒l��1.0�`0.0�A�����̒l��0.0�`-1.0�͈̔͂ŏ����_�ȉ��Q���ɂ��Ă��܂��B�����ς�����������A����100�̌���ς��Ă�������
        // ���̏グ�����Ɋւ��ẮAx_R,x_L�̒l��p����Α��v���Ǝv���܂�
        // max_R,min_R,max_L,max_L�ɂ��ꂼ��L�����u���[�V�����ő��肵�����̂�ݒ肷��
        var accel_R = m_joyconR.GetAccel();
        accel_R_x = accel_R.x - normalR_x;
        // accel_R_y = accel_R.y - normalR_y;
        // accel_R_z = accel_R.z - normalR_z;
        if (accel_R_x >= 0)
        {
            x_R = (Mathf.Floor((accel_R_x / (maxR_x - normalR_x)) * 100)) / 100;
        }
        else
        {
            // x_R = (Mathf.Floor((accel_R_x / (maxL_x - normalL_x)) * 100)) / 100;
            x_R = 0;
        }
        // if (accel_R_y >= 0)
        // {
        //     y_R = -(Mathf.Floor((accel_R_y / (maxR_y - normalR_y)) * 100)) / 100;
        // }
        // else
        // {
        //     y_R = -(Mathf.Floor((accel_R_y / (maxL_y - normalL_y)) * 100)) / 100;
        //     // y_R = 0;
        // }
        // if (accel_R_z >= 0)
        // {
        //     z_R = (Mathf.Floor((accel_R_z / (maxR_z - normalR_z)) * 100)) / 100;
        // }
        // else
        // {
        //     z_R = (Mathf.Floor((accel_R_z / (maxL_z - normalL_z)) * 100)) / 100;
        // }

        var accel_L = m_joyconL.GetAccel();
        accel_L_x = accel_L.x - normalL_x;
        // accel_L_y = accel_L.y - normalL_y;
        // accel_L_z = accel_L.z - normalL_z;
        if (accel_L_x >= 0)
        {
            x_L = (Mathf.Floor((accel_L_x / (maxL_x - normalL_x)) * 100)) / 100;
        }
        else
        {
            // x_L = -(Mathf.Floor((accel_L_x / (minL_x - normalL_x)) * 100)) / 100;
            x_L = 0;
        }
        // if (accel_L_y >= 0)
        // {
        //     y_L = (Mathf.Floor((accel_L_y / (maxL_y - normalL_y)) * 100)) / 100;
        // }
        // else
        // {
        //     y_L = (Mathf.Floor((accel_L_y / (maxR_y - normalR_y)) * 100)) / 100;
        //     // y_L = 0;
        // }
        // if (accel_L_z >= 0)
        // {
        //     z_L = -(Mathf.Floor((accel_L_z / (maxR_z - normalL_z)) * 100)) / 100;
        // }
        // else
        // {
        //     z_L = -(Mathf.Floor((accel_L_z / (minR_z - normalL_z)) * 100)) / 100;
        // }

        expText.text = "y_L " + x_L + " y_R " + x_R;
        
        //�l�ۑ��p�N���X�փf�[�^�o�^(�ҏW�� �ѐ�)
        joyconData.setHundle(x_R, x_L, y_R, y_L);
        ////////////////////////

        if (Math.Abs(x_L) >= Jumpx && Math.Abs(x_R) >= Jumpx)
        {
            JumpT.isOn = true;
            // Debug.Log(Math.Abs(x_L));
        }

    }

    //

    //�ԑ������v�����Ajoycon�N���X��n���֐�(�ҏW��: �ѐ�)
    public JoyconData GetJoyconData()
    {
        return joyconData;
    }

    public void Load()
    {
        maxR_x = PlayerPrefs.GetFloat("MaxR_x", 0);
        maxR_y = PlayerPrefs.GetFloat("MaxR_y", 0);
        maxR_z = PlayerPrefs.GetFloat("MaxR_z", 0);
        minR_x = PlayerPrefs.GetFloat("MinR_x", 0);
        minR_y = PlayerPrefs.GetFloat("MinR_y", 0);
        minR_z = PlayerPrefs.GetFloat("MinR_z", 0);
        maxL_x = PlayerPrefs.GetFloat("MaxL_x", 0);
        maxL_y = PlayerPrefs.GetFloat("MaxL_y", 0);
        maxL_z = PlayerPrefs.GetFloat("MaxL_z", 0);
        minL_x = PlayerPrefs.GetFloat("MinL_x", 0);
        minL_y = PlayerPrefs.GetFloat("MinL_y", 0);
        minL_z = PlayerPrefs.GetFloat("MinL_z", 0);
        normalR_x = PlayerPrefs.GetFloat("avenormalR_x", 0);
        normalR_y = PlayerPrefs.GetFloat("avenormalR_y", 0);
        normalR_z = PlayerPrefs.GetFloat("avenormalR_z", 0);
        normalL_x = PlayerPrefs.GetFloat("avenormalL_x", 0);
        normalL_y = PlayerPrefs.GetFloat("avenormalL_y", 0);
        normalL_z = PlayerPrefs.GetFloat("avenormalL_z", 0);
    }
}

