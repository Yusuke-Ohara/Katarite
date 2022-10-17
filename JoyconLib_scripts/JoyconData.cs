using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//joycon�ǂݎ��X�N���v�g����f�[�^��ǂݎ��p�̃N���X�ł��B
//���̃N���X��joycon�֘A�̐M���͂��ׂĊi�[���Ă����܂��B
public class JoyconData : MonoBehaviour
{
   
    //xr, xl �n���h������p�l
    private double xr, xl, yr, yl;

    //jump����p�l
    private bool jump;

    void Start()
    {
        xr = 0; xl = 0;
    }

    //�l�ݒ�p
    public void setHundle(double xr, double xl, double yr, double yl)
    {
        this.xr = xr;
        this.xl = xl;
        this.yr = yr;
        this.yl = yl;
    }

    public void setJump(bool j)
    {
        jump = j;
    }

    //�l�ǂݎ��p
    public double readXL()
    {
        return xl;
    }

    public double readXR()
    {
        return xr;
    }

    public double readYL()
    {
        return yl;
    }

    public double readYR()
    {
        return yr;
    }


}
