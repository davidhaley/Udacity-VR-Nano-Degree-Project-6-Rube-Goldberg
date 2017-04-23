using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FanSpeedHigh : FanSpeed
{
    private Image image;

    public override float Speed
    {
        get
        {
            return 6f;
        }
    }

    public override Quaternion DialRotation
    {
        get
        {
            return Quaternion.Euler(0f, 0f, 90f);
        }
    }
}
