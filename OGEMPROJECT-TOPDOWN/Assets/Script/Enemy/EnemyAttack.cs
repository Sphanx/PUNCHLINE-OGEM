using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public int damageAmount = 10; // Sald�r� hasar miktar�
    public float attackCooldown = 3f; // Sald�r�lar aras�ndaki cooldown s�resi (�rnekte 3 saniye)
    public float attackRange = 2f; // D��man�n sald�r� yapabilece�i maksimum mesafe
    public PlayerController playerController;
    public float attackDelay;
    public float enemyAttackAnimRange;
    private float cooldownTimer = 0f;
    public Vector2 playerPosition;
    float distanceToPlayer;

    Animator enemyAnimator;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        // Cooldown s�resini kontrol et ve azalt
        IsPlayerInRange();
        // D��man�n oyuncuya sald�rma durumu (�rnek)
        PlayenemyAttackAnim();
        
    }

    void AttackPlayer()
    {
        if (IsPlayerInRange())
        {
            playerController.TakeDamage(damageAmount);
            
        }
        // Oyuncuya sald�rma kodlar� burada yaz�l�r
        // �rne�in: Oyuncuya hasar verme
        Debug.Log("Enemy attacks player!");

        // Cooldown'u ba�lat
    }
    public void PlayenemyAttackAnim()
    {
        distanceToPlayer = Vector2.Distance(playerPosition, transform.position);
        if(distanceToPlayer <= enemyAttackAnimRange)
        {
            enemyAnimator.SetBool("isAttacking", true);
        }
        else
        {
            enemyAnimator.SetBool("isAttacking", false);
        }
    }

    bool IsPlayerInRange()
    {

        // D��man�n pozisyonunu al
        Vector2 enemyPosition = transform.position;

        // Oyuncu ile d��man aras�ndaki mesafeyi kontrol et
        distanceToPlayer = Vector2.Distance(playerPosition, enemyPosition);

        // E�er oyuncu d��man�n sald�r� alan�ndaysa true d�nd�r
        return distanceToPlayer <= attackRange;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Oyuncuya sald�r
            AttackPlayer();
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, enemyAttackAnimRange);
    }

}
