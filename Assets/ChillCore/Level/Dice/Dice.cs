using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceType
{
    number,
    element
}

public class Dice : MonoBehaviour
{
    public DiceType diceType; 

    /// <summary>
    /// 骰子的所有面
    /// </summary>
    public List<string> diceElement;

    /// <summary>
    /// 骰子现在朝上的面
    /// </summary>
    public string nowElement;

    public bool isUesd;

    public bool isSelected;

    public void RollTheDice()
    {
        if (diceElement.Count <= 0)
        {
            Debug.Log("骰子的面熟为0！检查是否有误！");

            return;
        }

        int sideNum = Random.Range(0, diceElement.Count - 1);

        Debug.Log("摇到了" + diceElement[sideNum]);

        nowElement = diceElement[sideNum];

        CheckUpperSide();
    }

    public virtual void CheckUpperSide()
    {

    }

    public void SelectTheDice()
    {
        if (!isUesd)
            isSelected = !isSelected;
    }

    public virtual void UseTheDice()
    {
        isSelected = false;
        isUesd = true;
    }
}
