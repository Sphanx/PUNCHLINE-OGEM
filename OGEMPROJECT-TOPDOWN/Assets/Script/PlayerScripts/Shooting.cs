using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField] Rigidbody2D arrowPrefab;
    [SerializeField] float speed;

    private void FixedUpdate()
    {
    }
    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            arrowPrefab.AddForce(speed * transform.forward, ForceMode2D.Impulse);
        }
    }
}
