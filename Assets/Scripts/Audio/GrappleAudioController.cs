using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleAudioController : MonoBehaviour
{
    public AudioSource grappleAudioSource;

    private void Start()
    {
        GrappleScript grappleScript = FindObjectOfType<GrappleScript>();
        grappleScript.OnGrappleHit += HandleGrappleHit;
    }

    private void HandleGrappleHit(AudioClip audioClip)
    {
        grappleAudioSource.Play();
    }
}
