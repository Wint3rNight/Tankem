using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class HealthDisplay : NetworkBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image healthBar;

    public override void OnNetworkSpawn()
    {
        if (!IsClient)
        {
            return;
        }
        health.currentHealth.OnValueChanged += HandleHealthChange;
        HandleHealthChange(0, health.currentHealth.Value);
    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient)
        {
            return;
        }
        health.currentHealth.OnValueChanged -= HandleHealthChange;
    }

    private void HandleHealthChange(int oldValue, int newValue)
    {
        healthBar.fillAmount = (float)newValue / health.MaxHealth;
    }
    
}
