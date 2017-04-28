using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class LevelLoader : MonoBehaviour {

    private List<int> levels;
    private MusicController musicController;

    private int currentLevel;
    private int nextLevel;
    private int lastLevel;

    private void OnEnable()
    {
        Ball.ballTouchedGoal += OnBallTouchedGoal;
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        musicController = FindObjectOfType<MusicController>();

        levels = new List<int>();

        for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
        {
            levels.Add(i);
        }

        currentLevel = Convert.ToInt32(EditorSceneManager.GetActiveScene().name);
        nextLevel = currentLevel + 1;
        lastLevel = levels.Count - 1;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadLevel(nextLevel);
        }
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode loadSceneMode)
    {
        Player.instance.audioListener.gameObject.SetActive(true);

        if (scene.name == currentLevel.ToString())
        {
            ChangeMusic(currentLevel);
        }
    }

    private void OnBallTouchedGoal()
    {
        if (CollectablesManager.CollectablesRemaining() == 0)
        {
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

    public void LoadLevel(int levelNum)
    {
        SteamVR_LoadLevel level = Camera.main.GetComponent<SteamVR_LoadLevel>();

        level.levelName = levelNum.ToString();
        level.fadeOutTime = 1f;
        level.fadeInTime = 1f;
        level.Trigger();
        Player.instance.audioListener.gameObject.SetActive(false);
    }

    private void ChangeMusic(int levelNum)
    {
        musicController.Change(levelNum);
    }
}