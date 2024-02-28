using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PotionCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI potionsCount;
    [SerializeField] Sprite emptyPotionSprite;
    [SerializeField] Sprite filledPotionSprite;
    [SerializeField] Image potionImage;
    private void Start()
    {
       

    }
    public void setPotionTextNumber(Potions potionsScript)
    {
        potionsCount.text = potionsScript.numberOfPotions.ToString();
        UpdatePotionImage(potionsScript.numberOfPotions);
    }
    void UpdatePotionImage(int potionNumber)
    {
        if (potionNumber <= 0)
        {
            potionImage.sprite = emptyPotionSprite;
        }
        else
        {
            potionImage.sprite = filledPotionSprite;
        }
    }
}
