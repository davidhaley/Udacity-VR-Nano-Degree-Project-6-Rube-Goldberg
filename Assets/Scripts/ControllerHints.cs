﻿
//Purpose: Interface to Valve's ControllerButtonHints script

using UnityEngine;
using System.Collections;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace MusicLab.InteractionSystem
{
    //-------------------------------------------------------------------------
    public class ControllerHints : MonoBehaviour
    {
        private Coroutine buttonHintCoroutine;
        private Coroutine textHintCoroutine;

        //-------------------------------------------------
        public void ShowButtonHints(Hand hand)
        {
            if (buttonHintCoroutine != null)
            {
                StopCoroutine(buttonHintCoroutine);
            }
            buttonHintCoroutine = StartCoroutine(TestButtonHints(hand));
        }


        //-------------------------------------------------
        public void ShowTextHints(Hand hand)
        {
            if (textHintCoroutine != null)
            {
                StopCoroutine(textHintCoroutine);
            }
            textHintCoroutine = StartCoroutine(TestTextHints(hand));
        }


        //-------------------------------------------------
        public void DisableHints()
        {
            if (buttonHintCoroutine != null)
            {
                StopCoroutine(buttonHintCoroutine);
                buttonHintCoroutine = null;
            }

            if (textHintCoroutine != null)
            {
                StopCoroutine(textHintCoroutine);
                textHintCoroutine = null;
            }

            foreach (Hand hand in Player.instance.hands)
            {
                ControllerButtonHints.HideAllButtonHints(hand);
                ControllerButtonHints.HideAllTextHints(hand);
            }
        }


        //-------------------------------------------------
        // Cycles through all the button hints on the controller
        //-------------------------------------------------
        private IEnumerator TestButtonHints(Hand hand)
        {
            ControllerButtonHints.HideAllTextHints(hand);

            while (true)
            {
                ControllerButtonHints.ShowButtonHint(hand, EVRButtonId.k_EButton_ApplicationMenu);
                yield return new WaitForSeconds(1.0f);
                ControllerButtonHints.ShowButtonHint(hand, EVRButtonId.k_EButton_System);
                yield return new WaitForSeconds(1.0f);
                ControllerButtonHints.ShowButtonHint(hand, EVRButtonId.k_EButton_Grip);
                yield return new WaitForSeconds(1.0f);
                ControllerButtonHints.ShowButtonHint(hand, EVRButtonId.k_EButton_SteamVR_Trigger);
                yield return new WaitForSeconds(1.0f);
                ControllerButtonHints.ShowButtonHint(hand, EVRButtonId.k_EButton_SteamVR_Touchpad);
                yield return new WaitForSeconds(1.0f);

                ControllerButtonHints.HideAllButtonHints(hand);
                yield return new WaitForSeconds(1.0f);
            }
        }


        //-------------------------------------------------
        // Cycles through all the text hints on the controller
        //-------------------------------------------------
        private IEnumerator TestTextHints(Hand hand)
        {
            ControllerButtonHints.HideAllTextHints(hand);

            while (true)
            {
                ControllerButtonHints.ShowTextHint(hand, EVRButtonId.k_EButton_ApplicationMenu, "Application");
                yield return new WaitForSeconds(3.0f);
                ControllerButtonHints.ShowTextHint(hand, EVRButtonId.k_EButton_System, "System");
                yield return new WaitForSeconds(3.0f);
                ControllerButtonHints.ShowTextHint(hand, EVRButtonId.k_EButton_Grip, "Grip");
                yield return new WaitForSeconds(3.0f);
                ControllerButtonHints.ShowTextHint(hand, EVRButtonId.k_EButton_SteamVR_Trigger, "Trigger");
                yield return new WaitForSeconds(3.0f);
                ControllerButtonHints.ShowTextHint(hand, EVRButtonId.k_EButton_SteamVR_Touchpad, "Touchpad");
                yield return new WaitForSeconds(3.0f);

                ControllerButtonHints.HideAllTextHints(hand);
                yield return new WaitForSeconds(3.0f);
            }
        }
    }
}

