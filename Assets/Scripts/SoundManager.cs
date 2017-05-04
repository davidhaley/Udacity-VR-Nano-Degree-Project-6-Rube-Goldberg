using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Valve.VR.InteractionSystem;

public class SoundManager : MonoBehaviour {

    private static AudioMixer audioMixer;

    private void Awake()
    {
        audioMixer = Resources.Load("MasterMixer") as AudioMixer;
    }

    public static PlaySound LoadAudio(GameObject parentGo, List<string> audioClipPaths, float volume, bool playOnAwake, bool useRandomVolume, bool useRandomPitch, string audioMixerGroup)
    {
        // Create child gameobject to hold sound(s)
        GameObject childGo = new GameObject();
        childGo.transform.SetParent(parentGo.transform);

        // Create audio clip(s)
        AudioClip[] clips = new AudioClip[audioClipPaths.Count];

        for (int i = 0; i < audioClipPaths.Count; i++)
        {
            AudioClip clip = Resources.Load<AudioClip>(audioClipPaths[i]);
            clips[i] = clip;
        }

        PlaySound playSound = childGo.GetComponent<PlaySound>();

        if (playSound == null)
        {
            playSound = childGo.AddComponent<PlaySound>();
        }

        playSound.useRandomVolume = useRandomVolume;
        playSound.useRandomPitch = useRandomPitch;
        playSound.waveFile = clips;

        childGo.name = playSound.waveFile[0].name + "Sound";

        AudioSource audioSource = childGo.GetComponent<AudioSource>();
        audioSource.playOnAwake = playOnAwake;
        audioSource.volume = volume;
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(audioMixerGroup)[0];

        // HRTF audio (Steam Audio)
        Phonon.PhononSource phonon = childGo.AddComponent<Phonon.PhononSource>();
        phonon.enableReflections = true;
        phonon.directBinauralEnabled = true;

        return playSound;
    }
}
