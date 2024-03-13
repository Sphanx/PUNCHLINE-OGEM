using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombScript : MonoBehaviour
{
    // The radius of the explosion
    public float explosionRadius = 5f;

    // The time before the bomb explodes
    public float explosionDelay = 2f;
    public int bombDamage;
    [SerializeField] float timeRemainingToExpload;
    [SerializeField] bool isTimerRunning = false;

    public bool IsTimerRunning
    {
        set { isTimerRunning = value; }
    }

    // Start is called before the first frame update
    private void Update()
    {
        if (isTimerRunning)
        {
            if(timeRemainingToExpload > 0)
            {
                timeRemainingToExpload -= Time.deltaTime;
            }
            else
            {
                Explode();
            }
        }
    }
    // The explosion method
    public void Explode()
    {
        // Get all objects within the explosion radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        // Loop through the hit colliders
        foreach (Collider2D hitCollider in hitColliders)
        {
            // Get the game object associated with the collider
            GameObject hitObject = hitCollider.gameObject;

            // Check if the hitobject is player
            if (hitObject.layer == 6)
            {
                hitObject.GetComponent<PlayerController>().TakeDamage(bombDamage);
            }
            // Check if the hitobject is enemy
            if (hitObject.layer == 7)
            {
                hitObject.GetComponent<EnemyController>().TakeDamage(bombDamage);
            }
        }

        // Destroy the bomb script
        Destroy(this.gameObject);
    }

    // The OnDrawGizmos method for visualizing the explosion radius
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, explosionRadius);
    }
}


