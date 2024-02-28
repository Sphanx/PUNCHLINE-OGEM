using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    private void Update()
    {
        this.transform.position = playerPos.position;
    }
}
