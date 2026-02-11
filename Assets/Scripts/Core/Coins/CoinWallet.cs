using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class CoinWallet : NetworkBehaviour
{
    [FormerlySerializedAs("TotalCoins")] public NetworkVariable<int> totalCoins = new NetworkVariable<int>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Coin>(out Coin coin))
        {
            return;
        }

        int coinValue = coin.Collect();

        if (!IsServer)
        {
            return;
        }
        
        totalCoins.Value += coinValue;
    }

    public void SpendCoins(int costToFire)
    {
        totalCoins.Value -= costToFire;
    }
}
