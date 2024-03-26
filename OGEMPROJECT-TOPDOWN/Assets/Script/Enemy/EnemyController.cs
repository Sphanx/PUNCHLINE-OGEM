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
    public float runAwayRange;

    private Animator enemyAnimator;

    private bool isFacingRight = false;
    
    
    public enum EnemyType
    {
        bomberman,
        slime,
        turret
    }
    public EnemyType enemyType;


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
            //Checks player in an area
            DetectPlayer();

            if (checkPlayer == true)
            {  
                //stop if at a certain distance from the player
                if(stopRange >= (Vector2.Distance(playerObj.transform.position, this.transform.position)))
                {
                    transform.position = this.transform.position;
                }
                else
                { 
                    FollowPlayer();
                }

                //run away function if this is bomberman enemy type
                if(enemyType == EnemyType.bomberman)
                {
                   if(runAwayRange >= (Vector2.Distance(playerObj.transform.position, this.transform.position)))
                   {
                        RunAway();
                   }
                }
                Debug.Log("Oyuncu Görüldü");
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
        
        //do these if this is attached to turret
        if(enemyType == EnemyType.turret && enemyHealth.Bar <= 0)
        {
            Destroy(this.gameObject.GetComponent<Enemy2Attack>());
            Destroy(this);
        }
        else if(enemyHealth.Bar <= 0 )
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
    public void RunAway()
    {
        Vector2 direction = (transform.position - playerObj.transform.position).normalized;
        transform.Translate(direction * enemySpeed * Time.deltaTime);
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

