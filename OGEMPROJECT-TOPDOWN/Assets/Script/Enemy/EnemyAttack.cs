using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public int damageAmount = 10; // Sald�r� hasar miktar�
    public float attackRange = 2f; // D��man�n sald�r� yapabilece�i maksimum mesafe
    public float attackSpeed;
    public PlayerController playerController;
    public float enemyAttackAnimRange;
    [SerializeField] Transform playerPosition;
    float distanceToPlayer;

    public float attackAnimCooldown = 3f; // Sald�r�lar aras�ndaki cooldown s�resi (�rnekte 3 saniye)
    public float attackCooldown;

    [Space(10)]
    [Header("Read Only")]
    [SerializeField] float timeRemaining;
    [SerializeField] float timeRemainingAttack;
    [SerializeField] bool isAttacking;

    Animator enemyAnimator;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        timeRemaining = attackAnimCooldown;
        timeRemainingAttack = 0;
        
    }
    void Update()
    {
        //get distance to player
        distanceToPlayer = Vector2.Distance(playerPosition.position, transform.position);


        // play attack animation
        if(timeRemainingAttack > 0)
        {
            timeRemainingAttack -= Time.deltaTime;
        }
        if (IsPlayerInRange(distanceToPlayer))
        {
            if (timeRemainingAttack <= 0)
            {
                isAttacking = true;
                AttackPlayer();
            }

        }
        

         if(timeRemaining > 0)
         {
            timeRemaining -= Time.deltaTime;
         }
        if(timeRemaining <= 0)
        {
            PlayenemyAttackAnim(distanceToPlayer);
        }


    }

    void AttackPlayer()
    {
        if (isAttacking)
        {
            
            playerController.TakeDamage(damageAmount);
            Vector2 knockbackDirection = (playerController.GetComponent<Transform>().position - this.transform.position).normalized;
            playerController.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * Time.fixedDeltaTime, ForceMode2D.Impulse);
            Debug.Log("Enemy attacks player!");
            isAttacking = false;
            timeRemainingAttack = attackCooldown;
        }
        
    }
    public void PlayenemyAttackAnim(float distanceBetween)
    {
        if(distanceBetween <= enemyAttackAnimRange)
        {
            enemyAnimator.SetBool("isAttacking", true);
            Vector2 dir = (playerController.GetComponent<Transform>().position - transform.position).normalized;
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(10 * attackSpeed * dir * Time.fixedDeltaTime, ForceMode2D.Impulse);
            timeRemaining = attackAnimCooldown;
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
