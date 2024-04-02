using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ArrowController : MonoBehaviour
{
    public float timeRemaining;
    public bool timerIsRunning;
    public int damage;

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if(timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //walls and other objects
        if (other.gameObject.layer == 9)
        {
            timerIsRunning = true;
            this.gameObject.GetComponent<Transform>().transform.position = transform.position;
            this.gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;
        }
        //enemy
        if (other.gameObject.layer == 7)
        {
            try
            {
                other.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
                Destroy(gameObject);
            }
            catch(Exception e)
            {

            }
        }
        
    }

}
