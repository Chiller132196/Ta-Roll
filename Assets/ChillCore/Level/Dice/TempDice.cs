using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TempDice : Dice, IPointerDownHandler
{
    public GameObject highLightSlide;

    public TMP_Text diceText;

    public Image diceUpperSide;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RollTheDice();
        }
    }

    public override void CheckUpperSide()
    {
        if (nowElement == "cup")
        {
            diceUpperSide.color = new Color(0f / 255f, 100f / 255f, 255f / 255f);
            diceText.text = "Water";
        }

        else if (nowElement == "wand")
        {
            diceUpperSide.color = new Color(255f / 255f, 0f / 255f, 0f / 255f);
            diceText.text = "Fire";
        }

        else if (nowElement == "coin")
        {
            diceUpperSide.color = new Color(255f / 255f, 157f / 255f, 0f / 255f);
            diceText.text = "Soil";
        }

        else if (nowElement == "sword")
        {
            diceUpperSide.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
            diceText.text = "Wind";
        }

        else
        {
            diceText.text = nowElement;
        }
    }

    public override void UseTheDice()
    {
        base.UseTheDice();

        diceUpperSide.color = new Color(0f, 0f, 0f);

        highLightSlide.SetActive(false);
    }

    private void Start()
    {
        isUesd = false;

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
