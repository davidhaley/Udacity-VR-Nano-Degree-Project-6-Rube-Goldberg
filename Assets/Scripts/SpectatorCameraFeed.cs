using MusicLab.InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpectatorCameraFeed : MonoBehaviour {

    private GameObject spectatorCameraFeed;
    private GameObject noCameraCanvasHolder;

    private bool spectatorCameraExists = false;

    private void Awake()
    {
        //spectatorCameraFeed = gameObject.transform.Find("SpectatorCameraFeed").gameObject;
        //noCameraCanvasHolder = gameObject.transform.Find("NoCameraCanvasHolder").gameObject;
        InitializeGameObjects();

    }

    private void InitializeGameObjects()
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in transforms)
        {
            if (t.name == "SpectatorCameraFeed")
            {
                spectatorCameraFeed = t.gameObject;
            }
            else if (t.name == "NoCameraCanvasHolder")
            {
                noCameraCanvasHolder = t.gameObject;
            }
        }

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

    private void OnDisable()
    {
        ObjectMenuSelector.OnSpectatorCameraInstantiated -= OnSpectatorCameraInstantiated;
        SteamVRControllerEvents.OnGripDown -= OnGripDown;
        SteamVRControllerEvents.OnGripUp -= OnGripUp;
    }

    //private void OnLevelFinishedLoading(Scene scene, LoadSceneMode loadSceneMode)
    //{
    //    Debug.Log("inside on level finished loading: " + noCameraCanvasHolder);

    //    InitializeGameObjects();
    //}

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
