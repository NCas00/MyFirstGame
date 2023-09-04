using UnityEngine;

public class BRGrappleAudioController : MonoBehaviour
{
    public AudioSource grappleAudioSource;

    private void Start()
    {
       BRPlayerController bRPlayerController = FindObjectOfType<BRPlayerController>();
       bRPlayerController.BROnGrappleHit += HandleGrappleHit;
    }

    private void HandleGrappleHit(AudioClip audioClip)
    {
        grappleAudioSource.Play();
    }
}
