using UnityEngine;
public enum ItemType
{
    Weapon,
    Consumable,
    Collectable,
}

public enum Rarity
{
    Common,
    Rare,
    Epic
}

[RequireComponent(typeof(SphereCollider))]
public class Item : Identity
{
    [Header("Item Info")]
    public string itemName;
    public ItemType itemType;
    public Rarity rarity = Rarity.Common;
    public Sprite icon;

    public Collider _collider;
    public Collider itemcollider
    {
        get
        {
            if (_collider == null)
            {
                _collider = GetComponent<Collider>();
                _collider.isTrigger = true;
            }
            return _collider;
        }
    }

    public override void SetUP()
    {
        base.SetUP();
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    public Item()
    {

    }

    public Item(Item item)
    {
        this.Name = item.Name;
        this.itemName = item.itemName;
        this.itemType = item.itemType;
        this.rarity = item.rarity;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player p = other.GetComponent<Player>();
            if (p != null)
            {
                OnCollect(p);
            }
        }
    }

    public virtual void OnCollect(Player player)
    {
        Debug.Log($"Collected {itemName} ({rarity})");
    }

    public virtual void Use(Player player)
    {
        Debug.Log($"Using {itemName}");
    }
}

