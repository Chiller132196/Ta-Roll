using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGrid : MonoBehaviour
{
    /// <summary>
    /// 格子上的棋子
    /// </summary>
    public GameObject chess;

    /// <summary>
    /// 占位用的棋子，在无法返回棋子时调用
    /// </summary>
    public GameObject emptyChess;

    public bool isPlayerSide;

    public int posX;

    public int posY;

    public GameObject GetChess()
    {
        if (!chess)
        {
            return emptyChess;
        }
        else if (!chess.GetComponent<Entity>().isAlive)
        {
            return emptyChess;
        }

        return chess;
    }

    /// <summary>
    /// 查询是否有棋子在此格子上
    /// </summary>
    /// <returns>是/否</returns>
    public bool hasChess()
    {
        if (!chess)
        {
            return false;
        }

        if (!chess.GetComponent<Entity>().isAlive)
        {
            return false;
        }

        return true;
    }

}
