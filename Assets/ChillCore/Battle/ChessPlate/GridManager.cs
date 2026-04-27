using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public List<ChessGrid> chessGrids;

    public static GridManager gridManager { get; private set; }

    void Awake()
    {
        if (gridManager != null && gridManager != this)
        {
            Destroy(gameObject);
            return;
        }

        gridManager = this;
    }

    // 根据XY找格子
    public ChessGrid GetGridByXY(int _x, int _y, bool _needPlayerSide)
    {
        foreach (var grid in chessGrids)
        {
            if (grid.posX == _x && grid.posY == _y && grid.isPlayerSide == _needPlayerSide)
                return grid;
        }

        Debug.LogError("---没有符合条件的格子---");

        return null;
    }

    public GameObject GetGridChessByXY(int _x, int _y, bool _needPlayerSide)
    {
        foreach (var grid in chessGrids)
        {
            if (grid.posX == _x && grid.posY == _y && grid.isPlayerSide == _needPlayerSide)
            {
                if (grid.hasChess())
                {
                    return grid.chess;
                }
            }
        }

        Debug.Log("未能找到符合技能释放条件的棋子");

        return null;
    }
}
