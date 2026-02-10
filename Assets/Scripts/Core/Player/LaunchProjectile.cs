using Unity.Netcode;
using UnityEngine;

public class LaunchProjectile : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private GameObject flashPrefab;
    [SerializeField] private Collider2D playerCollider;
    
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float flashDuration;
    
    private bool _shouldFire;
    private float _prevFireTime;
    private float _flashTimer;
    
    
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        inputReader.PrimaryFireEvent += HandlePrimaryFire;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }
        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }
    
    private void Update()
    {
        if(_flashTimer>0f)
        {
            _flashTimer -= Time.deltaTime;
            if(_flashTimer<=0f)
            {
                flashPrefab.SetActive(false);
            }
        }
        
        if(!IsOwner)
        {
            return;
        }

        if (!_shouldFire)
        {
            return;
        }

        if (Time.time < _prevFireTime+(1f / fireRate))
        {
            return;
        }
        
        PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);
        SpawnDummyProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);
        
        _prevFireTime = Time.time;
    }
    
    private void HandlePrimaryFire(bool shouldFire)
    {
        this._shouldFire = shouldFire;
    }

    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 spawnPos, Vector3 direction)
    {
        GameObject projectileInstance = Instantiate(serverProjectilePrefab, spawnPos, Quaternion.identity);
        
        projectileInstance.transform.up = direction;
        
        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

        if (projectileInstance.TryGetComponent<DealDamage>(out DealDamage dealDamage))
        {
            dealDamage.SetOwner(OwnerClientId);
        }
        
        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = rb.transform.up * projectileSpeed;
        }
        
        SpawnDummyProjectileClientRpc(spawnPos, direction);
    }

    [ClientRpc]
    private void SpawnDummyProjectileClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (IsOwner)
        {
            return;
        }
        SpawnDummyProjectile(spawnPos, direction);
    }
    
    private void SpawnDummyProjectile(Vector3 spawnPos, Vector3 direction)
    {
        flashPrefab.SetActive(true);
        _flashTimer = flashDuration;
        
        GameObject projectileInstance = Instantiate(clientProjectilePrefab, spawnPos, Quaternion.identity);
        
        projectileInstance.transform.up = direction;
        
        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = rb.transform.up * projectileSpeed;
        }
    }
}
