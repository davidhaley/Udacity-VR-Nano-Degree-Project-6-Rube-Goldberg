using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public Text description;
    public Text currentScore;
    public Text totalScore;
    public Text win;

    public int incrementScoreBy = 1;
    private int score = 0;

    private List<Text> texts;

    void Start()
    {
        InitializeScore();
        AddTextsToList();
    }

    public int CurrentScore
    {
        get { return score; }
    }

    public void IncreaseScore()
    {
        score += incrementScoreBy;
        SetScore();
    }

    private void InitializeScore()
    {
        currentScore.text = "";
        totalScore.text = "";
        description.text = "Stars Collected:";

        currentScore.text = score.ToString() + " of";
        totalScore.text = CollectablesManager.CollectablesRemaining().ToString();
    }

    private void SetScore()
    {
        currentScore.text = score.ToString() + " of";

        if (CollectablesManager.CollectablesRemaining() == 0)
        {
            Win();
        }
    }

    private void Win()
    {
        DeactivateTexts();
        win.gameObject.SetActive(true);
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
        Debug.Log("deactivating texts");

        foreach (Text text in texts)
        {
            text.gameObject.SetActive(false);
        }
    }
}
