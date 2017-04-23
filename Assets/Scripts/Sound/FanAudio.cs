//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FanAudio : MonoBehaviour {

//    private AudioSource audioSource;

//    private AudioClip fanSwitchOn;
//    private AudioClip fanSwitchOff;
//    private AudioClip fanRampUp;
//    private AudioClip fanLow;
//    private AudioClip fanNormal;
//    private AudioClip fanHigh;

//    private void Awake()
//    {
//        audioSource = GetComponent<AudioSource>();
//    }

//    public void TurnFanOn(string speed)
//    {
//        if (audioSource.isPlaying)
//        {
//            audioSource.Stop();
//        }

//        audioSource.loop = false;

//        audioSource.clip = fanSwitchOn;
//        audioSource.Play();

//        audioSource.clip = fanRampUp;
//        audioSource.Play();


//        audioSource.clip = FanSpeedAudioClip(speed);
//        audioSource.loop = true;
//        audioSource.Play();
//    }

//    public void StopFan()
//    {
//        audioSource.Stop();
//        audioSource.clip = fanSwitchOff;
//        audioSource.loop = false;
//        audioSource.Play();
//    }

//    private AudioClip FanSpeedAudioClip(string speed)
//    {
//        AudioClip clip = null;

//        if (speed == "Low")
//        {
//            clip = fanLow;
//        }
//        else if (speed == "Normal")
//        {
//            clip = fanNormal;
//        }
//        else if (speed == "High")
//        {
//            clip = fanHigh;
//        }

//        return clip;
//    }

//    private void LoadAudioClips()
//    {
//        fanSwitchOn = Resources.Load<AudioClip>("Sounds/Fan/FanSwitchOn");
//        fanSwitchOff = Resources.Load<AudioClip>("Sounds/Fan/FanSwitchOff");
//        fanRampUp = Resources.Load<AudioClip>("Sounds/Fan/FanRampUp");
//        fanLow = Resources.Load<AudioClip>("Sounds/Fan/FanLow");
//        fanNormal = Resources.Load<AudioClip>("Sounds/Fan/FanNormal");
//        fanHigh = Resources.Load<AudioClip>("Sounds/Fan/FanHigh");
//    }
//}
