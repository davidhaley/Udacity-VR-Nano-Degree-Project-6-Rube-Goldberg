using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Purpose: Manage score panel

public class ScoreManager : MonoBehaviour {

    public Text description;
    public Text currentScore;
    public Text totalScore;
    public Text win;

    public int incrementScoreBy = 1;
    private int score = 0;

    private List<Text> texts;

    private void OnEnable()
    {
        Ball.ballTouchedGround += OnBallTouchedGround;
        Ball.ballTouchedCollectable += OnBallTouchedCollectable;
        Ball.ballTouchedGoal += OnBallTouchedGoal;
    }

    private void Awake()
    {
        AddTextsToList();
    }

    void Start()
    {
        InitializeScore();
    }

    //public int CurrentScore
    //{
    //    get { return score; }
    //}

    public void IncreaseScore()
    {
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
