using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DropTable", menuName = "Game/DropTable")]
public class DropTable : ScriptableObject
{
    public List<DropEntry> drops = new List<DropEntry>();

    [Header("Settings")]
    public bool guaranteedDrop = true;

    [Header("Debug")]
    public bool showDebugLogs = true;

    public List<Item> GetDrops()
    {
        List<Item> droppedItems = new List<Item>();

        if (showDebugLogs)
        {
            Debug.Log($"🎲 DropTable [{name}]: Rolling for {drops.Count} possible drops...");
        }

        foreach (DropEntry entry in drops)
        {
            if (entry.itemPrefab == null)
            {
                Debug.LogWarning($"⚠️ DropTable [{name}]: Entry has NULL item prefab!");
                continue;
            }

            float roll = Random.Range(0f, 100f);

            if (showDebugLogs)
            {
                Debug.Log($"   🎲 {entry.itemPrefab.itemName}: Roll {roll:F1}% vs {entry.dropChance}%");
            }

            if (roll <= entry.dropChance)
            {
                int amount = Random.Range(entry.minAmount, entry.maxAmount + 1);

                for (int i = 0; i < amount; i++)
                {
                    droppedItems.Add(entry.itemPrefab);
                }

                if (showDebugLogs)
                {
                    Debug.Log($"   ✅ Dropped {amount}x {entry.itemPrefab.itemName}");
                }
            }
            else
            {
                if (showDebugLogs)
                {
                    Debug.Log($"   ❌ Failed roll for {entry.itemPrefab.itemName}");
                }
            }
        }

        // ถ้าไม่ได้อะไรเลย และตั้ง guaranteedDrop
        if (droppedItems.Count == 0 && guaranteedDrop && drops.Count > 0)
        {
            DropEntry randomEntry = drops[Random.Range(0, drops.Count)];

            if (randomEntry.itemPrefab != null)
            {
                droppedItems.Add(randomEntry.itemPrefab);

                if (showDebugLogs)
                {
                    Debug.Log($"   🎁 Guaranteed drop: {randomEntry.itemPrefab.itemName}");
                }
            }
        }

        if (showDebugLogs)
        {
            Debug.Log($"📦 DropTable [{name}]: Total drops: {droppedItems.Count}");
        }

        return droppedItems;
    }

    public Item GetRandomDrop()
    {
        if (drops.Count == 0)
        {
            Debug.LogWarning($"⚠️ DropTable [{name}]: No drops available!");
            return null;
        }

        // Weighted Random
        float totalWeight = 0f;
        foreach (DropEntry entry in drops)
        {
            totalWeight += entry.dropChance;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (DropEntry entry in drops)
        {
            cumulative += entry.dropChance;
            if (randomValue <= cumulative)
            {
                return entry.itemPrefab;
            }
        }

        return drops[0].itemPrefab;
    }
}