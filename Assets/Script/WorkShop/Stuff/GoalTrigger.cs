using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public UIManager uiManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.ShowClear();
            Debug.Log("CLEAR!");
        }
    }
}
