using System;
using UnityEngine;

public class RespawnCoin : Coin
{
    public event Action<RespawnCoin> OnCollected;
    
    private Vector3 _previousPosition;

    private void Update()
    {
        if (_previousPosition != transform.position)
        {
            Show(true);
        }
        _previousPosition = transform.position;
    }

    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }
        if(AlreadyCollected)
        {
            return 0;
        }
        AlreadyCollected = true;
        
        OnCollected?.Invoke(this);
        
        return CoinValue;
    }

    public void Reset()
    {
        AlreadyCollected = false;
    }
}
