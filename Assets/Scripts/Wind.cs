﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {

    public float force = 100000f; // Force magnitude required for the ball to act in a realistic manner

    private Collider ball;

    private float radius;
    private float surfaceArea;
    private float distance;
    private float appliedForce;

    private void Update()
    {
        if (ball != null)
        {
            // Distance between the fan and the ball
            distance = Vector3.Distance(transform.position, ball.transform.position);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            ball = col;
            ApplyWindForce(ball);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Ball") && ball != null)
        {
            ball = null;
        }
    }

    private void ApplyWindForce(Collider col)
    {
        Rigidbody rigidBody = ball.GetComponent<Rigidbody>();
        radius = ball.GetComponent<SphereCollider>().radius;
        surfaceArea = Mathf.Pow((2 * Mathf.PI * radius), 2);

        if (distance < 1f)
        {
            distance = 1f;
        }

        appliedForce = ((force / (1 + distance * distance)) * surfaceArea);

        rigidBody.AddForce(gameObject.transform.forward * appliedForce);
    }
}
