using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("Hand setting")]
    public Transform RightHand;
    public Transform LeftHand;
    public List<Item> inventory = new List<Item>();

    [Header("Potion System")]
    public int potionCount = 0;
    public int maxPotions = 10;
    public int potionHealAmount = 20;

    [Header("Weapon System")] 
    public Sword currentWeapon;
    public float dropDistance = 1.5f;

    
    Vector3 _inputDirection;
    bool _isAttacking = false;
    bool _isInteract = false;

    private bool showInventory = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        health = maxHealth;
    }

    public void FixedUpdate()
    {
        Move(_inputDirection);
        Turn(_inputDirection);
        Attack(_isAttacking);
        Interact(_isInteract);
    }

    public void Update()
    {
        HandleInput();

        if (Input.GetKeyDown(KeyCode.H))
        {
            UsePotion();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            showInventory = !showInventory;
        }

        
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropCurrentWeapon();
        }
    }

    public void AddItem(Item item)
    {
        inventory.Add(item);
    }

    private void HandleInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        _inputDirection = new Vector3(x, 0, y);

        if (Input.GetMouseButtonDown(0))
        {
            _isAttacking = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _isInteract = true;
        }
    }

    public void Attack(bool isAttacking)
    {
        if (isAttacking)
        {
            animator.SetTrigger("Attack");
            var e = InFront as Idestoryable;
            if (e != null)
            {
                e.TakeDamage(Damage);
                Debug.Log($"{gameObject.name} attacks for {Damage} damage.");
            }
            _isAttacking = false;
        }
    }

    private void Interact(bool interactable)
    {
        if (interactable)
        {
            IInteractable e = InFront as IInteractable;
            if (e != null)
            {
                e.Interact(this);
            }
            _isInteract = false;
        }
    }

    private void UsePotion()
    {
        if (potionCount <= 0)
        {
            Debug.Log("❌ No potion in inventory!");
            return;
        }

        if (health >= maxHealth)
        {
            Debug.Log("❌ HP is already full!");
            return;
        }

        Heal(potionHealAmount);
        potionCount--;

        Debug.Log($"✨ Used potion! Healed {potionHealAmount} HP. Remaining: {potionCount}/{maxPotions}");
    }

    
    public void DropCurrentWeapon()
    {
        if (currentWeapon == null)
        {
            Debug.Log("❌ No weapon to drop!");
            return;
        }

        Vector3 dropPosition = transform.position + transform.forward * dropDistance;

        
        currentWeapon.transform.parent = null;
        currentWeapon.transform.position = dropPosition;
        currentWeapon.transform.rotation = Quaternion.identity;

        
        currentWeapon.itemcollider.enabled = true;

        
        Damage -= currentWeapon.Damage;

        
        currentWeapon = null;

        Debug.Log($"📦 Dropped weapon!");
    }

    
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 14;
        style.normal.textColor = Color.white;

        
        GUI.Label(new Rect(10, 10, 300, 20), $"💚 HP: {health}/{maxHealth}", style);
        GUI.Label(new Rect(10, 30, 300, 20), $"💊 Potions [H]: {potionCount}/{maxPotions}", style);

        
        if (currentWeapon != null)
        {
            GUI.Label(new Rect(10, 50, 300, 20), $"⚔️ Weapon: {currentWeapon.itemName}", style);
            GUI.Label(new Rect(10, 70, 300, 20), $"   Damage: +{currentWeapon.Damage}", style);
        }
        else
        {
            GUI.Label(new Rect(10, 50, 300, 20), $"👊 Weapon: Bare Hands", style);
        }

        
        if (showInventory)
        {
            DrawInventoryUI();
        }
        else
        {
            GUIStyle hintStyle = new GUIStyle(GUI.skin.label);
            hintStyle.fontSize = 12;
            hintStyle.normal.textColor = Color.yellow;
            GUI.Label(new Rect(10, 90, 300, 20), "Press [I] to view inventory", hintStyle);
            GUI.Label(new Rect(10, 110, 300, 20), "Press [G] to drop weapon", hintStyle);
        }
    }

    private void DrawInventoryUI()
    {
        
        GUI.Box(new Rect(Screen.width / 2 - 200, 50, 400, 400), "");

        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 18;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.cyan;
        titleStyle.alignment = TextAnchor.MiddleCenter;

        GUI.Label(new Rect(Screen.width / 2 - 200, 60, 400, 30), "📦 INVENTORY", titleStyle);

        GUIStyle itemStyle = new GUIStyle(GUI.skin.label);
        itemStyle.fontSize = 14;
        itemStyle.normal.textColor = Color.white;

        int yPos = 100;

        
        GUI.Label(new Rect(Screen.width / 2 - 180, yPos, 360, 25),
            $"💊 Health Potion: {potionCount}/{maxPotions}", itemStyle);
        yPos += 30;

        
        GUI.Label(new Rect(Screen.width / 2 - 180, yPos, 360, 2), "─────────────────────", itemStyle);
        yPos += 20;

        
        if (inventory.Count == 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 180, yPos, 360, 25),
                "No items in inventory", itemStyle);
        }
        else
        {
            foreach (Item item in inventory)
            {
                if (item != null)
                {
                    string icon = item.itemType == ItemType.Weapon ? "⚔️" :
                                 item.itemType == ItemType.Consumable ? "💊" :
                                 item.itemType == ItemType.Collectable ? "💎" : "📜";

                    GUI.Label(new Rect(Screen.width / 2 - 180, yPos, 360, 25),
                        $"{icon} {item.itemName} ({item.rarity})", itemStyle);
                    yPos += 25;
                }
            }
        }

        GUIStyle closeStyle = new GUIStyle(GUI.skin.label);
        closeStyle.fontSize = 12;
        closeStyle.normal.textColor = Color.yellow;
        closeStyle.alignment = TextAnchor.MiddleCenter;

        GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 150, 400, 25),
            "Press [I] to close", closeStyle);
    }
}