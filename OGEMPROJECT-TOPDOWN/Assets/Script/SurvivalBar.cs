using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalBar
{
    private float survivalBar;
    private float fullValue;

    public float Bar
    {
        get { return survivalBar; }
        set { survivalBar = value; }
    }
    public float FullValue
    {
        get { return fullValue; } 
        set { survivalBar = value; }
    }

    public float ReduceBar(float amount)
    {
        return survivalBar -= amount;
    }
    public float IncreaseBar(float amount)
    {
        return survivalBar += amount;
    }
    public bool isEmpty()
    {
        return survivalBar <= 0;
    }
    public bool isFull()
    {
        return survivalBar >= fullValue;
    }
}
