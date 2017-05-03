using MusicLab.InteractionSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System.Collections.Specialized;

public class Introduction : MonoBehaviour {

    public Text instructions;

    private OrderedDictionary hintDict;

    private FadeCanvas fadeCanvas;

    private Valve.VR.EVRButtonId touchpad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private int hintCounter = 0;
    private bool finishedHints = false;

    private Coroutine hintCoroutine;
    private Coroutine controllerHintCoroutine;
    private Coroutine instructionHintCoroutine;

    private bool cycleMenuHintActive = false;
    private string actionColor = "<color=olive>";
    private string buttonColor = "<color=orange>";
    private string outcomeColor = "<color=blue>";


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
        hintDict = new OrderedDictionary();

        AddCoroutinesToList();


        fadeCanvas = GetComponent<FadeCanvas>();

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
        if (controllerHintCoroutine == null && instructionHintCoroutine == null)
        {
            hintCoroutine = null;
        }

        Debug.Log("Hint coroutine is null: " + hintCoroutine == null);
        Debug.Log("Instruction coroutine is null: " + instructionHintCoroutine == null);
        Debug.Log("Controller coroutine is null: " + controllerHintCoroutine == null);

        if (hintCoroutine == null)
        {
            if (hintCounter <= hintDict.Keys.Count)
            {
                Debug.Log("incrementing counter at: " + Time.time);
                hintCounter += 1;
                StartCoroutine(BeginHintSequence(hintCounter));
            }
        }
    }

    private IEnumerator BeginHintSequence(int hintCounter)
    {
        if (hintDict.Contains(hintCounter))
        {
            Debug.Log("starting coroutine");
            hintCoroutine = StartCoroutine(hintDict[hintCounter] as IEnumerator);
        }

        yield return null;
    }

    private IEnumerator Hint1()
    {
        instructionHintCoroutine = StartCoroutine(ChangeInstructionCoroutine(hintCounter));
        controllerHintCoroutine = StartCoroutine(ShowObjectMenuHint());

        yield return null;
    }

    private IEnumerator Hint2()
    {
        instructionHintCoroutine = StartCoroutine(ChangeInstructionCoroutine(hintCounter));
        controllerHintCoroutine = StartCoroutine(CycleObjectMenuHint());

        yield return null;
    }

    private IEnumerator Hint3()
    {
        instructionHintCoroutine = StartCoroutine(ChangeInstructionCoroutine(hintCounter));
        controllerHintCoroutine = StartCoroutine(GrabObjectHint());

        yield return null;
    }

    private IEnumerator Hint4()
    {
        instructionHintCoroutine = StartCoroutine(ChangeInstructionCoroutine(hintCounter));
        controllerHintCoroutine = StartCoroutine(TeleportHint());

        yield return null;
    }

    private IEnumerator Hint5()
    {
        instructionHintCoroutine = StartCoroutine(ChangeInstructionCoroutine(hintCounter));

        yield return null;
    }

    private IEnumerator Hint6()
    {
        instructionHintCoroutine = StartCoroutine(ChangeInstructionCoroutine(hintCounter));

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
        yield return new WaitForSeconds(7f);

        Teleport.instance.ShowTeleportHint();
        yield return null;

    }

    private IEnumerator ChangeInstructionCoroutine(int hintCounter)
    {
        // Wait some time before changing the instructions after the player ends the controller hint
        if (hintCounter > 0)
        {
            yield return new WaitForSeconds(7f);
        }

        CancelInstructionHint(hintCounter);
        instructions.text = GetPlayerInstructions(hintCounter);
    }

    public void CancelInstructionHint(int hintCounter)
    {
        if (hintCounter > 0 && instructionHintCoroutine != null)
        {
            StopCoroutine(instructionHintCoroutine);
            instructionHintCoroutine = null;
        }

        CancelInvoke("ChangeInstructionCoroutine");
    }

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

    private void FadeCanvas()
    {
        fadeCanvas.waitForSeconds = 7f;
        fadeCanvas.ToggleFade();
    }

    private string GetPlayerInstructions(int hintCounter)
    {
        if (hintCounter == 0)
        {
            return "Gently " + actionColor + "TOUCH</color> the " + buttonColor + "[RIGHT TOUCHPAD]</color> to show your " + outcomeColor + " OBJECT MENU</color>.";
        }
        else if (hintCounter == 1)
        {
            return outcomeColor + "CYCLE</color> through your object menu, by " + actionColor + "PRESSING LEFT or RIGHT</color> on the " + buttonColor + "[RIGHT TOUCHPAD]</color>";
        }
        else if (hintCounter == 2)
        {
            return outcomeColor + "CREATE</color> an object from your menu, hover over the object with your left hand, and " + actionColor + "PRESS</color> the " + buttonColor + "[LEFT TRIGGER]</color>." + "\n\n<size=120><color=yellow>Caution: You only have a limited number of objects available!</color></size>";
        }
        else if (hintCounter == 3)
        {
            return "You can move around your playspace by teleporting. " + actionColor + "HOLD</color> the " + buttonColor + "[LEFT TOUCHPAD]</color> to aim, and " + actionColor + "RELEASE</color> the " + buttonColor + "[LEFT TOUCHPAD]</color> to teleport!";
        }
        else if (hintCounter == 4)
        {
            return "<color=red>OBJECTIVE:</color>\n\n Build a contraption using your <color=teal>OBJECTS</color>. Guide the <color=silver>BALL</color> toward the <color=red>GOAL</color>. Collect every <color=yellow>STAR</color> to advance to the next level!";
        }
        else if (hintCounter == 5)
        {
            return "<size=200><color=lightblue>LET'S GET STARTED!</color></size>";
        }
        else
        {
            return null;
        }
    }

    private void ShowHint(Hand hand, EVRButtonId button, string hintText)
    {
        CancelHint(hand, button);

        controllerHintCoroutine = StartCoroutine(HintCoroutine(hand, button, hintText));
    }

    private void CancelHint(Hand hand, EVRButtonId button)
    {
        if (controllerHintCoroutine != null)
        {
            ControllerButtonHints.HideTextHint(hand, button);

            Debug.Log("cancelling controller hint");

            StopCoroutine(controllerHintCoroutine);
            controllerHintCoroutine = null;
        }

        CancelInvoke("ShowHint");
    }

    private void AddCoroutinesToList()
    {
        hintDict.Add(0, Hint1());
        hintDict.Add(1, Hint2());
        hintDict.Add(2, Hint3());
        hintDict.Add(3, Hint4());
        hintDict.Add(4, Hint5());
        hintDict.Add(5, Hint6());
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
