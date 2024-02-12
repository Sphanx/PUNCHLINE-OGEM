using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    SurvivalBar enemyHealth;
    [SerializeField] int health;
    [SerializeField] Slider enemyHealthSlider;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] int enemyfovRadius;
    public bool checkPlayer;
    public bool isDetecting = true;
    GameObject playerObj;
    [SerializeField] float enemySpeed;
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
        if (isDetecting)
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
                Debug.Log("Oyuncu Görüldü");

            }
        }
    }

    public void TakeDamage(int amount)
    {
        enemyHealth.DecreaseBar(amount);
        enemyHealthSlider.value = enemyHealth.Bar;
        if (enemyHealth.Bar <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DetectPlayer()
    {
        Collider2D OverlapPlayer = Physics2D.OverlapCircle(transform.position, enemyfovRadius, playerLayer);

        if (OverlapPlayer != null)
        {
            checkPlayer = true;
        }
        else
        {
            checkPlayer = false;
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

    public void A()
    {

    }
}

