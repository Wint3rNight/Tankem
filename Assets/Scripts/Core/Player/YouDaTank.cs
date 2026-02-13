using Unity.Netcode;
using UnityEngine;
using Unity.Cinemachine;
public class PlayerCamera : NetworkBehaviour
{
    
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private int ownerPriority = 15;
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            virtualCamera.Priority = ownerPriority;
        }
    }
    
}
