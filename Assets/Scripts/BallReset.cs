using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour {

    private Vector3 resetPosition;
    private Vector3 resetVelocity;

    public delegate void BallTouchedGround();

    public static event BallTouchedGround ballTouchedGround;

    private void Start()
    {
        resetPosition = transform.position;
        resetVelocity = gameObject.GetComponent<Rigidbody>().velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameObject.transform.position = resetPosition;
            gameObject.transform.GetComponent<Rigidbody>().velocity = resetVelocity;

            if (ballTouchedGround != null)
            {
                ballTouchedGround();
            }
        }
    }
}
