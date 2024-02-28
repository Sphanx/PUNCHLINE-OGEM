using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackObjs : MonoBehaviour
{
    [SerializeField] Transform playerObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerObj.position;
    }
}
