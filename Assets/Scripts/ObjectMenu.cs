using MusicLab.InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ObjectMenu : MonoBehaviour {

    public GameObject objectMenu;
    public List<GameObject> menuObjectHolders;

    private Hand rightHand;
    private uint rightControllerIndex;
    private bool touchingRightTouchpad;
    private static bool firstTimeShown = true;

    private int currentObjectIndex = 0;
    private int prevObjectIndex;

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

        rightHand = Player.instance.rightHand;
    }

    void Update ()
    {
        if (rightHand != null)
        {
            if (touchingRightTouchpad)
            {
                if (!objectMenu.activeSelf)
                {
                    objectMenu.SetActive(true);
                }
                
                if (firstTimeShown && transform.parent != rightHand.transform)
                {
                    MenuParentRightHand();
                }

                // Disallow right hand to interact with objects when showing menu
                rightHand.hoverLayerMask = 0;

                // Show current menu object if it is hidden
                if (!menuObjectHolders[currentObjectIndex].gameObject.activeSelf)
                {
                    menuObjectHolders[currentObjectIndex].gameObject.SetActive(true);
                }

                // Hide previous menu object if it is showing
                if (!firstTimeShown && menuObjectHolders[prevObjectIndex].gameObject.activeSelf)
                {
                    menuObjectHolders[prevObjectIndex].gameObject.SetActive(false);
                }
            }
            else if (!touchingRightTouchpad)
            {
                if (objectMenu.activeSelf)
                {
                    objectMenu.SetActive(false);
                }

                // Resume interactable layer mask so player can interact with objects using the right hand
                rightHand.hoverLayerMask = 2048;

                menuObjectHolders[currentObjectIndex].SetActive(false);
            }
        }
        else
        {
            UpdateRightHand();
        }
    }

    public static bool FirstTimeShown
    {
        get { return firstTimeShown; }
    }

    private void OnTouchpadTouch(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (e.deviceIndex == rightControllerIndex)
        {
            touchingRightTouchpad = true;
        }
    }

    private void OnTouchpadRelease(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (e.deviceIndex == rightControllerIndex)
        {
            touchingRightTouchpad = false;
        }
    }

    private void OnTouchpadDown(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (e.deviceIndex == rightControllerIndex)
        {
            if (e.touchpadAxis.x > 0.5f)
            {
                prevObjectIndex = currentObjectIndex;
                firstTimeShown = false;

                if (currentObjectIndex + 1 > menuObjectHolders.Count - 1)
                {
                    // Out of range, loop to beginning
                    currentObjectIndex = 0;
                }
                else
                {
                    currentObjectIndex += 1;
                }
            }
            else if (e.touchpadAxis.x < 0.5f)
            {
                prevObjectIndex = currentObjectIndex;
                firstTimeShown = false;

                if (currentObjectIndex - 1 < 0)
                {
                    // Out of range, loop to end
                    currentObjectIndex = menuObjectHolders.Count - 1;
                }
                else
                {
                    currentObjectIndex -= 1;
                }
            }
        }
    }

    private void UpdateRightHand()
    {
        rightHand = Player.instance.rightHand;
        rightControllerIndex = rightHand.controller.index;
    }

    private void MenuParentRightHand()
    {
        gameObject.transform.SetParent(rightHand.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.rotation = rightHand.transform.rotation;
    }
}
