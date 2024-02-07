using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public int damageAmount = 10; // Saldýrý hasar miktarý
    public float attackCooldown = 3f; // Saldýrýlar arasýndaki cooldown süresi (örnekte 3 saniye)
    public float attackRange = 2f; // Düþmanýn saldýrý yapabileceði maksimum mesafe
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
        // Cooldown süresini kontrol et ve azalt
        IsPlayerInRange();
        // Düþmanýn oyuncuya saldýrma durumu (örnek)
        PlayenemyAttackAnim();
        
    }

    void AttackPlayer()
    {
        if (IsPlayerInRange())
        {
            playerController.TakeDamage(damageAmount);
            
        }
        // Oyuncuya saldýrma kodlarý burada yazýlýr
        // Örneðin: Oyuncuya hasar verme
        Debug.Log("Enemy attacks player!");

        // Cooldown'u baþlat
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

        // Düþmanýn pozisyonunu al
        Vector2 enemyPosition = transform.position;

        // Oyuncu ile düþman arasýndaki mesafeyi kontrol et
        distanceToPlayer = Vector2.Distance(playerPosition, enemyPosition);

        // Eðer oyuncu düþmanýn saldýrý alanýndaysa true döndür
        return distanceToPlayer <= attackRange;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Oyuncuya saldýr
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
