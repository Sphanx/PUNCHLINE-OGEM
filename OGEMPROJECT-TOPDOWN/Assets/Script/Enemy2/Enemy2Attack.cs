using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Enemy2Attack : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    public float cooldown;
    private float timer;
    public float attackRange;
    GameObject playerObj;
    void Start()
    {
         playerObj = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer > cooldown)
        {
            timer = 0;
            shoot();
        }
    }
    void shoot()
    {
        float distanceToPlayer = Vector2.Distance(this.transform.position, playerObj.transform.position);
        if (distanceToPlayer < attackRange)
        {
            Instantiate(bullet, bulletPos.position, Quaternion.identity);

        }
    }

}