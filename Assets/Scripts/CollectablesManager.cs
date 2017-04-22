﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesManager : MonoBehaviour {

    public static GameObject[] collectables;

    private void OnEnable()
    {
        BallReset.ballTouchedGround += OnBallTouchedGround;
    }

    private void Start()
    {
        collectables = GameObject.FindGameObjectsWithTag("Collectable");
    }

    private void OnBallTouchedGround()
    {
        ResetCollectables();
    }

    private void ResetCollectables()
    {
        for (int i = 0; i < collectables.Length; i++)
        {
            collectables[i].gameObject.SetActive(true);
        }
    }

    public static int CollectablesRemaining()
    {
        if (collectables != null)
        {
            int active = 0;

            for (int i = 0; i < collectables.Length; i++)
            {
                if (collectables[i].gameObject.activeSelf)
                {
                    active += 1;
                }
            }

            return active;
        }
        else
        {
            return -1;
        }
    }
}
