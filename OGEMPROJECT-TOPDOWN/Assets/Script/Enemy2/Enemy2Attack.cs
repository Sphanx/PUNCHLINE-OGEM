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
    void Start()
    {

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
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }

}