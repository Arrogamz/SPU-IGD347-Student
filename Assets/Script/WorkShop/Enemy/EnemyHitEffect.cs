using TMPro;
using UnityEngine;

public class EnemyHitEffect : MonoBehaviour
{
    [Header("Hit Effects")]
    public GameObject hitEffectPrefab;          
    public AudioClip hitSound;                  
   

    public void ShowHitEffect(int damage, Vector3 hitPosition)
    {
        // Particle Effect
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, hitPosition, Quaternion.identity);
            Destroy(effect, 1.5f);
        }

        
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, hitPosition, 0.7f);
        }

        
    }
}

