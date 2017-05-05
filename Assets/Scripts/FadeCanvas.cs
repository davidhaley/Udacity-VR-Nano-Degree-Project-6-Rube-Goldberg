using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvas : MonoBehaviour {

    [Space(1)]
    [Header("VISIBILITY")]
    [Space(5)]
    public bool uiVisibleOnStart;
    public bool fadeOnce;
    public float fadeSpeed = 1f;
    public float waitForSeconds;

    private CanvasGroup canvasGroup;
    private bool visible;
    private bool fadedOnce = false;

    private void Awake()
    {
        if (gameObject.GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup = GetComponentInChildren<CanvasGroup>();

        SetState();
    }

    private void SetState()
    {
        if (!uiVisibleOnStart)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            visible = false;
        }
        else if (uiVisibleOnStart)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            visible = true;
        }
    }

    public void ToggleFade()
    {
        if (fadeOnce && fadedOnce)
        {
            return;
        }

        StartCoroutine(StartFade());
    }

    IEnumerator StartFade()
    {
        if (waitForSeconds > 0)
        {
            yield return new WaitForSeconds(waitForSeconds);
        }

        if (!visible)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / fadeSpeed;
                canvasGroup.blocksRaycasts = true;
                yield return null;
            }

            fadedOnce = true;
            visible = true;
        }
        else if (visible)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / fadeSpeed;
                canvasGroup.blocksRaycasts = false;
                yield return null;
            }

            fadedOnce = true;
            gameObject.SetActive(false);
            visible = false;
        }
    }
}
