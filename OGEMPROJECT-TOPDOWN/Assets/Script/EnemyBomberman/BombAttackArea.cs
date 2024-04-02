using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAttackArea : MonoBehaviour
{
    [SerializeField] private Transform areaTransform;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetBombArea(float explosionRadius)
    {
        areaTransform.localScale = new Vector3(explosionRadius * 2, explosionRadius * 2);
    }
}
