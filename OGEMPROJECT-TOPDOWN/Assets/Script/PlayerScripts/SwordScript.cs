using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] Vector3 swordPosVar;
    private void Update()
    {
        this.transform.position = playerPos.position + swordPosVar;
    }
    public void DeactivateObj()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
