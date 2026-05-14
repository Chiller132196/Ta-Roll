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

    public List<Entity> GetAllOpponents(Chesstype _chessType)
    {
        List<Entity> targets = new List<Entity>();

        foreach (var grid in chessGrids)
        {
            if (grid.HasChess() && grid.chesstype != _chessType)
            {
                targets.Add(grid.chess.GetComponent<Entity>());
            }
        }

        return targets;
    }

    /// <summary>
    /// 寻找敌方生命值最高的棋子
    /// </summary>
    /// <param name="_mySide">己方阵营</param>
    /// <returns></returns>
    public Entity FindOpponentWithMaxHP(Chesstype _mySide)
    {
        List<Entity> opponents = GetAllOpponents(_mySide);
        if (opponents.Count == 0) return null;

        Entity target = opponents[0];
        foreach (var enemy in opponents)
        {
            if (enemy.battleHP > target.battleHP ||
               (enemy.battleHP == target.battleHP && enemy.battleATK > target.battleATK)) // 平局选ATK高
            {
                target = enemy;
            }
        }
        return target;
    }

    /// <summary>
    /// 寻找敌方攻击力最高的棋子
    /// </summary>
    /// <param name="_mySide">己方阵营</param>
    /// <returns></returns>
    public Entity FindOpponentWithMaxATK(Chesstype _mySide)
    {
        List<Entity> opponents = GetAllOpponents(_mySide);
        if (opponents.Count == 0) return null;

        Entity target = opponents[0];
        foreach (var enemy in opponents)
        {
            if (enemy.battleATK > target.battleATK ||
               (enemy.battleATK == target.battleATK && enemy.battleHP < target.battleHP)) // 平局选HP低
            {
                target = enemy;
            }
        }
        return target;
    }

    /// <summary>
    /// 寻找敌方生命值最低的棋子
    /// </summary>
    /// <param name="_mySide">己方阵营</param>
    /// <returns></returns>
    public Entity FindOpponentWithMinHP(Chesstype _mySide)
    {
        List<Entity> opponents = GetAllOpponents(_mySide);
        if (opponents.Count == 0) return null;

        Entity target = opponents[0];
        foreach (var enemy in opponents)
        {
            if (enemy.battleHP < target.battleHP ||
               (enemy.battleHP == target.battleHP && enemy.battleATK > target.battleATK)) // 平局选ATK高
            {
                target = enemy;
            }
        }
        return target;
    }

    /// <summary>
    /// 寻找敌方攻击力最低的棋子
    /// </summary>
    /// <param name="_mySide">己方阵营</param>
    /// <returns></returns>

    public Entity FindOpponentWithMinATK(Chesstype _mySide)
    {
        List<Entity> opponents = GetAllOpponents(_mySide);
        if (opponents.Count == 0) return null;

        Entity target = opponents[0];
        foreach (var enemy in opponents)
        {
            if (enemy.battleATK < target.battleATK ||
               (enemy.battleATK == target.battleATK && enemy.battleHP < target.battleHP)) // 平局选HP低
            {
                target = enemy;
            }
        }
        return target;
    }


}
