using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    Vector3 swordPosVar;
    private GameObject attackPointObj;
    public float vectorVar = 0.2f;

    private void Start()
    {
        attackPointObj = GameObject.Find("Attack Point");
    }
    private void Update()
    {
        if(attackPointObj.transform.position.y > playerPos.position.y)
        {
            swordPosVar.y = vectorVar;
        }
        if (attackPointObj.transform.position.y < playerPos.position.y)
        {
            swordPosVar.y = -vectorVar;
        }
        if (attackPointObj.transform.position.x > playerPos.position.x)
        {
            swordPosVar.x = vectorVar;
        }
        if (attackPointObj.transform.position.x < playerPos.position.x)
        {
            swordPosVar.x = -vectorVar;
        }

        this.transform.position = playerPos.position + swordPosVar;
    }
    public void DeactivateObj()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

}
