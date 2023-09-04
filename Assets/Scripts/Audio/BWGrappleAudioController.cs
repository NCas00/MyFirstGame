using UnityEngine;

public class BWGrappleAudioController : MonoBehaviour
{
    public AudioSource grappleAudioSource;

    private void Start()
    {
        BWPlayerController bWPlayerController = FindObjectOfType<BWPlayerController>();
        bWPlayerController.BWOnGrappleHit += HandleGrappleHit;
    }

    private void HandleGrappleHit(AudioClip audioClip)
    {
        grappleAudioSource.Play();
    }
}
