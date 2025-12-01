using UnityEngine;

public class Enemy : Character
{
    protected enum State { idle, chase, attack, death }

    [Header("Combat")]
    [SerializeField]
    private float TimeToAttack = 1f;
    protected State currentState = State.idle;
    protected float timer = 0f;

    [Header("Vision")]
    public float visionRange = 8f;

    [Header("Item Drop")]
    public DropTable dropTable; 

    [Range(0, 100)]
    public float dropChance = 80f;

    public bool alwaysDrop = true;

    [Header("Debug")]
    public bool showDebugLogs = true;

    private ItemSpawner itemSpawner;

    public override void SetUP()
    {
        base.SetUP();

       
        itemSpawner = FindAnyObjectByType<ItemSpawner>();
        if (itemSpawner == null)
        {
            if (showDebugLogs)
                Debug.LogWarning($" [{Name}] No ItemSpawner found in scene!");
        }
        else
        {
            if (showDebugLogs)
                Debug.Log($" [{Name}] Found ItemSpawner: {itemSpawner.gameObject.name}");
        }

        
        OnDestory += HandleDeath;

        if (showDebugLogs)
            Debug.Log($" [{Name}] Enemy setup complete. Drop chance: {dropChance}%");
    }

    private void Update()
    {
        if (player == null)
        {
            animator.SetBool("Attack", false);
            return;
        }

        Turn(player.transform.position - transform.position);
        timer -= Time.deltaTime;

        float distanceToPlayer = GetDistanPlayer();

        if (distanceToPlayer < 1.5f)
        {
            currentState = State.attack;
            Attack(player);
        }
        if (distanceToPlayer > visionRange)
        {
            currentState = State.idle;
            animator.SetBool("Attack", false);
            return;
        }
    }

    protected override void Turn(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    protected virtual void Attack(Player _player)
    {
        if (timer <= 0)
        {
            _player.TakeDamage(Damage);
            animator.SetBool("Attack", true);
            if (showDebugLogs)
                Debug.Log($" {Name} attacks {_player.Name} for {Damage} damage.");
            timer = TimeToAttack;
        }
    }

    private void HandleDeath(Idestoryable destroyed)
    {
        if (showDebugLogs)
            Debug.Log($" [{Name}] HandleDeath called!");

        DropItems();
    }

    private void DropItems()
    {
        if (showDebugLogs)
            Debug.Log($" [{Name}] DropItems() called. Always Drop: {alwaysDrop}");

        if (itemSpawner == null)
        {
            Debug.LogError($" [{Name}] Cannot drop items: ItemSpawner is NULL!");
            return;
        }

       
        float roll = Random.Range(0f, 100f);

        if (showDebugLogs)
            Debug.Log($" [{Name}] Roll: {roll:F1}% vs Drop Chance: {dropChance}%");

        if (!alwaysDrop && roll > dropChance)
        {
            if (showDebugLogs)
                Debug.Log($" [{Name}] Failed drop roll. No loot.");
            return;
        }

        
        if (dropTable != null)
        {
            var drops = dropTable.GetDrops();

            if (drops.Count > 0)
            {
                if (showDebugLogs)
                    Debug.Log($" [{Name}] Got {drops.Count} items from drop table!");

                foreach (Item itemPrefab in drops)
                {
                    if (itemPrefab != null)
                    {
                        Item spawnedItem = Instantiate(itemPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

                        if (showDebugLogs)
                            Debug.Log($" [{Name}] Spawned: {itemPrefab.itemName}");
                    }
                }
            }
            else
            {
                if (showDebugLogs)
                    Debug.Log($"⚠️ [{Name}] Drop table returned 0 items.");
            }
        }
        else
        {
            
            if (showDebugLogs)
                Debug.Log($"⚠️ [{Name}] No DropTable assigned! Spawning random item...");

            itemSpawner.SpawnRandomItem(transform.position);
        }
    }

    private void OnDestroy()
    {
        
        OnDestory -= HandleDeath;
    }

    
    private void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, player.transform.position);

        // Attack Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}