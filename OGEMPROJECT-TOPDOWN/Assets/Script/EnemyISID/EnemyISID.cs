using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyISID : MonoBehaviour
{
    public int bomberDamage;
    [SerializeField] private PlayerController playerControllerScript;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            playerControllerScript.TakeDamage(bomberDamage);
            Destroy(gameObject);
        }
    }

    

}
