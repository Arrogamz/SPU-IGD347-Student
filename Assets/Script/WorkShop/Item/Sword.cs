using UnityEngine;
using TMPro;

public class Sword : Item, IInteractable
{
    public int Damage = 25;

                

    private bool isEquipped = false;
    public bool isInteractable { get => !isEquipped; set => isEquipped = !value; }

    public Sword(Sword sword) : base(sword)
    {
        Damage = sword.Damage;
    }

    private void Start()
    {
        itemType = ItemType.Weapon;
        if (string.IsNullOrEmpty(itemName))
        {
            itemName = "Sword";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player p = other.GetComponent<Player>();
            if (p != null)
            {
                Debug.Log($" Press [E] to pickup {itemName}!");
            }
        }
    }

    public override void OnCollect(Player player)
    {
        base.OnCollect(player);

        if (player.currentWeapon != null)
        {
            player.DropCurrentWeapon();
        }

        EquipSword(player);
    }

    private void EquipSword(Player player)
    {
        Vector3 swordUp = new Vector3(90, 0, 0);
        itemcollider.enabled = false;
        transform.parent = player.RightHand;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(swordUp);

        player.currentWeapon = this;
        player.Damage += Damage;
        isEquipped = true;

        Debug.Log($" Equipped {itemName}! Damage: +{Damage}");
    }

    public void Interact(Player player)
    {
        if (!isEquipped)
        {
            OnCollect(player);
        }
        else
        {
            Debug.Log(" Already equipped this sword!");
        }
    }

    
   
}
