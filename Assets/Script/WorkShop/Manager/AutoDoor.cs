using UnityEngine;
using System.Collections;
public class AutoDoor : Stuff, IInteractable
{
    public AutoDoor()
    {
        Name = "Door";
    }
    [Header("Door Settings")]
    public Transform door;
    public Vector3 openOffset = new Vector3(0, 0, 2f);
    public float slideSpeed = 2f;

    [Header("Auto Open Settings")]
    public bool canManualOpen = true;
    public bool isLockedUntilClear = true;


    private bool isOpen = false;

    public bool isInteractable
    {
        get => isLock;
        set => isLock = value;
    }

    
    public void Interact(Player player)
    {
        
        if (isLockedUntilClear)
        {
            Debug.Log("Door lock");

            return;
        }

        

        ToggleDoor();
    }

    
    public void OpenDoorAutomatically()
    {
        if (isOpen) return;

        isLockedUntilClear = false;
        isOpen = true;

        
        StopAllCoroutines();
        StartCoroutine(SlideDoor(door.position + openOffset, true));
    }

    
    private void ToggleDoor()
    {
        StopAllCoroutines();

        if (isOpen)
        {
            StartCoroutine(SlideDoor(door.position - openOffset, false));
        }
        else
        {
            StartCoroutine(SlideDoor(door.position + openOffset, true));
        }

        isOpen = !isOpen;
    }

    
    private IEnumerator SlideDoor(Vector3 targetPosition, bool opening)
    {
        Vector3 startPosition = door.position;
        float timeElapsed = 0;

       

        while (timeElapsed < 1)
        {
            timeElapsed += Time.deltaTime * slideSpeed;
            door.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed);
            yield return null;
        }

        door.position = targetPosition;
    }
}

