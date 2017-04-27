using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Structure : MonoBehaviour {

	private void OnAttachedToHand()
    {
        if (!Ball.BallWithinPlatformBounds)
        {
            Ball.DeactivateBall();
        }
    }
}
