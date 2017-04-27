using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    private List<int> levels;

    private int currentLevel;
    private int nextLevel;
    private int lastLevel;

    private void OnEnable()
    {
        Ball.ballTouchedGoal += OnBallTouchedGoal;
    }

    private void Awake()
    {
        levels = new List<int>();

        for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
        {
            levels.Add(i);
        }

        currentLevel = 1;
        lastLevel = levels.Count - 1;

        Debug.Log(levels);
        Debug.Log(levels.Count);
        Debug.Log(currentLevel);
        Debug.Log(nextLevel);
        Debug.Log(lastLevel);
    }

    private void OnBallTouchedGoal()
    {
        if (CollectablesManager.CollectablesRemaining() == 0)
        {
            nextLevel = currentLevel + 1;

            if (currentLevel != lastLevel)
            {
                LoadLevel(nextLevel);
            }
            else
            {
                // Show credits
            }
        }
    }

    public static void LoadLevel(int levelNum)
    {
        SteamVR_LoadLevel level = Camera.main.GetComponent<SteamVR_LoadLevel>();
        level.levelName = levelNum.ToString();
        level.fadeOutTime = 1f;
        level.fadeInTime = 1f;
        level.Trigger();
    }
}
