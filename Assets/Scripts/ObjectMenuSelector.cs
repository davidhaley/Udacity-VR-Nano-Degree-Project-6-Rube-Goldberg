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

    public int metalPlankMax;
    public int woodPlankMax;
    public int fanMax;
    public int trampolineMax;

    private static Hand selectorHand;
    private GameObject menuObjectHovering;
    private GameObject prefab;

    private int currentMetalPlank = 0;
    private int currentWoodPlank = 0;
    private int currentFan = 0;
    private int currentTrampoline = 0;

    private List<Text> limitTexts;

    private void Awake()
    {
        InitializeLimitTexts();
    }

    private void Update()
    {
        if (selectorHand != null)
        {
            if (ObjectMenu.FirstTimeShown)
            {
                if (transform.parent != selectorHand.transform)
                {
                    transform.SetParent(selectorHand.transform);
                }
            }

            if (selectorHand.hoverLayerMask != 2048)
            {
                // Interactable layer mask
                selectorHand.hoverLayerMask = 2048;
            }

            if (menuObjectHovering != null)
            {
                if (selectorHand.controller.GetHairTriggerDown())
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
        else
        {
            UpdateSelectorHand();
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
        prefab.transform.SetParent(selectorHand.transform);
        prefab.transform.localPosition = Vector3.zero;
        prefab.transform.rotation = selectorHand.transform.rotation;
    }

    private void UpdateSelectorHand()
    {
        selectorHand = ObjectMenu.MenuHand.otherHand;
    }

    public static Hand SelectorHand
    {
        get { return selectorHand; }
    }

    private void MaxPrefabsInstantiated(Text text)
    {
        Debug.Log("max prefabs instantiated");
    }
}
