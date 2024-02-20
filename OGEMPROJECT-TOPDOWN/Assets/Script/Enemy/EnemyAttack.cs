using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public int damageAmount = 10; // Sald�r� hasar miktar�
    public float attackRange = 2f; // D��man�n sald�r� yapabilece�i maksimum mesafe
    public PlayerController playerController;
    public float enemyAttackAnimRange;
    [SerializeField] Transform playerPosition;
    float distanceToPlayer;

    public float attackCooldown = 3f; // Sald�r�lar aras�ndaki cooldown s�resi (�rnekte 3 saniye)
    public float timeRemaining;
    [SerializeField] bool isAttacking;

    Animator enemyAnimator;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        timeRemaining = attackCooldown;
    }
    void Update()
    {
        distanceToPlayer = Vector2.Distance(playerPosition.position, transform.position);

        // Cooldown s�resini kontrol et ve azalt
       
        // D��man�n oyuncuya sald�rma durumu (�rnek)
        PlayenemyAttackAnim(distanceToPlayer);


         if(timeRemaining > 0)
         {
            timeRemaining -= Time.deltaTime;
         }
        if (IsPlayerInRange(distanceToPlayer))
        {
            if (timeRemaining <= 0)
            {
                isAttacking = true;
                AttackPlayer();
            }
        }


    }

    void AttackPlayer()
    {
        if (isAttacking)
        {
            playerController.TakeDamage(damageAmount);
            Debug.Log("Enemy attacks player!");
            timeRemaining = attackCooldown;
            isAttacking = false;
        }
        
    }
    public void PlayenemyAttackAnim(float distanceBetween)
    {
        if(distanceBetween <= enemyAttackAnimRange)
        {
            enemyAnimator.SetBool("isAttacking", true);
        }
        else
        {
            enemyAnimator.SetBool("isAttacking", false);
        }
    }

    bool IsPlayerInRange(float distanceToPlayer)
    {
        if(distanceToPlayer <= attackRange)
        {
            return true;
        }
        return false;
    }

  
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, enemyAttackAnimRange);
    }

}
