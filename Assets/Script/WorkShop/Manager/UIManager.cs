using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject clearText;

    public void ShowClear()
    {
        clearText.SetActive(true);
    }

    public void HideClear()
    {
        clearText.SetActive(false);
    }
}
