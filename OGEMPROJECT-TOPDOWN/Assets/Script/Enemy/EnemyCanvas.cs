using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvas : MonoBehaviour
{
    [SerializeField] GameObject enemyObj;
    [SerializeField] Vector3 healthBarPosition;
    private void Start()
    {
        
    }
    void Update()
    {
        transform.position = enemyObj.transform.position + healthBarPosition;
    }
}
