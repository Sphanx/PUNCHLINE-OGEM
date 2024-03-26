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
    private Animator animator;
    void Start()
    {
         playerObj = GameObject.FindGameObjectWithTag("Player");
         animator = this.GetComponent<Animator>();
    }
    void Update()
    {


        if (timer > cooldown)
        {
            animator.SetBool("isAttacking", true);

        }
        timer += Time.deltaTime;
        //timer ends
        if (timer > cooldown)
        {
            timer = 0;
            if (checkPlayerInRange())
            {
                Shoot();
            }
        }
    }
    bool checkPlayerInRange()
    {
        float distanceToPlayer = Vector2.Distance(this.transform.position, playerObj.transform.position);
        if (distanceToPlayer < attackRange)
        {
            return true;
        }
        return false;
    }
    void Shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
        animator.SetBool("isAttacking", false);
    }
}