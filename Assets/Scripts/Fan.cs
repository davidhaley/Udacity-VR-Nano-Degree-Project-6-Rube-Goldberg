using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour {

    public GameObject fanBlades;

    private float speed = 4f;

    private void Update()
    {
        // One 360 degree rotation per second, muliplied by speed (ie. 4 means 4 360 degree rotations per second)
        fanBlades.transform.Rotate(Vector3.forward, (Time.deltaTime * 360) * speed);
    }
}
