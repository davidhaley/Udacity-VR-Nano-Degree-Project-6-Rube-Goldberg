using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

// Purpose: Manage score panel

public class ScoreManager : MonoBehaviour {

    public Text description;
    public Text scoreText;
    public Text win;
    public Text timeElapsed;

    public int incrementScoreBy = 1;
    private int score = 0;

    private DateTime startTime;
    private TimeSpan elapsedTime;
    private String displayTime;

    private List<Text> texts;

    private void OnEnable()
    {
        Ball.ballTouchedGround += OnBallTouchedGround;
        Ball.ballTouchedCollectable += OnBallTouchedCollectable;
        Ball.ballTouchedGoal += OnBallTouchedGoal;
    }

    private void OnDisable()
    {
        Ball.ballTouchedGround -= OnBallTouchedGround;
        Ball.ballTouchedCollectable -= OnBallTouchedCollectable;
        Ball.ballTouchedGoal -= OnBallTouchedGoal;
    }

    private void Awake()
    {
        AddTextsToList();
    }

    void Start()
    {
        InitializeScore();

        startTime = DateTime.Now;
    }

    private void LateUpdate()
    {
        elapsedTime = DateTime.Now - startTime;

        displayTime = String.Format("{0:00}:{1:00}", elapsedTime.Minutes, elapsedTime.Seconds);

        timeElapsed.text = displayTime;
    }

    public void IncreaseScore()
    {
        score += incrementScoreBy;
        SetScore();
    }

    private void InitializeScore()
    {
        ActivateTexts();
        win.gameObject.SetActive(false);

        scoreText.text = System.String.Empty;
        description.text = "Stars Collected:";

        score = 0;
        CollectablesManager.ResetCollectables();
        SetScore();
    }

    private void SetScore()
    {
        scoreText.text = (score + " of " + CollectablesManager.collectables.Length.ToString());
    }

    private void Win()
    {
        DeactivateTexts();
        win.gameObject.SetActive(true);
    }

    private void OnBallTouchedGround()
    {
        ResetScore();
    }

    private void OnBallTouchedCollectable()
    {
        IncreaseScore();
    }

    private void OnBallTouchedGoal()
    {
        if (CollectablesManager.CollectablesRemaining() == 0)
        {
            Win();
        }
    }

    private void ResetScore()
    {
        InitializeScore();
    }

    private void AddTextsToList()
    {
        texts = new List<Text>();

        texts.Add(description);
        texts.Add(scoreText);
    }

    private void DeactivateTexts()
    {
        foreach (Text text in texts)
        {
            if (text.gameObject.activeSelf)
            {
                text.gameObject.SetActive(false);
            }
        }
    }

    private void ActivateTexts()
    {
        foreach (Text text in texts)
        {
            if (text.gameObject.activeSelf == false)
            {
                text.gameObject.SetActive(true);
            }
        }
    }
}
