using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<Item> itemPrefabs;
    public float spawnRadius = 2f;     
    public float spawnHeight = 0.5f;   
    public float upwardForce = 2f;     

    [Header("Visual")]
    public GameObject spawnEffectPrefab;

    [Header("Debug")]
    public bool showDebugLogs = true;

    private void Start()
    {
        if (showDebugLogs)
        {
            Debug.Log($"ItemSpawner initialized with {itemPrefabs.Count} prefabs");
        }
    }

    public void SpawnItem(Item itemPrefab, Vector3 position)
    {
        if (itemPrefab == null)
        {
            Debug.LogError("ItemSpawner: Item prefab is NULL!");
            return;
        }

        
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(0.4f, spawnRadius);

        Vector3 offset = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad) * distance,
            spawnHeight,
            Mathf.Sin(angle * Mathf.Deg2Rad) * distance
        );

        Vector3 spawnPos = position + offset;

        
        Item spawnedItem = Instantiate(itemPrefab, spawnPos, Quaternion.identity);


        if (showDebugLogs)
            Debug.Log($"ItemSpawner: Spawned {itemPrefab.itemName} at {spawnPos}");
    }

    public void SpawnRandomItem(Vector3 position)
    {
        if (itemPrefabs.Count == 0)
        {
            Debug.LogWarning("ItemSpawner: No items in spawn pool!");
            return;
        }

        Item randomItem = itemPrefabs[Random.Range(0, itemPrefabs.Count)];

        if (showDebugLogs)
            Debug.Log($"ItemSpawner: Random → {randomItem.itemName}");

        SpawnItem(randomItem, position);
    }

    public void SpawnMultipleItems(Vector3 position, int count)
    {
        if (showDebugLogs)
            Debug.Log($"ItemSpawner: Spawning {count} items");

        for (int i = 0; i < count; i++)
        {
            SpawnRandomItem(position);
        }
    }
}
