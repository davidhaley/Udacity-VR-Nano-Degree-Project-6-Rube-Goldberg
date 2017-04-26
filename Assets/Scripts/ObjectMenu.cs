using MusicLab.InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ObjectMenu : MonoBehaviour {

    public GameObject objectMenu;
    public List<GameObject> menuObjectHolders;

    private bool touchingTouchpad;

    private int currentObjectIndex;
    private int next;

    private void OnEnable()
    {
        SteamVRControllerEvents.OnTouchpadTouch += OnTouchpadTouch;
        SteamVRControllerEvents.OnTouchpadRelease += OnTouchpadRelease;
        SteamVRControllerEvents.OnTouchpadDown += OnTouchpadDown;
    }

    private void Awake()
    {
        if (objectMenu.activeSelf)
        {
            objectMenu.SetActive(false);
        }
    }
	
	void Update ()
    {
        AttachMenuToRightHand();

        if (touchingTouchpad && !objectMenu.activeSelf)
        {
            objectMenu.SetActive(true);
        }
        else if (!touchingTouchpad && objectMenu.activeSelf)
        {
            objectMenu.SetActive(false);
        }
    }

    private void OnTouchpadTouch(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (e.handOrientation == "RightHand")
        {
            touchingTouchpad = true;
        }
    }

    private void OnTouchpadRelease(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (e.handOrientation == "RightHand")
        {
            touchingTouchpad = false;
        }
    }

    private void OnTouchpadDown(SteamVRControllerEvents.ControllerEventArgs e)
    {
        // The index of the controller with the object menu
        uint menuControllerIndex = GetComponentInParent<Hand>().controller.index;

        if (e.deviceIndex == menuControllerIndex)
        {
            if (e.touchpadAxis.x > 0.5f)
            {
                Debug.Log("clicking right");

            }
            else if (e.touchpadAxis.x < 0.5f)
            {
                Debug.Log("clicking left");

            }
        }
    }

    private void AttachMenuToRightHand()
    {
        Hand hand = Player.instance.rightHand;
        gameObject.transform.SetParent(hand.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.rotation = hand.transform.rotation;
    }
}
