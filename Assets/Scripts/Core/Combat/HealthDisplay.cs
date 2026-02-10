using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class HealthDisplay : NetworkBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image healthBarImage;

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

    private void HandleHealthChange(int oldHealth, int newHealth)
    {
        if (healthBarImage == null || health == null)
        {
            return;
        }
        healthBarImage.fillAmount = (float)newHealth / health.MaxHealth;
    }
    
}
