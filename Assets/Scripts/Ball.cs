using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

// Purpose: Announce ball collisions, play sounds, and reset ball to start position when it touches the ground

public class Ball : MonoBehaviour {

    public delegate void BallTouchedGround();
    public delegate void BallTouchedGoal();
    public delegate void BallTouchedCollectable();

    public static event BallTouchedGround ballTouchedGround;
    public static event BallTouchedGround ballTouchedGoal;
    public static event BallTouchedGround ballTouchedCollectable;

    public GameObject collectableAudio;
    public GameObject trampolineAudio;
    public GameObject metalPlankAudio;
    public GameObject woodPlankAudio;

    private Vector3 resetPosition;
    private Vector3 resetVelocity;

    private AudioSource collectableAudioSource;
    private PlaySound trampolineAudioSource;
    private PlaySound metalPlankAudioSource;

    private void Awake()
    {
        InitializeBall();
        LoadPhononEffect();
        LoadCollectableAudio();
        LoadTrampolineAudio();
        LoadMetalPlankAudio();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ResetBall();

            if (ballTouchedGround != null)
            {
                ballTouchedGround();
            }

        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            if (ballTouchedGoal != null)
            {
                ballTouchedGoal();
            }
        }
        else if (collision.gameObject.CompareTag("Trampoline"))
        {
            trampolineAudioSource.Play();
        }
        else if (collision.gameObject.CompareTag("MetalPlank"))
        {
            metalPlankAudioSource.PlayLooping();
        }
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("MetalPlank"))
    //    {
    //        metalPlankAudioSource.PlayLooping();
    //    }
    //}

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MetalPlank"))
        {
            metalPlankAudioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Collectable"))
        {
            collectableAudioSource.Play();

            col.gameObject.SetActive(false);

            if (ballTouchedCollectable != null)
            {
                ballTouchedCollectable();
            }
        }
    }

    private void InitializeBall()
    {
        resetPosition = transform.position;
        resetVelocity = gameObject.GetComponent<Rigidbody>().velocity;
    }

    private void ResetBall()
    {
        gameObject.transform.position = resetPosition;
        gameObject.transform.GetComponent<Rigidbody>().velocity = resetVelocity;
    }

    private void LoadCollectableAudio()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/Effects/Collectable");

        collectableAudioSource = collectableAudio.AddComponent<AudioSource>();
        collectableAudioSource.playOnAwake = false;
        collectableAudioSource.clip = clip;
    }

    private void LoadTrampolineAudio()
    {
        AudioClip clip1 = Resources.Load<AudioClip>("Sounds/Effects/Trampoline1");
        AudioClip clip2 = Resources.Load<AudioClip>("Sounds/Effects/Trampoline2");
        AudioClip clip3 = Resources.Load<AudioClip>("Sounds/Effects/Trampoline3");

        AudioClip[] clips = new AudioClip[3];
        clips[0] = clip1;
        clips[1] = clip2;
        clips[2] = clip3;

        trampolineAudioSource = trampolineAudio.AddComponent<PlaySound>();

        trampolineAudioSource.waveFile = clips;
    }

    private void LoadMetalPlankAudio()
    {
        AudioClip clip1 = Resources.Load<AudioClip>("Sounds/Effects/RollingBallMetal");

        AudioClip[] clips = new AudioClip[1];
        clips[0] = clip1;

        metalPlankAudioSource = metalPlankAudio.AddComponent<PlaySound>();
        metalPlankAudioSource.useRandomVolume = false;
        metalPlankAudioSource.stopOnPlay = true;
        metalPlankAudioSource.looping = true;
        metalPlankAudioSource.waveFile = clips;

        metalPlankAudio.GetComponent<AudioSource>().playOnAwake = false;
    }

    private void LoadPhononEffect()
    {
        Phonon.PhononSource phonon = gameObject.AddComponent<Phonon.PhononSource>();
        phonon.enableReflections = true;
        phonon.directBinauralEnabled = true;
    }
}