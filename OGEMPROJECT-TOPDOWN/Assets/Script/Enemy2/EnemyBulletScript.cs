using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float force;
    private float timer;
    public float removeBulletTime;
    public int damage;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 directon = player.transform.position - transform.position;
        rb.velocity = new Vector2(directon.x, directon.y).normalized * force;
        float rot = Mathf.Atan2(-directon.y, directon.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0 , rot);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > removeBulletTime)
        {
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
         if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
         if(other.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }
    }
}
