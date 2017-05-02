using MusicLab.InteractionSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Introduction : MonoBehaviour {

    public Text instructions;

    private Dictionary<int, string> instructionDict;
    private Dictionary<int, IEnumerator> coroutineDict;

    private string actionColor = "<color=olive>";
    private string buttonColor = "<color=orange>";
    private string outcomeColor = "<color=blue>";

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
        CreatePlayerInstructions();
        CreateCoroutines();

        if (instructions.gameObject.activeSelf == false)
        {
            instructions.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        StartCoroutine(BeginHintSequence(hintCounter));
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
                StartCoroutine(BeginHintSequence(hintCounter));
            }
        }
    }

    private void CreatePlayerInstructions()
    {
        instructionDict = new Dictionary<int, string>();

        string instruction1 = "Gently " + actionColor + "TOUCH</color> the " + buttonColor + "[RIGHT TOUCHPAD]</color> to show your " + outcomeColor + " OBJECT MENU</color>.";
        string instruction2 = outcomeColor + "CYCLE</color> through your object menu, by " + actionColor + "PRESSING LEFT or RIGHT</color> on the " + buttonColor + "[RIGHT TOUCHPAD]</color>";
        string instruction3 = "To " + outcomeColor + "CREATE</color> an object from your menu, hover over the object with your left hand, and " + actionColor + "PRESS</color> the " + buttonColor + "[LEFT TRIGGER]</color>." + "\n\n<size=120><color=yellow>Caution: You only have a limited number of objects available!</color></size>";
        string instruction4 = "You can move around your playspace by teleporting. " + actionColor + "HOLD</color> the " + buttonColor + "[LEFT TOUCHPAD]</color> to aim, and " + actionColor + "RELEASE</color> the " + buttonColor + "[LEFT TOUCHPAD]</color> to teleport!";
        string instruction5 = "Build a contraption using your <color=teal>OBJECTS</color>. Guide the <color=silver>BALL</color> toward the <color=red>GOAL</color>. Collect every <color=yellow>STAR</color> to advance to the next level!";
        string instruction6 = "<size=200><color=lightblue>LET'S GET STARTED!</color></size>";

        instructionDict.Add(0, instruction1);
        instructionDict.Add(1, instruction2);
        instructionDict.Add(2, instruction3);
        instructionDict.Add(3, instruction4);
        instructionDict.Add(4, instruction5);
        instructionDict.Add(5, instruction6);
    }

    private void CreateCoroutines()
    {
        coroutineDict = new Dictionary<int, IEnumerator>();

        coroutineDict.Add(0, ShowObjectMenuHint());
        coroutineDict.Add(1, CycleObjectMenuHint());
        coroutineDict.Add(2, GrabObjectHint());
        coroutineDict.Add(3, TeleportHint());
    }

    private void ChangeText(Text instructions, int hintCounter)
    {
        if (instructionDict.ContainsKey(hintCounter))
        {
            instructions.text = instructionDict[hintCounter];
        }
    }

    private void ChangeCoroutine(int hintCounter)
    {
        if (coroutineDict.ContainsKey(hintCounter))
        {
            hintCoroutine = StartCoroutine(coroutineDict[hintCounter]);
        }
    }

    private IEnumerator BeginHintSequence(int hintCounter)
    {
        ChangeCoroutine(hintCounter);
        ChangeText(instructions, hintCounter);
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
