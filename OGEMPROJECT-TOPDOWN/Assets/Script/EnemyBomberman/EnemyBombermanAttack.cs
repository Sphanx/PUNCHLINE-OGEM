using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBombermanAttack : MonoBehaviour
{
    [SerializeField] GameObject bombPrefab;
    public float bombThrowForce;
    public float bombThrowGravity;
    public float verticalForce;
    public float throwPeriod;
    public bool isThrowing = false;
    public float attackRange;

    private GameObject bombObj;
    private GameObject playerObj;
    private float resetThrowPeriod;

    private void Start()
    {
        playerObj = GameObject.Find("Player");
        resetThrowPeriod = throwPeriod;
        
    }
    private void Update()
    {
        if (Vector2.Distance(playerObj.transform.position, this.transform.position) < attackRange)
        {
            isThrowing = true;
            if (isThrowing)
            {
                if(throwPeriod > 0)
                {
                    throwPeriod -= Time.deltaTime;
                }
                else
                {
                    ThrowObject();
                    isThrowing = false;
                }
            }
        }
    }
    public void ThrowObject()
    {
        //assign bomb prefab to bomb object
        bombObj = Instantiate(bombPrefab, transform.position, transform.rotation);
        bombObj.GetComponent<Rigidbody2D>().gravityScale = bombThrowGravity;

        Vector2 throwDirection = (playerObj.transform.position - this.transform.position).normalized;
        //add force
        bombObj.GetComponent<Rigidbody2D>().velocity = bombThrowForce * throwDirection * Time.fixedDeltaTime * new Vector2(1, verticalForce);
        isThrowing = true;
        throwPeriod = resetThrowPeriod;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }

}
