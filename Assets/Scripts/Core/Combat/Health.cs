using System;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [field:SerializeField] public int MaxHealth { get; private set; }=100;
    
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    
    private bool _isDead;
    
    public Action<Health> OnDeath;
    
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }
        currentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int damageValue)
    {
        ModifyHealth(-damageValue);
    }

    public void Heal(int healValue)
    {
        ModifyHealth(healValue);
    }

    private void ModifyHealth(int value)
    {
        if (_isDead)
        {
            return;
        }
        int newHealth = currentHealth.Value + value;
        currentHealth.Value = Mathf.Clamp(newHealth, 0, MaxHealth);
        
        if (currentHealth.Value <= 0)
        {
            _isDead = true;
            OnDeath?.Invoke(this);
        }
    }
}
