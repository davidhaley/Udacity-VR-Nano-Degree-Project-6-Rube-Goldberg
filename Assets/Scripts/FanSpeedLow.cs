using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FanSpeedLow : FanSpeed
{
    private Image image;

    public override float Speed
    {
        get { return 2f; }
    }

    public override Quaternion DialRotation
    {
        get { return Quaternion.Euler(0f, 0f, 0f); }
    }

    public override AudioClip audioClip
    {
        get
        {
            AudioClip clip = Resources.Load<AudioClip>("Sounds/Fan/FanLow");
            return clip;
        }
    }
}
