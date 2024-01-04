using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponParent : MonoBehaviour
{
    public SpriteRenderer weaponRenderer;
    public Vector2 PointerPosition { get; set; }
    public Animator animator;
    public PlayerController playerController;

    private void Start()
    {
         
    }

    private void Update()
    {

        /*
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if(direction.x < 0)
        {
            scale.y = -1;
        }
        else if(direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;

        if(transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180 )
        {
            weaponRenderer.sortingOrder = 5;
        }
        else
        {
            weaponRenderer.sortingOrder = 6;
        }
        */
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && (playerController.movementInput.x < 0 || playerController.movementInput.x > 0) )
        {
            animator.SetTrigger("AttackHorizontal");
        }
        else if(context.performed && (playerController.movementInput.y < 0 || playerController.movementInput.y > 0))
        {
            animator.SetTrigger("AttackVertical");
        }
    }
}
