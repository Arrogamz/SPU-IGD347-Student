using UnityEngine;
using TMPro;
public class LootChest : Identity, IInteractable
{
    [Header("Loot Settings")]
    public DropTable dropTable;
    public int minItems = 1;
    public int maxItems = 3;

    [Header("UI")]
    public TMP_Text interactionText;

    private bool isOpened = false;
    public bool isInteractable
    {
        get => !isOpened; set { }

    }
    void Start()
    {
        if (interactionText != null)
            interactionText.text = " E ";
    }

    void Update()
    {
        if (interactionText == null) return;

        if (GetDistanPlayer() >= 2f || isOpened)
            interactionText.gameObject.SetActive(false);
        else
            interactionText.gameObject.SetActive(true);
    }

    public void Interact(Player player)
    {
        if (!isOpened)
            OpenBox();
    }

    private void OpenBox()
    {
        isOpened = true;

        if (interactionText != null)
            interactionText.gameObject.SetActive(false);

        SpawnItems();
    }

    private void SpawnItems()
    {
        if (dropTable == null)
        {
            Debug.LogError("No DropTable ");
            return;
        }

        var droppedItems = dropTable.GetDrops();
        if (droppedItems.Count == 0)
        {
            Debug.Log("DropTable Didn't drop");
            return;
        }

        for (int i = 0; i < droppedItems.Count; i++)
        {
            Item itemPrefab = droppedItems[i];

            float angle = (360f / droppedItems.Count) * i;
            float radius = 1.2f;

            Vector3 pos = transform.position +
                new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                            0.3f,
                            Mathf.Sin(angle * Mathf.Deg2Rad) * radius);

            Instantiate(itemPrefab, pos, Quaternion.identity);
        }
    }
}
    
