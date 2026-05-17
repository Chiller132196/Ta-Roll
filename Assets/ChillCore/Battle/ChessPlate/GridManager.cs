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
    /// 寻找一个最靠前的棋子（基于最小X坐标）
    /// </summary>
    /// <param name="_needSide">需要的阵营</param>
    /// <returns>最靠前的存活棋子</returns>

    public bool TeleportChess(GameObject _chess, int _x, int _y, Chesstype _needSide)
    {
        Entity chessEntity = _chess.GetComponent<Entity>();

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
            if (grid.posX == chessEntity.posX && grid.posY == chessEntity.posY && grid.chesstype == _chess.GetComponent<Entity>().chesstype)
            {
                grid.ChessLeave();

                return true;
            }
        }

        return false;
    }
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
    // 根据XY找格子

    public GameObject GetGridChessByXY(int _x, int _y, Chesstype _needSide)
    {
        foreach (var grid in chessGrids)
        {
            if (grid.posX == _x && grid.posY == _y && grid.chesstype == _needSide)
            {
                if (grid.HasChess())
                {
                    Entity entity = grid.chess.GetComponent<Entity>();
                    if (entity != null && entity.isAlive)
                    {
                        return grid.chess;
                    }
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

    public List<Entity> GetAllOpponents(Chesstype _chessType)
    {
        List<Entity> targets = new List<Entity>();

        foreach (var grid in chessGrids)
        {
            if (grid.HasChess() && grid.chesstype != _chessType)
            {
                Entity entity = grid.chess.GetComponent<Entity>();
                if (entity != null && entity.isAlive)
                {
                    targets.Add(entity);
                }
            }
        }

        return targets;
    }

    /// <summary>
    /// 寻找一个对面最靠前的棋子（基于最小X坐标）
    /// </summary>
    /// <param name="_needSide">自己所在的阵容</param>
    /// <returns>敌方最靠前的存活棋子</returns>
    public Entity FindAnyOpponentFrontChess(Chesstype _needSide)
    {
        Entity target = null;
        int searchPosX = 1;
        int searchPosY = 1;

        foreach (var grid in chessGrids)
        {
            if (grid.posX != searchPosX || grid.posY != searchPosY || grid.chesstype != _needSide)
            {
                continue;
            }

            else if (grid.HasChess() && grid.chesstype != _needSide)
            {
                Entity entity = grid.chess.GetComponent<Entity>();

                if (entity.isAlive)
                {
                    target = entity;
                    break;
                }

                else
                {
                    searchPosY++;
                    if (searchPosY > 3)
                    {
                        searchPosY = 1;
                        searchPosX++;
                    }
                }
            }
        }

        if (target == null)
        {
            Debug.Log("---GridManager 未能找到合适的棋子---");
        }

        // 如果找到了目标，返回；否则返回 null
        return target;
    }

    /// <summary>
    /// 寻找敌方后排（X坐标最大）的任意一个棋子
    /// </summary>
    /// <param name="_ownerSide">己方阵营</param>
    /// <returns>敌方后排的一个存活棋子，如果没有则返回 null</returns>
    public Entity FindAnyOpponentBackChess(Chesstype _ownerSide)
    {
        Entity target = null;
        int max_X = -1;

        foreach (var grid in chessGrids)
        {
            if (grid.HasChess() && grid.chesstype != _ownerSide)
            {
                Entity entity = grid.chess.GetComponent<Entity>();
                if (entity != null && entity.isAlive && grid.posX > max_X)
                {
                    max_X = grid.posX;
                    target = entity;
                }
            }
        }

        return target;
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