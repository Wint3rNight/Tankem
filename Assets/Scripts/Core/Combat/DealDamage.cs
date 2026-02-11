using System;
using Unity.Netcode;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private int damage = 17;

    private ulong _ownerClientID;
    
    public void SetOwner(ulong ownerClientID)
    {
        this._ownerClientID = ownerClientID;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        if (col.attachedRigidbody == null)
        {
            return;
        }
        
        if (col.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject networkObject))
        {
            if (_ownerClientID == networkObject.OwnerClientId)
            {
                return;
            }
        }
    
        if (col.attachedRigidbody.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    
}