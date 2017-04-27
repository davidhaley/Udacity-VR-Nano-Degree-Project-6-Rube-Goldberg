using MusicLab.InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

// Purpose: Script lives on opposite hand of the object menu hand and allows player to grab objects from the menu

public class ObjectMenuSelector : MonoBehaviour {

    public List<GameObject> menuObjectPrefabs;

    private Hand leftHand;
    private GameObject menuObjectHovering;
    private GameObject prefab;

    private void Update()
    {
        leftHand = Player.instance.leftHand;

        if (leftHand != null)
        {
            if (leftHand.hoverLayerMask != 2048)
            {
                // Ensure left hand can interact with interactables
                // If the player switches hands, the left hand's interactable mask
                // May be reset from the ObjectMenu script
                leftHand.hoverLayerMask = 2048;
            }

            SelectorParentLeftHand();

            if (menuObjectHovering != null)
            {
                if (leftHand.controller.GetHairTriggerDown())
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

    private void SelectorParentLeftHand()
    {
        if (gameObject.transform.parent != leftHand.transform)
        {
            gameObject.transform.SetParent(leftHand.transform);
        }
    }

    private void PrefabParentLeftHand(GameObject prefab)
    {
        prefab.transform.SetParent(leftHand.transform);
        prefab.transform.localPosition = Vector3.zero;
        prefab.transform.rotation = leftHand.transform.rotation;
    }
}
