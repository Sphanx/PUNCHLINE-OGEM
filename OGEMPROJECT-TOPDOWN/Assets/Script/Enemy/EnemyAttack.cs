using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public int damageAmount = 10; // Sald�r� hasar miktar�
    public float attackCooldown = 3f; // Sald�r�lar aras�ndaki cooldown s�resi (�rnekte 3 saniye)
    public float attackRange = 2f; // D��man�n sald�r� yapabilece�i maksimum mesafe

    private float cooldownTimer = 0f;

    void Update()
    {
        // Cooldown s�resini kontrol et ve azalt
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // D��man�n oyuncuya sald�rma durumu (�rnek)
        if (cooldownTimer <= 0 && IsPlayerInRange())
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        // Oyuncuya sald�rma kodlar� burada yaz�l�r
        // �rne�in: Oyuncuya hasar verme
        Debug.Log("Enemy attacks player!");

        // Cooldown'u ba�lat
        cooldownTimer = attackCooldown;
    }

    bool IsPlayerInRange()
    {
        // Oyuncunun pozisyonunu al
        Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        // D��man�n pozisyonunu al
        Vector2 enemyPosition = transform.position;

        // Oyuncu ile d��man aras�ndaki mesafeyi kontrol et
        float distanceToPlayer = Vector2.Distance(playerPosition, enemyPosition);

        // E�er oyuncu d��man�n sald�r� alan�ndaysa true d�nd�r
        return distanceToPlayer <= attackRange;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && cooldownTimer <= 0)
        {
            // Oyuncuya sald�r
            AttackPlayer();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
