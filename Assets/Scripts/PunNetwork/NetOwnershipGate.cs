using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetOwnershipGate : MonoBehaviourPun
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Camera playerCamera;

    private void Awake()
    {
        bool isMine = photonView.IsMine;

        if (playerController)
            playerController.enabled = isMine;

        if (playerInput)
            playerInput.enabled = isMine;

        if (playerCamera)
        {
            playerCamera.enabled = isMine;
            var listener = playerCamera.GetComponent<AudioListener>();
            if (listener)
                listener.enabled = isMine;
        }
    }
}
