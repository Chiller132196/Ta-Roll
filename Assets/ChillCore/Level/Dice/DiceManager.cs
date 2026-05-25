using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public List<Dice> dices;

    /// <summary>
    /// 当自己的骰子被选中后，触发
    /// </summary>
    public void DiceMarge()
    {
        string summonChessID = "";

        string mainElement = "";

        string viceElement = "";

        int activedDiceNum = 0;

        foreach (Dice dice in dices)
        {
            if (dice.isSelected)
            {
                activedDiceNum++;
            }
            else
            {
                continue;
            }

            // 骰子为数字时，确认其数字
            if (dice.diceType == DiceType.number)
            {
                // 如果已经选取过数字骰，则点数相加
                if (viceElement != "" && int.TryParse(viceElement, out int result))
                {
                    viceElement = "" + Mathf.Min(10, result + int.Parse(dice.nowElement));
                }

                else
                {
                    viceElement = dice.nowElement;
                }
            }

            // 骰子为元素时，有多重判定模式
            else
            {
                if (mainElement == "")
                {
                    mainElement = dice.nowElement;
                }
                else if (mainElement == dice.nowElement)
                {
                    if (mainElement == "cup")
                    {
                        mainElement = "";
                        viceElement = "queen";
                    }
                    else if (mainElement == "sword")
                    {
                        mainElement = "";
                        viceElement = "knight";
                    }
                    else if (mainElement == "coin")
                    {
                        mainElement = "";
                        viceElement = "king";
                    }
                    else if (mainElement == "wand")
                    {
                        mainElement = "";
                        viceElement = "page";
                    }
                }
            }
        }

        // 遍历后，若任意参数不存在，以及骰子数超过时，取消生成
        if (mainElement == "" || viceElement == "" || activedDiceNum >= 4)
        {
            Debug.Log(mainElement + "和" + viceElement + "组合的ID非法或超过了" + activedDiceNum +"个骰子被激活");
            return;
        }

        summonChessID = mainElement + viceElement;

        Debug.Log("尝试生成 "+summonChessID);

        if (ChessKeeper.chessKeeper.SummonPlayerChess(summonChessID))
        {
            foreach(var dice in dices)
            {
                if (dice.isSelected)
                    dice.GetComponent<Dice>().UseTheDice();
            }
        }
    }
}
