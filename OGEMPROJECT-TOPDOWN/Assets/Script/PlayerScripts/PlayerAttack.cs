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

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;


    private void Update()
    {
        
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking && (playerController.stamina.Bar > 0))
        {
            Debug.Log("Saldýrdý");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Düþmana vurdu! " + enemy.name);
            }

            playerController.DecreaseValue(playerController.stamina, playerController.reduceStmOnDash);
            Debug.Log("stamina: " + playerController.stamina.Bar);

            playerController.StartStaminaRecovery();

            isAttacking = true;
            playerController.StartCoroutine(AttackAnimation());
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
}
