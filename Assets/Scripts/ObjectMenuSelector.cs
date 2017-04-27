using MusicLab.InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

// Purpose: Script lives on opposite hand of the object menu hand and allows player to grab objects from the menu

public class ObjectMenuSelector : MonoBehaviour {

    public List<GameObject> menuObjectPrefabs;

    private static Hand selectorHand;
    private GameObject menuObjectHovering;
    private GameObject prefab;

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
                // Ensure left hand can interact with interactables
                // If the player switches hands, the left hand's interactable mask
                // May be reset from the ObjectMenu script
                selectorHand.hoverLayerMask = 2048;
            }

            if (menuObjectHovering != null)
            {
                if (selectorHand.controller.GetHairTriggerDown())
                {
                    if (menuObjectHovering.name == "MenuMetalPlank")
                    {
                        prefab = GameObject.Instantiate(menuObjectPrefabs[0]);
                    }
                    else if (menuObjectHovering.name == "MenuFan")
                    {
                        prefab = GameObject.Instantiate(menuObjectPrefabs[1]);
                    }
                    else if (menuObjectHovering.name == "MenuWoodPlank")
                    {
                        prefab = GameObject.Instantiate(menuObjectPrefabs[2]);
                    }
                    else if (menuObjectHovering.name == "MenuTrampoline")
                    {
                        prefab = GameObject.Instantiate(menuObjectPrefabs[3]);
                    }
                    else
                    {
                        prefab = null;
                    }

                    if (prefab != null)
                    {
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
}
