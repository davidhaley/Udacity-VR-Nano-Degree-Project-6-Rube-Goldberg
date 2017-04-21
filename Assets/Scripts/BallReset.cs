﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour {

    private Vector3 resetPosition;
    private Vector3 resetVelocity;

    private void Start()
    {
        resetPosition = transform.position;
        resetVelocity = gameObject.GetComponent<Rigidbody>().velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("his the ground");
            gameObject.transform.position = resetPosition;
            gameObject.transform.GetComponent<Rigidbody>().velocity = resetVelocity;
        }
    }
}
