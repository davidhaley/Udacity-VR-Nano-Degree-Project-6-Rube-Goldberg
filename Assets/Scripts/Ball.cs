﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Valve.VR.InteractionSystem;

// Purpose: Handle collision logic for ball, play sounds, and reset ball to start position when it touches the ground

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

    private PlaySound collectablePlaySound;
    private PlaySound trampolinePlaySound;
    private PlaySound metalPlankPlaySound;

    private AudioSource collectableAudioSource;
    private AudioSource trampolineAudioSource;
    private AudioSource metalPlankAudioSource;

    private Rigidbody rigidBody;
    private Vector3 resetPosition;
    private Vector3 resetVelocity;
    private Vector3 resetAngularVelocity;
    private Vector3 velocity;

    private bool ballWithinPlatformBounds;
    private Renderer ballRenderer;
    private bool ballActive = true;
    private Material ballInactiveMaterial;
    private Material ballActiveMaterial;

    private bool structureAttachedToHand;

    AudioMixer audioMixer;
    private List<AudioSource> audioSources;

    private void OnEnable()
    {
        Structure.onAttachedToHand += OnAttachedToHand;
        Structure.onDetachedFromHand += OnDetachedFromHand;
    }

    private void Awake()
    {
        InitializeBall();

        audioMixer = Resources.Load("MasterMixer") as AudioMixer;
        audioSources = new List<AudioSource>();
    }

    private void Start()
    {
        LoadCollectableAudio();
        LoadTrampolineAudio();
        LoadMetalPlankAudio();
    }

    private void FixedUpdate()
    {
        velocity = rigidBody.velocity;
    }

    private void Update()
    {
        if (structureAttachedToHand && !ballWithinPlatformBounds)
        {
            DeactivateBall();
        }
    }

    public Vector3 GetVelocity
    {
        get { return velocity; }
    }

    // Ball attached to hand
    private void HandAttachedUpdate()
    {
        if (ballWithinPlatformBounds)
        {
            ActivateBall();
        }
        else if (!ballWithinPlatformBounds)
        {
            DeactivateBall();
        }
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
        else if (collision.gameObject.CompareTag("Goal") && ballActive)
        {
            if (ballTouchedGoal != null)
            {
                ballTouchedGoal();
            }
        }
        else if (collision.gameObject.CompareTag("Trampoline"))
        {
            trampolinePlaySound.Play();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("MetalPlank"))
        {
            if (!metalPlankAudioSource.isPlaying)
            {
                metalPlankPlaySound.Play();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MetalPlank"))
        {
            StartCoroutine(StopAfterDelay(metalPlankPlaySound, 0.7f));
        }
    }

    private IEnumerator StopAfterDelay(PlaySound playSound, float delay)
    {
        yield return new WaitForSeconds(delay);
        playSound.Stop();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Collectable") && ballActive)
        {
            collectablePlaySound.Play();

            col.gameObject.SetActive(false);

            if (ballTouchedCollectable != null)
            {
                ballTouchedCollectable();
            }
        }
        else if (col.gameObject.CompareTag("PlatformBounds"))
        {
            ballWithinPlatformBounds = true;
        }
        else if (col.gameObject.CompareTag("TeleportTarget"))
        {
            Teleporter teleporter = col.gameObject.GetComponent<Teleporter>();
            teleporter.Teleport(gameObject);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("PlatformBounds"))
        {
            ballWithinPlatformBounds = false;
        }
    }

    private void OnAttachedToHand()
    {
        structureAttachedToHand = true;
    }

    private void OnDetachedFromHand()
    {
        structureAttachedToHand = false;
    }

    private void InitializeBall()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();

        resetPosition = transform.position;
        resetVelocity = rigidBody.velocity;
        resetAngularVelocity = rigidBody.angularVelocity;

        ballActiveMaterial = Resources.Load<Material>("Materials/BallActive");
        ballInactiveMaterial = Resources.Load<Material>("Materials/BallInactive");
        ballRenderer = gameObject.GetComponent<Renderer>();
    }

    private void ResetBall()
    {
        gameObject.transform.position = resetPosition;
        rigidBody.velocity = resetVelocity;
        rigidBody.angularVelocity = resetAngularVelocity;

        StopAudioSources();

        // Keep ball deactivated if player holds onto structure as ball resets
        if (structureAttachedToHand)
        {
            DeactivateBall();
        }
        else
        {
            ActivateBall();
        }
    }

    private void StopAudioSources()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void DeactivateBall()
    {
        ballRenderer.material = ballInactiveMaterial;
        ballActive = false;
    }

    private void ActivateBall()
    {
        ballRenderer.material = ballActiveMaterial;
        ballActive = true;
    }

    private void StopAudioSource(AudioSource audioSource)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void LoadCollectableAudio()
    {
        AudioClip clip1 = Resources.Load<AudioClip>("Sounds/Effects/Collectable");
        AudioClip[] clips = new AudioClip[1];
        clips[0] = clip1;

        collectablePlaySound = collectableAudio.AddComponent<PlaySound>();
        collectablePlaySound.waveFile = clips;
        collectablePlaySound.useRandomVolume = false;

        collectableAudioSource = collectableAudio.GetComponent<AudioSource>();
        collectableAudioSource.playOnAwake = false;
        collectableAudioSource.volume = 0.30f;
        collectableAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effects")[0];
        audioSources.Add(collectableAudioSource);

        LoadPhononEffect(collectableAudio);
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

        trampolinePlaySound = trampolineAudio.AddComponent<PlaySound>();
        trampolinePlaySound.waveFile = clips;

        trampolineAudioSource = trampolineAudio.GetComponent<AudioSource>();
        trampolineAudioSource.playOnAwake = false;
        trampolineAudioSource.volume = 0.50f;
        trampolineAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effects")[0];
        audioSources.Add(trampolineAudioSource);

        LoadPhononEffect(trampolineAudio);
    }

    private void LoadMetalPlankAudio()
    {
        AudioClip clip1 = Resources.Load<AudioClip>("Sounds/Effects/RollingBallMetal");

        AudioClip[] clips = new AudioClip[1];
        clips[0] = clip1;

        metalPlankPlaySound = metalPlankAudio.AddComponent<PlaySound>();
        metalPlankPlaySound.useRandomVolume = false;
        metalPlankPlaySound.waveFile = clips;

        metalPlankAudioSource = metalPlankAudio.GetComponent<AudioSource>();
        metalPlankAudioSource.playOnAwake = false;
        metalPlankAudioSource.volume = 0.25f;
        metalPlankAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effects")[0];
        audioSources.Add(metalPlankAudioSource);

        LoadPhononEffect(metalPlankAudio);
    }

    private void LoadPhononEffect(GameObject go)
    {
        Phonon.PhononSource phonon = go.AddComponent<Phonon.PhononSource>();
        phonon.enableReflections = true;
        phonon.directBinauralEnabled = true;
    }
}