using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovetoPlayer : Enemy
{
    [Header("Movement Settings")]
           
    public float attackRange = 1.5f;     

    private void Update()
    {
        if (player == null)
        {
            animator.SetBool("Attack", false);
            Move(Vector3.zero);
            return;
        }

        float distance = GetDistanPlayer();
        timer -= Time.deltaTime;

        if (distance > visionRange)
        {
            animator.SetBool("Attack", false);
            Move(Vector3.zero);  
            return;
        }

        Turn(player.transform.position - transform.position);

        
        if (distance <= attackRange)
        {
            Move(Vector3.zero); 
            Attack(player);
            return;
        }
        animator.SetBool("Attack", false);

        Vector3 direction = (player.transform.position - transform.position).normalized;
        Move(direction); 
    }
}
