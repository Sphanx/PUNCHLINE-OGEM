using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : MonoBehaviour
{
    Animator healEffectAnimator;

    private void Start()
    {
        this.healEffectAnimator = GetComponent<Animator>();
    }
    
    public void SetHealEffect()
    {
        healEffectAnimator.SetTrigger("HealEffect");
        Debug.Log("heal");
    }
}
