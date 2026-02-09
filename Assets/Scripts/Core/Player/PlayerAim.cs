using Unity.Netcode;
using UnityEngine;

public class PlayerAim : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform turretTransform;
    
    private void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        Vector2 aimPos = inputReader.AimPosition;
        Vector2 aimWorldPos = Camera.main.ScreenToWorldPoint(aimPos);
        
        turretTransform.up = new Vector2(aimWorldPos.x - turretTransform.position.x, aimWorldPos.y - turretTransform.position.y);
    }
}
