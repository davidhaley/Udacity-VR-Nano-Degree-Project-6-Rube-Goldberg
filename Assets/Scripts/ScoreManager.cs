using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

// Purpose: Manage score panel

public class ScoreManager : MonoBehaviour {

    public Text description;
    public Text currentScore;
    public Text totalScore;
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

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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
        Debug.Log("increasing score");
        score += incrementScoreBy;
        SetScore();
    }

    private void InitializeScore()
    {
        ActivateTexts();
        win.gameObject.SetActive(false);

        currentScore.text = "";
        totalScore.text = "";
        description.text = "Stars Collected:";

        score = 0;

        currentScore.text = score.ToString() + " of";
        CollectablesManager.ResetCollectables();
        totalScore.text = CollectablesManager.CollectablesRemaining().ToString();
    }

    private void SetScore()
    {
        Debug.Log("setting score");

        currentScore.text = score.ToString() + " of";
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
        texts.Add(currentScore);
        texts.Add(totalScore);
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
