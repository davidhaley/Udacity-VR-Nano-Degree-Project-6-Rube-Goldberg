using MusicLab.InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

// Purpose: Script lives on opposite hand of the object menu hand and allows player to grab objects from the menu

public class ObjectMenuSelector : MonoBehaviour {

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

    private void Awake()
    {
        InitializeLimitTexts();
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
                        else
                        {
                            MaxPrefabsInstantiated(metalPlankLimitText);
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
                        else
                        {
                            MaxPrefabsInstantiated(fanBodyLimitText);
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
                        else
                        {
                            MaxPrefabsInstantiated(woodPlankLimitText);
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
                        else
                        {
                            MaxPrefabsInstantiated(trampolineLimitText);
                        }
                    }
                    else if (menuObjectHovering.name == "MenuSpectatorCamera")
                    {
                        if (currentSpectatorCamera + 1 <= spectatorCameraMax)
                        {
                            prefab = GameObject.Instantiate(menuObjectPrefabs[4]);
                            currentSpectatorCamera += 1;
                            SetLimitText(spectatorCameraLimitText, currentSpectatorCamera, spectatorCameraMax);
                        }
                        else
                        {
                            MaxPrefabsInstantiated(spectatorCameraLimitText);
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
                    }
                }
            }

        }
    }

    private void InitializeLimitTexts()
    {
        SetLimitText(metalPlankLimitText, currentMetalPlank, metalPlankMax);
        SetLimitText(woodPlankLimitText, currentWoodPlank, woodPlankMax);
        SetLimitText(fanBodyLimitText, currentFan, fanMax);
        SetLimitText(trampolineLimitText, currentTrampoline, trampolineMax);
    }

    private void SetLimitText(Text limitText, int current, int max)
    {
        limitText.text = (max - current).ToString() + " Available";
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

    private void MaxPrefabsInstantiated(Text text)
    {
        Debug.Log("max prefabs instantiated");
    }
}
