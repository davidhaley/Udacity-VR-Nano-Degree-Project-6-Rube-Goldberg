using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FanSpeedMed : FanSpeed
{
    private Image image;

    public override float Speed
    {
        get { return 4f; }
    }

    public override Quaternion DialRotation
    {
        get { return Quaternion.Euler(0f, 0f, 45f); }
    }

    public override AudioClip audioClip
    {
        get
        {
            AudioClip clip = Resources.Load<AudioClip>("Sounds/Fan/FanMed");
            return clip;
        }
    }
}
