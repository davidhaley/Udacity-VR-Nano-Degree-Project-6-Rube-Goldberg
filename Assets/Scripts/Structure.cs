using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Structure : MonoBehaviour {

    public delegate void AttachedToHand();
    public delegate void DetachedFromHand();

    public static event AttachedToHand onAttachedToHand;
    public static event AttachedToHand onDetachedFromHand;

    private void OnAttachedToHand()
    {
        if (onAttachedToHand != null)
        {
            onAttachedToHand();
        }

        //if (!Ball.BallWithinPlatformBounds)
        //{
        //    Ball.DeactivateBall();
        //}
    }

    private void OnDetatchedFromHand()
    {
        if (onDetachedFromHand != null)
        {
            onDetachedFromHand();
        }

        //if (!Ball.BallWithinPlatformBounds)
        //{
        //    Ball.DeactivateBall();
        //}
    }
}
