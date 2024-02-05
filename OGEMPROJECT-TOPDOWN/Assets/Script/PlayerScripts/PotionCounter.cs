using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PotionCounter : MonoBehaviour
{
    TextMeshProUGUI potionsCount;
    private void Start()
    {
        TextMeshProUGUI potionsCount = this.GetComponent<TextMeshProUGUI>();

    }
    public void setPotionTextNumber(Potions potionsScript)
    {
        potionsCount.text = potionsScript.numberOfPotions.ToString();
    }
}
