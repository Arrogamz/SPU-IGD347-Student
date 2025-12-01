using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Room Settings")]
    public Enemy[] enemies;
    public AutoDoor exitDoor;

    [Header("Loot Box (Optional)")]
    public GameObject lootBoxPrefab;        
    public Transform lootBoxSpawnPoint;    

    [Header("Debug")]
    public bool showDebugLogs = true;

    private bool roomCleared = false;

    private void Update()
    {
        if (roomCleared) return;

        
        bool allEnemiesDead = true;

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && enemy.gameObject.activeSelf)
            {
                allEnemiesDead = false;
                break;
            }
        }

        
        if (allEnemiesDead)
        {
            roomCleared = true;

            ClearRoom();
        }
    }

    
    private void ClearRoom()
    {
        
        if (exitDoor != null)
        {
            exitDoor.OpenDoorAutomatically();
        }
        else
        {
            Debug.LogWarning("No Door");
        }

        SpawnLootBox();
    }

    private void SpawnLootBox()
    {
        if (lootBoxPrefab == null) return;

        Vector3 spawnPos = lootBoxSpawnPoint != null
            ? lootBoxSpawnPoint.position
            : transform.position;

        GameObject lootBox = Instantiate(lootBoxPrefab, spawnPos, Quaternion.identity);

    }
}