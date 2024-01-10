using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AllahinCezasiBar : MonoBehaviour
{
    SurvivalBar enemyHealth;
    [SerializeField] int health;
    [SerializeField] Slider enemyHealthSlider;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] int enemyfovRadius;
    [SerializeField] bool checkPlayer;
    [SerializeField] GameObject playerObj;
    [SerializeField] int enemySpeed;
    void Start()
    {
        enemyHealth = new SurvivalBar();
        enemyHealth.FullValue = health;
        enemyHealthSlider.maxValue = health;
        enemyHealthSlider.value = health;

        playerObj = GameObject.Find("Player");
    }

    
    void Update()
    {
        DetectPlayer();
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(50);
            Debug.Log(enemyHealth.Bar);
        }
        if (checkPlayer == true)
        {
            FollowPlayer();
            Debug.Log("Oyuncu G�r�ld�");
        }
    }

    public void TakeDamage(int amount)
    {
        enemyHealth.ReduceBar(amount);
        enemyHealthSlider.value = enemyHealth.Bar;
        if (enemyHealth.Bar <= 0 )
        {
            Destroy(gameObject);
        }
    }

    public void DetectPlayer()
    {
        Collider2D OverlapPlayer = Physics2D.OverlapCircle(transform.position, enemyfovRadius, playerLayer);

        if (OverlapPlayer != null )
        {
            checkPlayer = true;
        }
        else 
        {
            checkPlayer= false;
        }
        
    }

    public void FollowPlayer()
    {
        Vector2 direction = (playerObj.transform.position - transform.position).normalized;
        transform.Translate(direction * enemySpeed * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyfovRadius);
    }
}

