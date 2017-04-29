using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour {

    public float magnitude;
    public float maxUpwardVelocity;
    public float upwardVelocity;
    private Rigidbody rigidBody;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();

            Bounce(ball.gameObject, ball.GetVelocity);
        }
    }

    private void Bounce(GameObject gameObject, Vector3 vel)
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        upwardVelocity = Math.Abs(vel.y);

        if (upwardVelocity >= maxUpwardVelocity)
        {
            upwardVelocity = maxUpwardVelocity;
        }

        rigidBody.AddForce(transform.forward * upwardVelocity * magnitude, ForceMode.Impulse);
    }
}
