using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDestination : MonoBehaviour {

    public bool changePosition;
    public Transform end;
    public float speed;

    private Vector3 startPosition;
    private bool towardEnd = true;
    private float closeDistance = 0.025f;

    private Vector3 offset;
    private float sqrLen;

    private void Awake()
    { 
        startPosition = gameObject.transform.position;
    }

    private void Update()
    {
        if (changePosition)
        {
            float step = speed * Time.deltaTime;

            if (towardEnd)
            {
                offset = end.position - transform.position;
                sqrLen = offset.sqrMagnitude;

                if (sqrLen > closeDistance * closeDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, end.position, step);
                }
                else
                {
                    towardEnd = false;
                }
            }
            else if (!towardEnd)
            {
                offset = startPosition - transform.position;
                sqrLen = offset.sqrMagnitude;

                if (sqrLen > closeDistance * closeDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, startPosition, step);
                }
                else
                {
                    towardEnd = true;
                }
            }
        }
    }

    public bool ChangingPosition
    {
        get { return changePosition; }
    }
}
