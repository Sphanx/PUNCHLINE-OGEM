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
    public float duration = 1.0f;
    public float heightY = 3.0f;
    public AnimationCurve curve;

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
    public IEnumerator Curve(Vector3 start, Vector2 target, GameObject bombObj)
    {
        float timePassed = 0f;
        Vector2 end = target;

        while(timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = curve.Evaluate(linearT);
            
            float height = Mathf.Lerp(0f, heightY, heightT);

            // update bomb position
            bombObj.transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0f, height);

            yield return null;
        }
        bombObj.GetComponent<BombScript>().IsTimerRunning = true;

    }
    public void ThrowObject()
    {
        //assign bomb prefab to bomb object
        bombObj = Instantiate(bombPrefab, transform.position, transform.rotation);
        bombObj.GetComponent<Rigidbody2D>().gravityScale = bombThrowGravity;

        //add force
        StartCoroutine(Curve(this.transform.position, playerObj.transform.position,bombObj));

        isThrowing = true;
        throwPeriod = resetThrowPeriod;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }
    private void CallBombScript()
    {
        bombObj.GetComponent<BombScript>().Explode();
    }
}
