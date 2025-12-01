using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropEntry
{
    
    public Item itemPrefab;

    [Range(0, 100)]
    public float dropChance = 50f;

    public int minAmount = 1;
    public int maxAmount = 1;
}