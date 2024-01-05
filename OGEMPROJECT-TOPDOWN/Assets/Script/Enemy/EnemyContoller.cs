using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContoller : MonoBehaviour
{

    public LayerMask playerLayer; // Oyuncu katman�n� belirlemek i�in kullan�lacak
    [SerializeField] private float algilamaMesafesi = 5f; // Oyuncuyu alg�lama mesafesi
    [SerializeField] private float takipHizi = 3f; // D��man�n oyuncuyu takip etme h�z�

    public SurvivalBar enemyHealth;
    [SerializeField] private int healthBar;

    private bool oyuncuAlanda = false; // Oyuncu alanda m�?

    private void Start()
    {
        enemyHealth = new SurvivalBar();
        enemyHealth.FullValue = healthBar;
    }

    void Update()
    {
        AlgilaOyuncu();

        if (oyuncuAlanda)
        {
            TakipEtOyuncu();
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(50);
        }
    }

    void AlgilaOyuncu()
    {
        // D��man�n �zerinde bir �ember olu�tur ve oyuncuyu alg�la
        Collider2D oyuncuCollider = Physics2D.OverlapCircle(transform.position, algilamaMesafesi, playerLayer);

        // E�er oyuncu alg�land�ysa
        if (oyuncuCollider != null)
        {
            oyuncuAlanda = true;
        }
        else
        {
            oyuncuAlanda = false;
        }
    }

    void TakipEtOyuncu()
    {
        // E�er oyuncu varsa, oyuncuya do�ru hareket et
        GameObject oyuncu = GameObject.FindGameObjectWithTag("Player");

        if (oyuncu != null)
        {
            Vector3 yon = oyuncu.transform.position - transform.position;
            yon.Normalize();

            transform.Translate(yon * takipHizi * Time.deltaTime);
        }
    }

    // Gizli olarak �izilen �emberi g�rmek i�in
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, algilamaMesafesi);
    }


    public void TakeDamage(int damage)
    {
        enemyHealth.ReduceBar(damage);
        Debug.Log(enemyHealth.Bar);

        if (enemyHealth.Bar <= 0)
        {
            Destroy(gameObject);
        }

    }
   
}
