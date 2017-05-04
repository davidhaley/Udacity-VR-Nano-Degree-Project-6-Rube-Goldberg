using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TutorialBounds : MonoBehaviour {

    public GameObject teleportPointHolder;
    private bool playerInTutorialBounds;
    private bool tutorialComplete = false;

    private void OnEnable()
    {
        Introduction.OnTutorialComplete += OnTutorialComplete;
    }

    private void OnDisable()
    {
        Introduction.OnTutorialComplete -= OnTutorialComplete;
    }

    private void Awake()
    {
        if (teleportPointHolder.gameObject.activeSelf)
        {
            teleportPointHolder.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        // Track player's head (has existing rigidbody)
        if (col.gameObject.name == "HeadCollider" && !tutorialComplete)
        {
            playerInTutorialBounds = true;

            if (teleportPointHolder.gameObject.activeSelf)
            {
                teleportPointHolder.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "HeadCollider" && !tutorialComplete)
        {
            playerInTutorialBounds = false;

            if (!teleportPointHolder.gameObject.activeSelf)
            {
                teleportPointHolder.gameObject.SetActive(true);
            }
        }
    }

    public bool PlayerInTutorialBounds
    {
        get { return playerInTutorialBounds; }
    }

    private void OnTutorialComplete()
    {
        tutorialComplete = true;
    }
}
