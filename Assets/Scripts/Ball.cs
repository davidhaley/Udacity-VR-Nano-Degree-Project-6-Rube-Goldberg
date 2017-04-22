using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Purpose: Announce ball collisions and reset ball to start position when it touches the ground

public class Ball : MonoBehaviour {

    public delegate void BallTouchedGround();
    public delegate void BallTouchedGoal();
    public delegate void BallTouchedCollectable();

    public static event BallTouchedGround ballTouchedGround;
    public static event BallTouchedGround ballTouchedGoal;
    public static event BallTouchedGround ballTouchedCollectable;

    private Vector3 resetPosition;
    private Vector3 resetVelocity;

    private void Start()
    {
        InitializeBall();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ResetBall();

            if (ballTouchedGround != null)
            {
                ballTouchedGround();
            }

        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            if (ballTouchedGoal != null)
            {
                ballTouchedGoal();
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Collectable"))
        {
            col.gameObject.SetActive(false);

            if (ballTouchedCollectable != null)
            {
                ballTouchedCollectable();
            }
        }
    }

    private void InitializeBall()
    {
        resetPosition = transform.position;
        resetVelocity = gameObject.GetComponent<Rigidbody>().velocity;
    }

    private void ResetBall()
    {
        gameObject.transform.position = resetPosition;
        gameObject.transform.GetComponent<Rigidbody>().velocity = resetVelocity;
    }
}