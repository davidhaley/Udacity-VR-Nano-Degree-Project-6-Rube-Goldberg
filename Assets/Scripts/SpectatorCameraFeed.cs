using MusicLab.InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCameraFeed : MonoBehaviour {

    public GameObject spectatorCameraFeed;
    public GameObject noCameraCanvasHolder;

    private bool spectatorCameraExists = false;

    private void Awake()
    {
        if (spectatorCameraFeed.gameObject.activeSelf)
        {
            spectatorCameraFeed.gameObject.SetActive(false);
        }

        if (noCameraCanvasHolder.gameObject.activeSelf)
        {
            noCameraCanvasHolder.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        ObjectMenuSelector.OnSpectatorCameraInstantiated += OnSpectatorCameraInstantiated;
        SteamVRControllerEvents.OnGripDown += OnGripDown;
        SteamVRControllerEvents.OnGripUp += OnGripUp;
    }

    private void OnSpectatorCameraInstantiated()
    {
        spectatorCameraExists = true;
    }

    private void OnGripDown(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (e.fixedHandOrientation == "Left")
        {
            if (!spectatorCameraExists)
            {
                noCameraCanvasHolder.gameObject.SetActive(true);
            }
            else
            {
                noCameraCanvasHolder.gameObject.SetActive(false);
            }

            spectatorCameraFeed.gameObject.SetActive(true);
        }
    }

    private void OnGripUp(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (e.fixedHandOrientation == "Left")
        {
            spectatorCameraFeed.gameObject.SetActive(false);
        }
    }
}
