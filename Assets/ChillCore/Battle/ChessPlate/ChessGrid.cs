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

    /// <summary>
    /// 上方的棋盘格子
    /// </summary>
    public GameObject upGrid;

    /// <summary>
    /// 下方的棋盘格子
    /// </summary>
    public GameObject downGrid;

    /// <summary>
    /// 左侧的棋盘格子
    /// </summary>
    public GameObject leftGrid;
    
    /// <summary>
    /// 右侧的棋盘格子
    /// </summary>
    public GameObject rightGrid;

    public int girdX;

    public int gridY;

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

    public GameObject GetUpGid()
    {
        return upGrid;
    }

    public GameObject GetDownGrid()
    {
        return downGrid;
    }

    public GameObject GetLeftGrid()
    {
        return leftGrid;
    }

    public GameObject GetRightGrid()
    {
        return rightGrid;
    }

    /// <summary>
    /// 寻找四周有棋子的格子
    /// </summary>
    /// <returns></returns>
    public GameObject GetNextGird()
    {
        if (rightGrid && rightGrid.GetComponent<ChessGrid>().hasChess())
        {
            return rightGrid;
        }

        else if (leftGrid && leftGrid.GetComponent<ChessGrid>().hasChess())
        {
            return leftGrid;
        }

        else if (upGrid && upGrid.GetComponent<ChessGrid>().hasChess())
        {
            return upGrid;
        }

        else
        {
            return downGrid;
        }
    }
}
