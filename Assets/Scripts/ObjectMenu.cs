using MusicLab.InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Valve.VR.InteractionSystem;

public class ObjectMenu : MonoBehaviour {

    public GameObject objectMenu;
    public List<GameObject> menuObjectHolders;

    private static Hand rightHand;
    private bool touchingRightTouchpad;
    private static bool firstTimeShown = true;

    private int currentObjectIndex = 0;
    private int prevObjectIndex;

    private PlaySound menuScrollSound;

    private void OnEnable()
    {
        SteamVRControllerEvents.OnTouchpadTouch += OnTouchpadTouch;
        SteamVRControllerEvents.OnTouchpadRelease += OnTouchpadRelease;
        SteamVRControllerEvents.OnTouchpadDown += OnTouchpadDown;
    }

    private void OnDisable()
    {
        SteamVRControllerEvents.OnTouchpadTouch -= OnTouchpadTouch;
        SteamVRControllerEvents.OnTouchpadRelease -= OnTouchpadRelease;
        SteamVRControllerEvents.OnTouchpadDown -= OnTouchpadDown;
    }

    private void Awake()
    {
        LoadAudio();

        if (objectMenu.activeSelf)
        {
            objectMenu.SetActive(false);
        }
    }

    void Update ()
    {
        rightHand = Player.instance.hands[(int)Hand.HandType.Right];

        if (rightHand != null)
        {
            if (touchingRightTouchpad)
            {
                if (!objectMenu.activeSelf)
                {
                    objectMenu.SetActive(true);
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
    }

    private bool FirstTimeShown
    {
        get { return firstTimeShown; }
    }

    private void OnTouchpadTouch(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (e.fixedHandOrientation == "Right")
        {
            touchingRightTouchpad = true;
        }
    }

    private void OnTouchpadRelease(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (e.fixedHandOrientation == "Right")
        {
            touchingRightTouchpad = false;
        }
    }

    private void OnTouchpadDown(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (e.fixedHandOrientation == "Right")
        {
            if (e.touchpadAxis.x > 0.5f || e.touchpadAxis.x < 0.5f)
            {
                prevObjectIndex = currentObjectIndex;
                firstTimeShown = false;
                menuScrollSound.Play();
            }
            if (e.touchpadAxis.x > 0.5f)
            {
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

    private void LoadAudio()
    {
        menuScrollSound = SoundManager.LoadAudio(gameObject, new List<string> { "Sounds/Effects/MenuScroll" }, 0.20f, false, false, false, "Effects");
    }
}
