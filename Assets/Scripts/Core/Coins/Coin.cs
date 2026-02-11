using Unity.Netcode;
using UnityEngine;

public abstract class Coin : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    protected int CoinValue = 10;
    protected bool AlreadyCollected;

    public abstract int Collect();
    
    public void SetCoinValue(int coinValue)
    {
        CoinValue = coinValue;
    }

    protected void Show(bool show)
    {
        spriteRenderer.enabled = show;
    }
}
