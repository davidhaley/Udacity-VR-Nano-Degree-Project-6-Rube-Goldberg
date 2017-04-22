using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour {

    public float velocityMultiplier = 2f;
    public float bounceMultiplier = 2f;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            Bounce(col.gameObject);
        }
    }

    private void Bounce(GameObject gameObject)
    {
        Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();

        rigidBody.angularVelocity *= velocityMultiplier;
        rigidBody.AddForce(0, bounceMultiplier, 0, ForceMode.Impulse);
    }
}
