using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContoller : MonoBehaviour
{

    public LayerMask playerLayer; // Oyuncu katmanýný belirlemek için kullanýlacak
    [SerializeField] private float algilamaMesafesi = 5f; // Oyuncuyu algýlama mesafesi
    [SerializeField] private float takipHizi = 3f; // Düþmanýn oyuncuyu takip etme hýzý

    public SurvivalBar enemyHealth;
    [SerializeField] private int healthBar;

    private bool oyuncuAlanda = false; // Oyuncu alanda mý?

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
        // Düþmanýn üzerinde bir çember oluþtur ve oyuncuyu algýla
        Collider2D oyuncuCollider = Physics2D.OverlapCircle(transform.position, algilamaMesafesi, playerLayer);

        // Eðer oyuncu algýlandýysa
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
        // Eðer oyuncu varsa, oyuncuya doðru hareket et
        GameObject oyuncu = GameObject.FindGameObjectWithTag("Player");

        if (oyuncu != null)
        {
            Vector3 yon = oyuncu.transform.position - transform.position;
            yon.Normalize();

            transform.Translate(yon * takipHizi * Time.deltaTime);
        }
    }

    // Gizli olarak çizilen çemberi görmek için
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
