using MusicLab.InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCameraFeed : MonoBehaviour {

    public GameObject spectatorCamera;

    private void Awake()
    {
        if (spectatorCamera.gameObject.activeSelf)
        {
            spectatorCamera.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        SteamVRControllerEvents.OnGripDown += OnGripDown;
        SteamVRControllerEvents.OnGripUp += OnGripUp;
    }

    private void OnGripDown(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (e.fixedHandOrientation == "Left")
        {
            Debug.Log("left grip down");
            spectatorCamera.gameObject.SetActive(true);
        }
    }

    private void OnGripUp(SteamVRControllerEvents.ControllerEventArgs e)
    {
        Debug.Log("left grip down");

        if (e.fixedHandOrientation == "Left")
        {
            spectatorCamera.gameObject.SetActive(false);
        }
    }
}
