using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Teleporter : MonoBehaviour {

    public GameObject teleportDestination;

    private AudioSource teleportTargetAudioSource;
    private AudioSource teleportDestinationAudioSource;
    private AudioClip teleportTargetAudioClip;
    private AudioClip teleportDestinationAudioClip;

    private GameObject teleportingObject;
    private Vector3 destination;

    private void Awake()
    {
        destination = teleportDestination.transform.position;

        teleportTargetAudioSource = gameObject.GetComponent<AudioSource>();
        teleportDestinationAudioSource = teleportDestination.GetComponent<AudioSource>();
    }

    public void Teleport(GameObject go)
    {
        StartCoroutine(TeleportToDestination(go));
    }

    private IEnumerator TeleportToDestination(GameObject go)
    {
        Rigidbody rigidBody = go.GetComponent<Rigidbody>();

        teleportTargetAudioSource.Play();
        go.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        teleportDestinationAudioSource.Play();
        go.SetActive(true);

        go.transform.position = destination;
        rigidBody.velocity = teleportDestination.transform.forward * rigidBody.velocity.magnitude;
    }
}
