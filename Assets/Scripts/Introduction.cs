using MusicLab.InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Introduction : MonoBehaviour {

    public Text introduction;

    private Valve.VR.EVRButtonId touchpad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private Coroutine hintCoroutine;
    private int hintCounter = 0;
    private bool finishedHints = false;
    private bool cycleMenuHintActive;

    private void OnEnable()
    {
        SteamVRControllerEvents.OnTouchpadTouch += OnTouchpadTouch;
        SteamVRControllerEvents.OnTouchpadDown += OnTouchpadDown;
        SteamVRControllerEvents.OnTriggerDown += OnTriggerDown;
    }

    private void OnDisable()
    {
        SteamVRControllerEvents.OnTouchpadTouch -= OnTouchpadTouch;
        SteamVRControllerEvents.OnTouchpadDown -= OnTouchpadDown;
        SteamVRControllerEvents.OnTriggerDown -= OnTriggerDown;
    }

    private void Awake()
    {
        if (introduction.gameObject.activeSelf == false)
        {
            introduction.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        StartCoroutine(BeginHintSequence());
    }

    private void Update()
    {
        if (hintCoroutine == null && !finishedHints)
        {
            if (hintCounter + 1 >= 5)
            {
                finishedHints = true;
            }
            else
            {
                hintCounter += 1;
                StartCoroutine(BeginHintSequence());
            }
        }
    }

    private IEnumerator BeginHintSequence()
    {
        if (hintCounter == 0)
        {
            hintCoroutine = StartCoroutine(ShowObjectMenuHint());
        }
        else if (hintCounter == 1)
        {
            hintCoroutine = StartCoroutine(CycleObjectMenuHint());
        }
        else if (hintCounter == 2)
        {
            hintCoroutine = StartCoroutine(GrabObjectHint());
        }
        else if (hintCounter == 3)
        {
            hintCoroutine = StartCoroutine(TeleportHint());
        }

        yield return null;
    }

    private IEnumerator ShowObjectMenuHint()
    {
        yield return new WaitForSeconds(5f);

        ShowHint(Player.instance.rightHand, touchpad, "Touch To Show Object Menu");
    }

    private IEnumerator CycleObjectMenuHint()
    {
        yield return new WaitForSeconds(7f);

        while (Player.instance.rightHand.controller.GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad))
        {
            yield return new WaitForSeconds(1f);
        }

        // Wait for 2 seconds after the player finishes with the touchpad
        yield return new WaitForSeconds(2f);

        cycleMenuHintActive = true;
        ShowHint(Player.instance.rightHand, touchpad, "Press Down To Cycle Through Object Menu");
    }

    private IEnumerator GrabObjectHint()
    {
        yield return new WaitForSeconds(7f);

        while (Player.instance.rightHand.controller.GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad))
        {
            yield return new WaitForSeconds(1f);
        }

        // Wait for 2 seconds after the player finishes with the touchpad again...
        yield return new WaitForSeconds(2f);

        ShowHint(Player.instance.leftHand, trigger, "Grab Object from Menu");
    }
    private IEnumerator TeleportHint()
    {
        yield return new WaitForSeconds(5f);

        //ShowHint(Player.instance.leftHand, touchpad, "Teleport");

        Teleport.instance.ShowTeleportHint();
        yield return null;

    }

    //--------
    // Events
    //--------

    private void OnTouchpadTouch(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (e.fixedHandOrientation == "Right")
        {
            bool isShowingHint = !string.IsNullOrEmpty(ControllerButtonHints.GetActiveHintText(Player.instance.rightHand, touchpad));

            if (isShowingHint && !cycleMenuHintActive)
            {
                CancelHint(Player.instance.rightHand, touchpad);
            }
        }
    }

    private void OnTouchpadDown(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (e.fixedHandOrientation == "Right")
        {
            bool isShowingHint = !string.IsNullOrEmpty(ControllerButtonHints.GetActiveHintText(Player.instance.rightHand, touchpad));

            if (isShowingHint && cycleMenuHintActive)
            {
                CancelHint(Player.instance.rightHand, touchpad);
                cycleMenuHintActive = false;
            }
        }
        else if (e.fixedHandOrientation == "Left")
        {
            bool isShowingHint = !string.IsNullOrEmpty(ControllerButtonHints.GetActiveHintText(Player.instance.leftHand, touchpad));

            if (isShowingHint)
            {
                CancelHint(Player.instance.leftHand, touchpad);
            }
        }
    }

    private void OnTriggerDown(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (e.fixedHandOrientation == "Left")
        {
            bool isShowingHint = !string.IsNullOrEmpty(ControllerButtonHints.GetActiveHintText(Player.instance.leftHand, trigger));

            if (isShowingHint)
            {
                CancelHint(Player.instance.leftHand, trigger);
            }
        }
    }

    //---------------------
    // Hint & Haptic Pulse
    //---------------------

    private void ShowHint(Hand hand, EVRButtonId button, string hintText)
    {
        CancelHint(hand, button);

        hintCoroutine = StartCoroutine(HintCoroutine(hand, button, hintText));
    }

    private void CancelHint(Hand hand, EVRButtonId button)
    {
        if (hintCoroutine != null)
        {
            ControllerButtonHints.HideTextHint(hand, button);

            StopCoroutine(hintCoroutine);
            hintCoroutine = null;
        }

        CancelInvoke("ShowHint");
    }

    private IEnumerator HintCoroutine(Hand hand, EVRButtonId button, string hintText)
    {
        float prevBreakTime = Time.time;
        float prevHapticPulseTime = Time.time;

        while (true)
        {
            bool pulsed = false;

            bool isShowingHint = !string.IsNullOrEmpty(ControllerButtonHints.GetActiveHintText(hand, button));

            if (!isShowingHint)
            {
                ControllerButtonHints.ShowTextHint(hand, button, hintText, true);
                prevBreakTime = Time.time;
                prevHapticPulseTime = Time.time;
            }

            if (Time.time > prevHapticPulseTime + 0.01f)
            {
                //Haptic pulse for a few seconds
                pulsed = true;

                hand.controller.TriggerHapticPulse(500);
            }
            else if (isShowingHint)
            {
                ControllerButtonHints.HideTextHint(hand, button);
            }
            //}

            if (Time.time > prevBreakTime + 3.0f)
            {
                //Take a break for a few seconds
                yield return new WaitForSeconds(3.0f);

                prevBreakTime = Time.time;
            }

            if (pulsed)
            {
                prevHapticPulseTime = Time.time;
            }

            yield return null;
        }
    }
}
