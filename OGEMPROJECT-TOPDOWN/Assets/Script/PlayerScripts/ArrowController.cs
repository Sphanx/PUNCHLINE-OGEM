using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 || other.gameObject.layer == 7)
        {
            Destroy(gameObject);
        }
        if(other.gameObject.layer == 7)
        {
            Destroy(gameObject);
        }
    }
}
