using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] int damage;
    [SerializeField] bool isAttacking = false;
    [SerializeField] float attackCD;

    float lastDashTime;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;


    private void Update()
    {
        AttackDirection();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time - lastDashTime > attackCD && (playerController.stamina.Bar > 0))
        {
            lastDashTime = Time.time;
            
            Debug.Log("Sald�rd�");
            //Hit functions
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("D��mana vurdu! " + enemy.name);
            }

            //decrease stamina
            playerController.DecreaseValue(playerController.stamina, 20);
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
        if (attackPoint == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    public void AttackDirection()
    {
        attackPoint.position = playerController.transform.position;
        if(playerController.movementInput.x > 0)
        {
            attackPoint.position += new Vector3(1, 0);
        }
        else if (playerController.movementInput.x < 0)
        {
            attackPoint.position += new Vector3(-1, 0);
        }
        else if (playerController.movementInput.y > 0)
        {
            attackPoint.position += new Vector3(0, 1);
        }
        else if (playerController.movementInput.y < 0)
        {
            attackPoint.position += new Vector3(0, -1);
        }

    }
}