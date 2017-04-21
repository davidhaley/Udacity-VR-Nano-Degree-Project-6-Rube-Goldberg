using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectCollectable : MonoBehaviour
{
    public ScoreManager scoreManager;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Collectable"))
        {
            col.gameObject.SetActive(false);
            IncreaseScore();
        }
    }

    private void IncreaseScore()
    {
        scoreManager.IncreaseScore();
    }
}
