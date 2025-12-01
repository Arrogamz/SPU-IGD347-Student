using UnityEngine;

public class Potion : Item
{
    [Header("Potion Settings")]
    public int AmountHealth = 20;

    [Header("Visual (Optional)")]
    public GameObject collectEffectPrefab;

    private void Start()
    {
        itemType = ItemType.Consumable;

        if (string.IsNullOrEmpty(itemName))
        {
            itemName = "Health Potion";
        }
    }

    public override void OnCollect(Player player)
    {
        base.OnCollect(player);

        // ตรวจสอบว่าเก็บได้อีกไหม
        if (player.potionCount >= player.maxPotions)
        {
            Debug.Log($"❌ Cannot carry more potions! Max: {player.maxPotions}");
            return;
        }

        // ✅ เพิ่มจำนวน Potion แทนเก็บในวัตถุ
        player.potionCount++;
        Debug.Log($"✨ Collected {itemName}. Total potions: {player.potionCount}/{player.maxPotions}");

        // แสดง Effect
        if (collectEffectPrefab != null)
        {
            GameObject effect = Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }

        // ✅ Destroy ตัวจริง (ไม่เก็บใน Inventory)
        Destroy(gameObject);
    }

    public override void Use(Player player)
    {
        base.Use(player);

        if (player.health >= player.maxHealth)
        {
            Debug.Log("❌ HP is already full!");
            return;
        }

        player.Heal(AmountHealth);
        player.potionCount--;

        Debug.Log($"💊 {player.Name} used {itemName} and healed {AmountHealth} HP! Remaining: {player.potionCount}");
    }
}