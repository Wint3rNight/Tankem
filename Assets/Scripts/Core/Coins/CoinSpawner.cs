using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    [SerializeField] private RespawnCoin coinPrefab;
    [SerializeField] private int maxCoins = 20;
    [SerializeField] private int coinValue = 10;
    [SerializeField] private Vector2 xSpawnRange;
    [SerializeField] private Vector2 ySpawnRange;
    [SerializeField] private LayerMask layerMask;
    
    private readonly Collider2D[] _coinBuffer = new Collider2D[1];
    
    private float _coinRadius;
    
    private ContactFilter2D _contactFilter;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }
        
        _coinRadius=coinPrefab.GetComponent<CircleCollider2D>().radius;
        
        _contactFilter = new ContactFilter2D();
        _contactFilter.SetLayerMask(layerMask);
        _contactFilter.useTriggers = true;
        
        for(int i=0; i<maxCoins; i++)
        {
            SpawnCoin();
        }
    }
    
    private void SpawnCoin()
    {
        RespawnCoin coinInstance= Instantiate(coinPrefab, GetRandomSpawnPoint(), Quaternion.identity);
        coinInstance.SetCoinValue(coinValue);
        coinInstance.GetComponent<NetworkObject>().Spawn();
        
        coinInstance.OnCollected+=HandleCoinCollected;
    }

    private void HandleCoinCollected(RespawnCoin coin)
    {
        coin.transform.position = GetRandomSpawnPoint();
        coin.Reset();
        
    }

    private Vector2 GetRandomSpawnPoint()
    {
        while (true)
        {
            float x = Random.Range(xSpawnRange.x, xSpawnRange.y);
            float y = Random.Range(ySpawnRange.x, ySpawnRange.y);
            Vector2 spawnPoint = new Vector2(x, y);
            
            int numColliders = Physics2D.OverlapCircle(spawnPoint, _coinRadius, _contactFilter, _coinBuffer);
            
            if (numColliders==0)
            {   
                return spawnPoint;
            }
        }
    }
}
