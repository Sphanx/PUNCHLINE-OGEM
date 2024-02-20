using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField] Rigidbody2D arrowPrefab;
    [SerializeField] float speed;

    public Transform aimObj;
    Vector2 shootDir;

    private void FixedUpdate()
    {       
            shootDir = (aimObj.position - transform.position).normalized;
            arrowPrefab.AddForce(speed * shootDir, ForceMode2D.Impulse);
    }
}
