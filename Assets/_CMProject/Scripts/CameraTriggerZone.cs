using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraTriggerZone : MonoBehaviour
{
    public CinemachineCamera topDownCamera;
    public CinemachineCamera firstPersonCamera;
    public CinemachineCamera thirdPersonCamera;

    [Tooltip("Reference to the player GameObject with PlayerInput")]
    public GameObject player;

    private PlayerInput playerInput;
    private CinemachineCamera previousCamera;

    private void Awake()
    {
        if (player != null)
            playerInput = player.GetComponent<PlayerInput>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != player) return;

        // Store currently active gameplay camera
        if (firstPersonCamera.Priority > thirdPersonCamera.Priority)
            previousCamera = firstPersonCamera;
        else
            previousCamera = thirdPersonCamera;

        // Switch to TopDown
        topDownCamera.Priority = 100;
        firstPersonCamera.Priority = 10;
        thirdPersonCamera.Priority = 10;

        // Disable input
        //playerInput.DeactivateInput();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != player) return;

        // Restore previous camera
        if (previousCamera != null)
            previousCamera.Priority = 100;

        topDownCamera.Priority = 10;

        // Re-enable input
        //playerInput.ActivateInput();mel
    }
}
