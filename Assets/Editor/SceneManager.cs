//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SceneManager : MonoBehaviour {

//    private static int sceneCount;
//    private static string activeSceneName;

//    private void Awake()
//    {
//        DontDestroyOnLoad(this.gameObject);

//        sceneCount = UnityEditor.SceneManagement.EditorSceneManager.sceneCountInBuildSettings;
//    }

//    static int SceneCountInBuildSettings
//    {
//        get { return sceneCount; }
//    }

//    static string ActiveSceneName
//    {
//        get { return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; }
//    }
//}
