using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraSwitchHandler : MonoBehaviour
{
    public CinemachineCamera thirdPersonCam;
    public CinemachineCamera firstPersonCam;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Scoped.started += OnScopedStarted;
        inputActions.Player.Scoped.canceled += OnScopedCanceled;
    }

    private void OnDisable()
    {
        inputActions.Player.Scoped.started -= OnScopedStarted;
        inputActions.Player.Scoped.canceled -= OnScopedCanceled;
        inputActions.Disable();
    }

    private void OnScopedStarted(InputAction.CallbackContext context)
    {
        // Switch to First Person
        firstPersonCam.Priority = 20;
        thirdPersonCam.Priority = 10;
    }

    private void OnScopedCanceled(InputAction.CallbackContext context)
    {
        // Switch back to Third Person
        firstPersonCam.Priority = 5;
        thirdPersonCam.Priority = 20;
    }
}

