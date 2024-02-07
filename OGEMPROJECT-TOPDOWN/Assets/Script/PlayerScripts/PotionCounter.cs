using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PotionCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI potionsCount;
    private void Start()
    {
       

    }
    public void setPotionTextNumber(Potions potionsScript)
    {
        potionsCount.text = potionsScript.numberOfPotions.ToString();
    }
}
