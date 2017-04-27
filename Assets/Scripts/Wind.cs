using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Purpose: Apply wind force to object. This is attached to a wind particle system game object.
//          When the particle system is enabled (when fan is turned on), and the object enters and
//          Stays in the collision zone, the force will be applied.

public class Wind : MonoBehaviour {

    public float force = 100000f; // Force magnitude required for the ball to act in a realistic manner

    private Collider ball;
    private Rigidbody ballRigidbody;

    private float radius;
    private float surfaceArea;
    private float distance;
    private float appliedForce;

    private bool ballInWindZone;

    private void Update()
    {
        if (ball != null)
        {
            // Distance between the fan and the ball
            distance = Vector3.Distance(transform.position, ball.transform.position);
        }
    }

    public float Force
    {
        get { return force; }

        set { force = value; }
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
        if (col.gameObject.CompareTag("Ball"))
        {
            ball = null;
        }
    }

    private void ApplyWindForce(Collider col)
    {
        if (surfaceArea == 0)
        {
            ballRigidbody = ball.GetComponent<Rigidbody>();
            radius = ball.GetComponent<SphereCollider>().radius;
            surfaceArea = Mathf.Pow((2 * Mathf.PI * radius), 2);
        }

        if (distance < 1f)
        {
            distance = 1f;
        }

        appliedForce = ((force / (1 + distance * distance)) * surfaceArea);

        ballRigidbody.AddForce(gameObject.transform.forward * appliedForce);
    }
}
