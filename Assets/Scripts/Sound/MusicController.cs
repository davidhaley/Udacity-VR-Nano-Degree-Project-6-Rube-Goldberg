using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MusicController : MonoBehaviour {

    public AudioSource audioSource;

    public static AudioClip level1;
    public static AudioClip level2;
    public static AudioClip level3;
    public static AudioClip level4;

    private void Awake()
    {
        level1 = Resources.Load<AudioClip>("Sounds/Music/Zusammenallien");
        level2 = Resources.Load<AudioClip>("Sounds/Music/Roll Your Own");
        level3 = Resources.Load<AudioClip>("Sounds/Music/Cosmopolitan Drink");
        level4 = Resources.Load<AudioClip>("Sounds/Music/Head Down (Jay P Mix)");

        if (audioSource.clip == null && EditorSceneManager.GetActiveScene().name == "1")
        {
            Change(1);
        }
    }

    public void Change(int level)
    {
        if (level == 1)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = level1;
            audioSource.Play();
        }
        else if (level == 2)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = level2;
            audioSource.Play();
        }
        else if (level == 3)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = level3;
            audioSource.Play();
        }
        else if (level == 4)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = level4;
            audioSource.Play();
        }
    }
}
