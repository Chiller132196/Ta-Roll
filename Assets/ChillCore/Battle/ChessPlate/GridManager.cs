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

    /// <summary>
    /// 寻找一个最靠前的棋子
    /// </summary>
    /// <param name="_needPlaerside">是否需要是自己这边的</param>
    /// <returns></returns>
    public Entity FindAnyFrontChess(Chesstype _needSide)
    {
        foreach (var grid in chessGrids)
        {
            if (grid.HasChess() && grid.chesstype == _needSide)
                return grid.chess.GetComponent<Entity>();
        }
         

        return null;
    }

    /// <summary>
    /// 寻找一个对面最靠前的棋子
    /// </summary>
    /// <param name="_needPlaerside">自己所在的阵容</param>
    /// <returns></returns>
    public Entity FindAnyOpponentFrontChess(Chesstype _needSide)
    {
        foreach (var grid in chessGrids)
        {
            if (grid.HasChess() && grid.chesstype != _needSide)
            {
                //Debug.Log("已将 " + grid.chess.gameObject.name + " 返回");

                return grid.chess.GetComponent<Entity>();
            }
        }

        Debug.Log("---GridManager 未能找到合适的棋子---");

        return null;
    }

    // 根据XY找格子
    public ChessGrid GetGridByXY(int _x, int _y, Chesstype _needSide)
    {
        foreach (var grid in chessGrids)
        {
            if (grid.posX == _x && grid.posY == _y && grid.chesstype == _needSide)
                return grid;
        }

        Debug.LogError("---没有符合条件的格子---");

        return null;
    }

    public GameObject GetGridChessByXY(int _x, int _y, Chesstype _needSide)
    {
        foreach (var grid in chessGrids)
        {
            if (grid.posX == _x && grid.posY == _y && grid.chesstype == _needSide)
            {
                if (grid.HasChess())
                {
                    return grid.chess;
                }
            }
        }

        Debug.Log("未能找到符合技能释放条件的棋子");

        return null;
    }

    /// <summary>
    /// 将棋子传送到特定阵营的某个坐标格
    /// </summary>
    /// <param name="_position"></param>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_needSide"></param>
    /// <returns></returns>
    public bool TeleportChess(GameObject _chess, int _x, int _y, Chesstype _needSide)
    {
        ChessPosition chessPosition = _chess.GetComponent<Entity>().chessPosition;

        foreach (var grid in chessGrids)
        {
            // 尝试找到棋子想移动到的格子
            if (grid.posX == _x && grid.posY == _y && grid.chesstype == _needSide)
            {
                if (grid.HasChess())
                {
                    return false;
                }

                else
                {
                    grid.TeleportToMe(_chess);

                    break;
                }
            }
        }

        // 完成移动后，寻找原来的格子，并告知它棋子已离开
        foreach (var grid in chessGrids)
        {
            // 寻找原来的格子
            if (grid.posX == chessPosition.x && grid.posY == chessPosition.y && grid.chesstype == _chess.GetComponent<Entity>().chesstype)
            {
                grid.ChessLeave();

                return true;
            }
        }

        return false;
    }
}
