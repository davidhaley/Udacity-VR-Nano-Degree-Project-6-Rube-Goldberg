using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Teleporter : MonoBehaviour {

    public GameObject teleportDestination;
    public float force;

    private AudioSource teleportTargetAudioSource;
    private AudioSource teleportDestinationAudioSource;
    private AudioClip teleportTargetAudioClip;
    private AudioClip teleportDestinationAudioClip;

    private GameObject goToTeleport;
    private Vector3 destination;

    private void Awake()
    {
        destination = teleportDestination.transform.position;

        teleportTargetAudioSource = gameObject.GetComponent<AudioSource>();
        teleportDestinationAudioSource = teleportDestination.GetComponent<AudioSource>();
        //teleportTargetAudioClip = Resources.Load<AudioClip>("Sounds/Effects/TeleportTarget");
        //teleportDestinationAudioClip = Resources.Load<AudioClip>("Sounds/Effects/TeleportDestination");

        //teleportTargetAudioSource.clip = teleportTargetAudioClip;
        //teleportDestinationAudioSource.clip = teleportDestinationAudioClip;
    }

    public void Teleport(GameObject go)
    {
        goToTeleport = go;
        StartCoroutine(TeleportToDestination(goToTeleport));
    }

    private IEnumerator TeleportToDestination(GameObject go)
    {
        teleportTargetAudioSource.Play();
        goToTeleport.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        teleportDestinationAudioSource.Play();
        goToTeleport.SetActive(true);

        go.transform.position = destination;
        Rigidbody rigidBody = go.GetComponent<Rigidbody>();
        rigidBody.velocity = teleportDestination.transform.forward * force;
    }
}
