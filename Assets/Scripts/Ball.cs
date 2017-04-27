using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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

    private PlaySound collectablePlaySound;
    private PlaySound trampolinePlaySound;
    private PlaySound metalPlankPlaySound;

    private AudioSource collectableAudioSource;
    private AudioSource trampolineAudioSource;
    private AudioSource metalPlankAudioSource;

    private Vector3 resetPosition;
    private Vector3 resetVelocity;

    private static bool ballWithinPlatformBounds;
    private static Renderer ballRenderer;
    private static bool ballActive = true;
    private static Material ballInactiveMaterial;
    private Material ballActiveMaterial;


    AudioMixer audioMixer;

    private void Awake()
    {
        InitializeBall();

        audioMixer = Resources.Load("MasterMixer") as AudioMixer;
    }

    private void Start()
    {
        LoadCollectableAudio();
        LoadTrampolineAudio();
        LoadMetalPlankAudio();
    }

    public static bool BallWithinPlatformBounds
    {
        get { return ballWithinPlatformBounds; }
    }

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
        else if (collision.gameObject.CompareTag("MetalPlank"))
        {
            metalPlankPlaySound.PlayLooping();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MetalPlank"))
        {
            metalPlankPlaySound.Stop();
        }
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
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("PlatformBounds"))
        {
            ballWithinPlatformBounds = false;
        }
    }

    private void InitializeBall()
    {
        resetPosition = transform.position;
        resetVelocity = gameObject.GetComponent<Rigidbody>().velocity;

        ballActiveMaterial = Resources.Load<Material>("Materials/BallActive");
        ballInactiveMaterial = Resources.Load<Material>("Materials/BallInactive");
        ballRenderer = gameObject.GetComponent<Renderer>();
    }

    private void ResetBall()
    {
        gameObject.transform.position = resetPosition;
        gameObject.transform.GetComponent<Rigidbody>().velocity = resetVelocity;
        ActivateBall();
    }

    public static void DeactivateBall()
    {
        ballRenderer.material = ballInactiveMaterial;
        ballActive = false;
    }

    private void ActivateBall()
    {
        ballRenderer.material = ballActiveMaterial;
        ballActive = true;
    }

    private void LoadCollectableAudio()
    {
        AudioClip clip1 = Resources.Load<AudioClip>("Sounds/Effects/Collectable");
        AudioClip[] clips = new AudioClip[1];
        clips[0] = clip1;

        collectablePlaySound = collectableAudio.AddComponent<PlaySound>();
        collectablePlaySound.waveFile = clips;

        collectableAudioSource = collectableAudio.GetComponent<AudioSource>();
        collectableAudioSource.playOnAwake = false;
        collectableAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effects")[0];

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
        trampolineAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effects")[0];

        LoadPhononEffect(trampolineAudio);
    }

    private void LoadMetalPlankAudio()
    {
        AudioClip clip1 = Resources.Load<AudioClip>("Sounds/Effects/RollingBallMetal");

        AudioClip[] clips = new AudioClip[1];
        clips[0] = clip1;

        metalPlankPlaySound = metalPlankAudio.AddComponent<PlaySound>();
        metalPlankPlaySound.useRandomVolume = false;
        metalPlankPlaySound.stopOnPlay = true;
        metalPlankPlaySound.looping = true;
        metalPlankPlaySound.waveFile = clips;

        metalPlankAudioSource = metalPlankAudio.GetComponent<AudioSource>();
        metalPlankAudioSource.playOnAwake = false;
        metalPlankAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effects")[0];

        LoadPhononEffect(metalPlankAudio);
    }

    private void LoadPhononEffect(GameObject go)
    {
        Phonon.PhononSource phonon = go.AddComponent<Phonon.PhononSource>();
        phonon.enableReflections = true;
        phonon.directBinauralEnabled = true;
    }
}