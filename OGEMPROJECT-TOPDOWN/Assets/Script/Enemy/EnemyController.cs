using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
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
    public float knockbackForce;
    public float stopRange;

    private Animator enemyAnimator;

    private bool isFacingRight = false;
    void Start()
    {
        enemyHealth = new SurvivalBar();
        enemyHealth.FullValue = health;
        enemyHealthSlider.maxValue = health;
        enemyHealthSlider.value = health;
        enemyAnimator = GetComponent<Animator>();

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
                if(stopRange >= (Vector2.Distance(playerObj.transform.position, this.transform.position)))
                {
                    transform.position = this.transform.position;
                }
                else
                { 
                    FollowPlayer();

                }
                Debug.Log("Oyuncu Görxüldü");

            }
        }
        CheckFlipDirection();
    }

    public void TakeDamage(int amount)
    {
        //play anim
        enemyAnimator.SetTrigger("isHurt");
        SoundManager.PlaySound(SoundManager.Sound.EnemyHit);
        //math stuff
        enemyHealth.DecreaseBar(amount);
        enemyHealthSlider.value = enemyHealth.Bar;
        Vector3 knockbackDirection = (this.transform.position - playerObj.transform.position).normalized;
        this.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
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

    private void CheckFlipDirection()
    {
        // Düþmanýn x eksenindeki hýzýný kontrol et
        Vector2 horizontalSpeed = (playerObj.transform.position - transform.position).normalized;

        // Eðer hýz pozitifse düþman saða doðru hareket ediyor demektir
        // Scale'ý (-1, 1, 1) yaparak yatayda tersine çevir
        if (horizontalSpeed.x > 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
            isFacingRight = true;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
            isFacingRight = false;
        }

    }


}

