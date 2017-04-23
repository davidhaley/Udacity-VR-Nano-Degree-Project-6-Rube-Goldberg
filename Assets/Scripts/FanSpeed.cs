using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FanSpeed : Fan
{
    public abstract float Speed             { get; }
    public abstract Quaternion DialRotation { get; }
    public abstract AudioClip audioClip     { get; }
}
