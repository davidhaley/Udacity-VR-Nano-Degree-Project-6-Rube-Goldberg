using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Valve.VR.InteractionSystem;

public class SoundManager : MonoBehaviour {

    private static AudioMixer audioMixer;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        audioMixer = Resources.Load("MasterMixer") as AudioMixer;
    }

    public static PlaySound LoadAudio(GameObject parentGo, List<string> audioClipPaths, float volume, bool playOnAwake, bool useRandomVolume, bool useRandomPitch, string audioMixerGroup, float maxPitch = 1f, float minPitch = 1f)
    {
        if (audioMixer == null)
        {
            audioMixer = Resources.Load("MasterMixer") as AudioMixer;
        }

        // Create a child gameobject for each audio source
        GameObject childGo = new GameObject();
        childGo.transform.SetParent(parentGo.transform, false);


        // Pull clips by path
        AudioClip[] clips = new AudioClip[audioClipPaths.Count];

        for (int i = 0; i < audioClipPaths.Count; i++)
        {
            AudioClip clip = Resources.Load<AudioClip>(audioClipPaths[i]);
            clips[i] = clip;
        }

        AudioSource audioSource = childGo.AddComponent<AudioSource>();
        audioSource.playOnAwake = playOnAwake;
        audioSource.volume = volume;
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(audioMixerGroup)[0];

        // Valve's feature rich sound player
        PlaySound playSound = childGo.GetComponent<PlaySound>();

        if (playSound == null)
        {
            playSound = childGo.AddComponent<PlaySound>();
        }

        playSound.useRandomVolume = useRandomVolume;
        playSound.useRandomPitch = useRandomPitch;
        playSound.waveFile = clips;

        if (useRandomPitch)
        {
            playSound.pitchMin = minPitch;
            playSound.pitchMax = maxPitch;
        }

        // Name the game object according to the sound's file name
        childGo.name = playSound.waveFile[0].name + "Sound";

        // Add HRTF audio (Steam Audio)
        Phonon.PhononSource phonon = childGo.AddComponent<Phonon.PhononSource>();
        phonon.enableReflections = true;
        phonon.directBinauralEnabled = true;

        return playSound;
    }

    public static void StopAllAudio(List<PlaySound> playSounds)
    {
        foreach (PlaySound playSound in playSounds)
        {
            playSound.Stop();
        }
    }

    public static bool AudioSourcePlaying(PlaySound playSound)
    {
        return playSound.GetComponent<AudioSource>().isPlaying;
    }

    public static void SetAudioSourcePitch(PlaySound playSound, float pitch)
    {
        playSound.GetComponent<AudioSource>().pitch = pitch;
    }
}
