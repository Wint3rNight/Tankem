using System;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;

    private void OnDestroy()
    {
        Instantiate(spawnPrefab, transform.position, Quaternion.identity);
    }
}
