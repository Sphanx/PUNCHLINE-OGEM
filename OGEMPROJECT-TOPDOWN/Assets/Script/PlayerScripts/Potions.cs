using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potions : MonoBehaviour
{
    public int healAmount;
    [System.NonSerialized] public int numberOfPotions;

    public Potions(int healAmount, int numberOfPotions)
    {
        this.healAmount = healAmount;
        this.numberOfPotions = numberOfPotions;
    }
}
