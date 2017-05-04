using MusicLab.InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

// Purpose: Script lives on opposite hand of the object menu hand and allows player to grab objects from the menu

public class ObjectMenuSelector : MonoBehaviour {

    public delegate void SpectatorCameraInstantiated();

    public static event SpectatorCameraInstantiated OnSpectatorCameraInstantiated;

    public List<GameObject> menuObjectPrefabs;

    public Text metalPlankLimitText;
    public Text woodPlankLimitText;
    public Text fanBodyLimitText;
    public Text trampolineLimitText;
    public Text spectatorCameraLimitText;

    public int metalPlankMax;
    public int woodPlankMax;
    public int fanMax;
    public int trampolineMax;
    public int spectatorCameraMax;

    private GameObject menuObjectHovering;
    private GameObject prefab;
    private Hand leftHand;

    private int currentMetalPlank = 0;
    private int currentWoodPlank = 0;
    private int currentFan = 0;
    private int currentTrampoline = 0;
    private int currentSpectatorCamera = 0;

    private PlaySound menuSelectSound;
    private PlaySound menuZeroObjectsAvailSound;

    private void Awake()
    {
        InitializeLimitTexts();
        LoadAudio();
    }

    private void OnEnable()
    {
        SteamVRControllerEvents.OnGripDown += OnGripDown;
    }

    private void OnDisable()
    {
        SteamVRControllerEvents.OnGripDown -= OnGripDown;
    }

    private void Update()
    {
        leftHand = Player.instance.hands[(int)Hand.HandType.Left];

        if (leftHand != null)
        {
            if (transform.parent != leftHand.transform)
            {
                transform.SetParent(leftHand.transform);
            }

            if (leftHand.hoverLayerMask != 2048)
            {
                // Interactable layer mask
                leftHand.hoverLayerMask = 2048;
            }

            if (menuObjectHovering != null)
            {
                if (leftHand.controller.GetHairTriggerDown())
                {
                    prefab = null;

                    if (menuObjectHovering.name == "MenuMetalPlank")
                    {
                        if (currentMetalPlank + 1 <= metalPlankMax)
                        {
                            prefab = GameObject.Instantiate(menuObjectPrefabs[0]);
                            currentMetalPlank += 1;
                            SetLimitText(metalPlankLimitText, currentMetalPlank, metalPlankMax);
                        }
                    }
                    else if (menuObjectHovering.name == "MenuFan")
                    {
                        if (currentFan + 1 <= fanMax)
                        {
                            prefab = GameObject.Instantiate(menuObjectPrefabs[1]);
                            currentFan += 1;
                            SetLimitText(fanBodyLimitText, currentFan, fanMax);
                        }
                    }
                    else if (menuObjectHovering.name == "MenuWoodPlank")
                    {
                        if (currentWoodPlank + 1 <= woodPlankMax)
                        {
                            prefab = GameObject.Instantiate(menuObjectPrefabs[2]);
                            currentWoodPlank += 1;
                            SetLimitText(woodPlankLimitText, currentWoodPlank, woodPlankMax);
                        }
                    }
                    else if (menuObjectHovering.name == "MenuTrampoline")
                    {
                        if (currentTrampoline + 1 <= trampolineMax)
                        {
                            prefab = GameObject.Instantiate(menuObjectPrefabs[3]);
                            currentTrampoline += 1;
                            SetLimitText(trampolineLimitText, currentTrampoline, trampolineMax);
                        }
                    }
                    else if (menuObjectHovering.name == "MenuSpectatorCamera")
                    {
                        if (currentSpectatorCamera + 1 <= spectatorCameraMax)
                        {
                            prefab = GameObject.Instantiate(menuObjectPrefabs[4]);
                            currentSpectatorCamera += 1;
                            SetLimitText(spectatorCameraLimitText, currentSpectatorCamera, spectatorCameraMax);

                            ControllerButtonHints.ShowTextHint(leftHand, Valve.VR.EVRButtonId.k_EButton_Grip, "View Camera Feed", true);

                            if (OnSpectatorCameraInstantiated != null)
                            {
                                OnSpectatorCameraInstantiated();
                            }
                        }
                    }
                    else
                    {
                        prefab = null;
                    }

                    if (prefab != null)
                    {
                        prefab.GetComponent<Rigidbody>().isKinematic = true;
                        PrefabParentLeftHand(prefab);
                        menuSelectSound.Play();
                    }
                    else
                    {
                        // Zero objects available!
                        menuZeroObjectsAvailSound.Play();
                    }
                }
            }

        }
    }

    private void OnGripDown(SteamVRControllerEvents.ControllerEventArgs e)
    {
        if (currentSpectatorCamera >= 1 && e.fixedHandOrientation == "Left")
        {
            ControllerButtonHints.HideTextHint(leftHand, Valve.VR.EVRButtonId.k_EButton_Grip);
        }
    }

    private void InitializeLimitTexts()
    {
        if (metalPlankLimitText != null)
        {
            SetLimitText(metalPlankLimitText, currentMetalPlank, metalPlankMax);
        }

        if (woodPlankLimitText != null)
        {
            SetLimitText(woodPlankLimitText, currentWoodPlank, woodPlankMax);
        }

        if (fanBodyLimitText != null)
        {
            SetLimitText(fanBodyLimitText, currentFan, fanMax);
        }

        if (trampolineLimitText != null)
        {
            SetLimitText(trampolineLimitText, currentTrampoline, trampolineMax);
        }

        if (spectatorCameraLimitText != null)
        {
            SetLimitText(spectatorCameraLimitText, currentSpectatorCamera, spectatorCameraMax);
        }
    }

    private void SetLimitText(Text limitText, int current, int max)
    {
        limitText.text = (max - current).ToString() + " Available";
    }

    private void LoadAudio()
    {
        menuSelectSound = SoundManager.LoadAudio(gameObject, new List<string> { "Sounds/Effects/MenuSelect" }, 0.15f, false, false, false, "Effects");
        menuZeroObjectsAvailSound = SoundManager.LoadAudio(gameObject, new List<string> { "Sounds/Effects/ZeroObjectsAvailable" }, 0.25f, false, false, false, "Effects");
    }

    private void OnParentHandHoverBegin(Interactable hoveringInteractable)
    {
        if (hoveringInteractable.tag == "MenuObject")
        {
            menuObjectHovering = hoveringInteractable.gameObject;
        }
    }

    private void OnParentHandHoverEnd()
    {
        menuObjectHovering = null;
    }

    private void PrefabParentLeftHand(GameObject prefab)
    {
        prefab.transform.SetParent(leftHand.transform);
        prefab.transform.localPosition = Vector3.zero;
        prefab.transform.rotation = leftHand.transform.rotation;
    }
}
