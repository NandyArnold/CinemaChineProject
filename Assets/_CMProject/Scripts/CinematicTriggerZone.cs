using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CinematicTriggerZone : MonoBehaviour
{
    public CinemachineCamera firstPersonCamera;
    public CinemachineCamera thirdPersonCamera;
    public CinemachineCamera topDownCamera;
    public CinemachineCamera cinematicCamera;

    public GameObject player;
    public PlayableDirector cinematicTimeline;

    private CinemachineCamera previousCamera;
    //public PlayableDirector director;
    private bool cinematicPlaying = false;

    void Start()
    {
        cinematicTimeline.time = 0;
        cinematicTimeline.Stop(); // Ensures Timeline doesn't hold any evaluated state
        cinematicTimeline.Evaluate(); // Force it to snap to start frame without playing
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || cinematicPlaying) return;

        cinematicPlaying = true;

        // Store currently active camera
        previousCamera = GetActiveGameplayCamera();

        // Drop all gameplay cameras' priority
        firstPersonCamera.Priority = 10;
        thirdPersonCamera.Priority = 10;
        topDownCamera.Priority = 10;

        // Boost cinematic
        cinematicCamera.Priority = 100;

        // Play Timeline
        cinematicTimeline.Play();

        // Optional: disable input
        //player.GetComponent<PlayerInput>()?.DeactivateInput();

        // Wait for timeline end
        StartCoroutine(EndAfterTimeline());
    }

    private IEnumerator EndAfterTimeline()
    {
        yield return new WaitForSeconds((float)cinematicTimeline.duration + 0.2f);

        // Lower cinematic camera
        cinematicCamera.Priority = 10;

        // Restore previous gameplay camera
        if (previousCamera != null)
            previousCamera.Priority = 100;

        // Re-enable input
        //player.GetComponent<PlayerInput>()?.ActivateInput();

        cinematicPlaying = false;
    }

    private CinemachineCamera GetActiveGameplayCamera()
    {
        if (firstPersonCamera.Priority > thirdPersonCamera.Priority &&
            firstPersonCamera.Priority > topDownCamera.Priority)
            return firstPersonCamera;

        if (thirdPersonCamera.Priority > topDownCamera.Priority)
            return thirdPersonCamera;

        return topDownCamera;
    }
}
