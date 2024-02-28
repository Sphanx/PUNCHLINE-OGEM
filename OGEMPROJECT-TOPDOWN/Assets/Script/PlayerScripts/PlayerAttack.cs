using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerAttack : MonoBehaviour
{   
    [SerializeField] PlayerController playerController;
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform aimPoint;
    [SerializeField] GameObject swordObj;
    
    [Space(20)]
    [SerializeField] int damage;
    [SerializeField] float attackCD;
    public float attackDistance;
    [SerializeField] float knockbackForce;
    [SerializeField] float enemyStun;
    [SerializeField] float reduceStaminaOnAttack;
    [Space(20)]
    [SerializeField] bool isHit;
    public bool isAttacking = false;
    Vector2 knockbackDirection;

    float enemyStunPlaceHolder;

    EnemyController currentEnemy;
    Animator playerAnimator;
    public float attackSlowTime = 1f;
    private float attackSlowPlaceHolder;
    public float slowOnAttack;


    float lastDashTime;

    public float attackRange = 0.5f;
    public LayerMask enemyLayers;


    private void Start()
    {
        enemyStunPlaceHolder = enemyStun;
        attackSlowPlaceHolder = attackSlowTime;
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        setEnemyStun();

        attackSlowTime -= Time.deltaTime;
        if (isAttacking)
        {
            if (attackSlowTime <= 0)
            {
                isAttacking = false;
                attackSlowTime = attackSlowPlaceHolder;
            }

        }
        else
        {
            attackSlowTime = attackSlowPlaceHolder;
            ResetAttackPoint();
            swordObj.GetComponent<SpriteRenderer>().enabled = false;
            swordObj.GetComponent<Animator>().enabled = false;
        }
       

    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time - lastDashTime > attackCD && (playerController.stamina.Bar > 0))
        {
            lastDashTime = Time.time;
            isAttacking = true;
            //shake Camera
            CameraShake.Instance.ShakeCamera(5f, 0.1f);
            //set attack point
            SetAttackPoint();
            SetAttackPointRotation();

            Debug.Log("Sald�rd�");
            //Hit functions
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                isHit = true;
                Debug.Log("D��mana vurdu! " + enemy.name);
                currentEnemy = enemy.GetComponent<EnemyController>();
                AttackOutcome(enemy, currentEnemy);
                
            }

            //decrease stamina
            playerController.DecreaseValue(playerController.stamina, reduceStaminaOnAttack);
            Debug.Log("stamina: " + playerController.stamina.Bar);

            playerController.StopStaminaRecovery();
            playerController.currentStaminaRecovery = playerController.StaminaRecovery();
            playerController.StartStaminaRecovery();

            //set animation
            playerAnimator.SetTrigger("Attack1");
            swordObj.GetComponent<SpriteRenderer>().enabled = true;
            swordObj.GetComponent<Animator>().enabled = true;
        }
    }
    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(attackCD);
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }    
    
    public void AttackOutcome(Collider2D enemy, EnemyController enemyScript)
    {
        enemyScript.TakeDamage(damage);
        knockbackDirection = (enemy.transform.position - transform.position).normalized;
        enemy.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
    public void setEnemyStun()
    {
        enemyStun -= Time.deltaTime;
        if (isHit && currentEnemy)
        {
            if (enemyStun < 0.0f)
            {
                currentEnemy.isDetecting = true;
                currentEnemy.checkPlayer = true;
                isHit = false;
                currentEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                enemyStun = enemyStunPlaceHolder;
            }
           else
            {
                currentEnemy.isDetecting = false;
                currentEnemy.checkPlayer = false;
            } 
        }
        else
        {
            enemyStun = enemyStunPlaceHolder;
        }
    }
    public void SetAttackPoint()
    {
        Vector2 aimDir = (aimPoint.position - this.transform.position).normalized;
        attackPoint.position = (new Vector3(aimDir.x/2, aimDir.y/2) + transform.position);
    }
    public void ResetAttackPoint()
    {
        attackPoint.position = transform.position;
    }
    public void SetAttackPointRotation()
    {
        Vector2 swordDirToPlayer = transform.position;

        if(attackPoint.transform.position.x < swordDirToPlayer.x)
        {
            swordObj.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            swordObj.GetComponent<SpriteRenderer>().flipY = false;
        }
        Vector3 direction = aimPoint.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        swordObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}
