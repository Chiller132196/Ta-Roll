using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDice : Dice
{
    public GameObject highLightSlide;

    private void Start()
    {
        RollTheDice();
    }

    void Update()
    {
        if (isSelected)
        {
            highLightSlide.SetActive(true);
        }
        else
        {
            highLightSlide.SetActive(false);
        }
    }
}
