using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerAttack : MonoBehaviour
{   
    [SerializeField] PlayerController playerController;
    
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
            
            Debug.Log("Saldýrdý");
            //Hit functions
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(playerController.attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                isHit = true;
                Debug.Log("Düþmana vurdu! " + enemy.name);
                currentEnemy = enemy.GetComponent<EnemyController>();
                AttackOutcome(enemy, currentEnemy);
                
            }

            //decrease stamina
            playerController.DecreaseValue(playerController.stamina, reduceStaminaOnAttack);
            Debug.Log("stamina: " + playerController.stamina.Bar);

            playerController.StopStaminaRecovery();
            playerController.currentStaminaRecovery = playerController.StaminaRecovery();
            playerController.StartStaminaRecovery();

        }
    }
    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(attackCD);
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        if (playerController.attackPoint == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerController.attackPoint.position, attackRange);
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
}
