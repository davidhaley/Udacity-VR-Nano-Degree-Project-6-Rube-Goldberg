using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesManager : MonoBehaviour {

    public static GameObject[] collectables;

    private void Start()
    {
        collectables = GameObject.FindGameObjectsWithTag("Collectable");
        Debug.Log("collectable length: " + collectables.Length);
    }

    private void ResetCollectables()
    {
        for (int i = 0; i < collectables.Length; i++)
        {
            collectables[i].gameObject.SetActive(true);
        }
    }

    public static int CollectablesRemaining()
    {
        if (collectables != null)
        {
            int active = 0;

            for (int i = 0; i < collectables.Length; i++)
            {
                if (collectables[i].gameObject.activeSelf)
                {
                    active += 1;
                }
            }

            return active;
        }
        else
        {
            return -1;
        }
    }

    //void Start()
    //{

    //    origins = new Vector3[collectables.Length];

    //    for (int i = 0; i < origins.Length; i++)
    //    {
    //        origins[i] = collectables[i].gameObject.transform.position;
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //private void ResetCollectables()
    //{
    //    for (int i = 0; i < collectables.Length; i++)
    //    {
    //        collectables[i].gameObject.transform.position = origins[i];
    //    }
    //}
}
