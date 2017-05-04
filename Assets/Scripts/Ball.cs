using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Valve.VR.InteractionSystem;

// Purpose: Handle collision logic for ball, play sounds, and reset ball to start position when it touches the ground

public class Ball : MonoBehaviour {

    public float maxPitchCollectable;
    public float minPitchCollectable;

    public delegate void BallTouchedGround();
    public delegate void BallTouchedGoal();
    public delegate void BallTouchedCollectable();

    public static event BallTouchedGround ballTouchedGround;
    public static event BallTouchedGround ballTouchedGoal;
    public static event BallTouchedGround ballTouchedCollectable;

    private PlaySound collectableSound;
    private PlaySound trampolineSound;
    private PlaySound metalPlankSound;

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

    private List<PlaySound> playSounds;

    private void OnEnable()
    {
        Structure.onAttachedToHand += OnAttachedToHand;
        Structure.onDetachedFromHand += OnDetachedFromHand;
    }

    private void Awake()
    {
        InitializeBall();
        LoadAudio();
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
            trampolineSound.Play();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("MetalPlank"))
        {
            if (!SoundManager.AudioSourcePlaying(metalPlankSound))
            {
                metalPlankSound.Play();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MetalPlank"))
        {
            StartCoroutine(StopAfterDelay(metalPlankSound, 0.7f));
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
            // Determine collectable sound pitch (pitches increases as less collectables remain)
            int numberOfCollectablesPickedUp = CollectablesManager.collectables.Length - CollectablesManager.CollectablesRemaining();
            float pitchIncrement = (maxPitchCollectable - minPitchCollectable) / CollectablesManager.collectables.Length;
            float currentPitch = minPitchCollectable + (numberOfCollectablesPickedUp * pitchIncrement);

            if (currentPitch <= 0)
            {
                currentPitch = minPitchCollectable;
            }

            SoundManager.SetAudioSourcePitch(collectableSound, currentPitch);
            collectableSound.Play();

            // Disable the collectable
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
        // Reset the ball to its starting position
        gameObject.transform.position = resetPosition;
        rigidBody.velocity = resetVelocity;
        rigidBody.angularVelocity = resetAngularVelocity;

        // Stop any sounds that were playing before the reset
        SoundManager.StopAllAudio(playSounds);

        // Keep ball deactivated if player holds onto structure as ball resets (prevents cheating)
        if (structureAttachedToHand)
        {
            DeactivateBall();
        }
        else
        {
            ActivateBall();
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

    private void LoadAudio()
    {
        playSounds = new List<PlaySound>();

        // Collectable
        collectableSound = SoundManager.LoadAudio(gameObject, new List<string> { "Sounds/Effects/Collectable" }, 0.30f, false, false, false, "Effects", 2f, 1f);

        //Trampoline
        List<string> trampolineAudioPaths = new List<string>();
        trampolineAudioPaths.Add("Sounds/Effects/Trampoline1");
        trampolineAudioPaths.Add("Sounds/Effects/Trampoline2");
        trampolineAudioPaths.Add("Sounds/Effects/Trampoline3");

        trampolineSound = SoundManager.LoadAudio(gameObject, trampolineAudioPaths, 0.50f, false, false, false, "Effects");

        // Metal Plank
        metalPlankSound = SoundManager.LoadAudio(gameObject, new List<string> { "Sounds/Effects/RollingBallMetal" }, 0.25f, false, false, false, "Effects");

        playSounds.Add(collectableSound);
        playSounds.Add(trampolineSound);
        playSounds.Add(metalPlankSound);
    }
}