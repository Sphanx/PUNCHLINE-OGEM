using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public int damageAmount = 10; // Saldýrý hasar miktarý
    public float attackCooldown = 3f; // Saldýrýlar arasýndaki cooldown süresi (örnekte 3 saniye)
    public float attackRange = 2f; // Düþmanýn saldýrý yapabileceði maksimum mesafe

    private float cooldownTimer = 0f;

    void Update()
    {
        // Cooldown süresini kontrol et ve azalt
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Düþmanýn oyuncuya saldýrma durumu (örnek)
        if (cooldownTimer <= 0 && IsPlayerInRange())
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        // Oyuncuya saldýrma kodlarý burada yazýlýr
        // Örneðin: Oyuncuya hasar verme
        Debug.Log("Enemy attacks player!");

        // Cooldown'u baþlat
        cooldownTimer = attackCooldown;
    }

    bool IsPlayerInRange()
    {
        // Oyuncunun pozisyonunu al
        Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        // Düþmanýn pozisyonunu al
        Vector2 enemyPosition = transform.position;

        // Oyuncu ile düþman arasýndaki mesafeyi kontrol et
        float distanceToPlayer = Vector2.Distance(playerPosition, enemyPosition);

        // Eðer oyuncu düþmanýn saldýrý alanýndaysa true döndür
        return distanceToPlayer <= attackRange;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && cooldownTimer <= 0)
        {
            // Oyuncuya saldýr
            AttackPlayer();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
