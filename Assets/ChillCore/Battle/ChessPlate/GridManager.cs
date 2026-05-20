using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 棋盘管理器：维护全部格子列表，提供移动、查询、选敌等接口。
/// 技能选目标时主要依赖 chessGrids 遍历；开局由 CollectChessGrids 自动收集子物体中的 ChessGrid。
/// </summary>
public class GridManager : MonoBehaviour
{
    #region 字段与单例

    /// <summary>
    /// 本局已注册的全部棋盘格（玩家 9 格 + 敌人 9 格，共 18 格）。
    /// 运行时由 CollectChessGrids 填充，勿依赖 Inspector 手填的 6 格旧数据。
    /// </summary>
    public List<ChessGrid> chessGrids;

    public static GridManager gridManager { get; private set; }

    #endregion

    #region 初始化

    void Awake()
    {
        if (gridManager != null && gridManager != this)
        {
            Destroy(gameObject);
            return;
        }

        gridManager = this;
        CollectChessGrids();
    }

    /// <summary>
    /// 从子物体收集全部 ChessGrid，写入 chessGrids。
    /// </summary>
    void CollectChessGrids()
    {
        ChessGrid[] grids = GetComponentsInChildren<ChessGrid>(true);

        if (grids.Length == 0)
        {
            Debug.LogWarning("GridManager 未找到任何 ChessGrid，请检查场景结构");
            return;
        }

        chessGrids = new List<ChessGrid>(grids);
        Debug.Log($"GridManager 已注册 {chessGrids.Count} 个格子");
    }

    #endregion

    #region 格子同步

    public void SyncGridChessReferences(List<GameObject> entities)
    {
        ClearInvalidChessReferences();

        if (entities == null)
            return;

        foreach (GameObject entityObject in entities)
        {
            if (entityObject == null)
                continue;

            Entity entity = entityObject.GetComponent<Entity>();
            if (entity == null || !entity.isAlive)
                continue;

            ChessGrid targetGrid = GetGridByXY(entity.posX, entity.posY, entity.chesstype);
            if (targetGrid == null)
                continue;

            if (targetGrid.chess != entityObject)
            {
                targetGrid.chess = entityObject;
            }
        }
    }

    public void ClearChessReference(GameObject chessObject)
    {
        foreach (var grid in chessGrids)
        {
            if (grid.chess == chessObject)
            {
                grid.ChessLeave();
                return;
            }
        }
    }

    private void ClearInvalidChessReferences()
    {
        foreach (var grid in chessGrids)
        {
            if (grid.chess == null)
                continue;

            Entity entity = grid.chess.GetComponent<Entity>();
            if (entity == null || !grid.chess.activeInHierarchy || !entity.isAlive || entity.chesstype != grid.chesstype)
            {
                grid.ChessLeave();
            }
        }
    }

    private void SyncFromCurrentBattle()
    {
        if (BattleManager.battleManager == null)
            return;

        SyncGridChessReferences(BattleManager.battleManager.entitysThisRound);
    }

    #endregion

    #region 棋子移动

    /// <summary>
    /// 将棋子传送到指定阵营的 (x, y) 格；目标格须为空。
    /// </summary>
    /// <param name="_chess">要移动的棋子</param>
    /// <param name="_x">目标行 posX（1~3）</param>
    /// <param name="_y">目标列 posY（1~3）</param>
    /// <param name="_needSide">目标格所属阵营</param>
    /// <returns>是否移动成功</returns>
    public bool TeleportChess(GameObject _chess, int _x, int _y, Chesstype _needSide)
    {
        Entity chessEntity = _chess.GetComponent<Entity>();

        foreach (var grid in chessGrids)
        {
            if (grid.posX != _x || grid.posY != _y || grid.chesstype != _needSide)
                continue;

            if (grid.HasChess())
                return false;

            grid.TeleportToMe(_chess);
            break;
        }

        foreach (var grid in chessGrids)
        {
            if (grid.posX == chessEntity.posX
            && grid.posY == chessEntity.posY
            && grid.chesstype == chessEntity.chesstype)
            {
                grid.ChessLeave();
                return true;
            }
        }

        return false;
    }

    #endregion

    #region 格子与棋子查询（按坐标）

    /// <summary>
    /// 按坐标与阵营查找格子组件（不保证格上有棋子）。
    /// </summary>
    /// <param name="_x">posX</param>
    /// <param name="_y">posY</param>
    /// <param name="_needSide">格子阵营</param>
    /// <returns>匹配的 ChessGrid；找不到时打 Error 并返回 null</returns>
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

    /// <summary>
    /// 在指定阵营中找任意一个空格子（遍历顺序不保证）。
    /// </summary>
    /// <param name="_type">阵营</param>
    /// <returns>空格子；没有则返回 null</returns>
    public ChessGrid GetAnyEmptyGrid(Chesstype _type)
    {
        ClearInvalidChessReferences();

        ChessGrid targetGrid = null;

        foreach (ChessGrid grid in chessGrids)
        {
            if (!grid.HasChess() && grid.chesstype == _type)
                targetGrid = grid;
        }

        if (targetGrid == null)
            Debug.Log("未能找到空的格子");

        return targetGrid;
    }

    /// <summary>
    /// 按坐标与阵营查找该格上的存活棋子。
    /// 对位攻击、定点技能（如 Skill_Sword_5）直接调用此方法即可。
    /// </summary>
    /// <param name="_x">posX</param>
    /// <param name="_y">posY</param>
    /// <param name="_needSide">要查询哪一方的棋盘（敌方阵营就传 Enemy）</param>
    /// <returns>棋子 GameObject；无棋子或已死亡则返回 null</returns>
    public GameObject GetGridChessByXY(int _x, int _y, Chesstype _needSide)
    {
        SyncFromCurrentBattle();

        foreach (var grid in chessGrids)
        {
            if (grid.posX != _x || grid.posY != _y || grid.chesstype != _needSide)
                continue;

            if (!grid.HasChess())
                continue;

            Entity entity = grid.chess.GetComponent<Entity>();
            if (entity != null && entity.isAlive)
                return grid.chess;
        }

        Debug.Log("未能找到符合技能释放条件的棋子");
        return null;
    }

    #endregion

    #region 敌方列表

    /// <summary>
    /// 收集所有敌方存活棋子（不限坐标）。
    /// </summary>
    /// <param name="_chessType">己方阵营；方法内部会筛选 chesstype 与之不同的格子</param>
    /// <returns>敌方 Entity 列表，可能为空</returns>
    public List<Entity> GetAllOpponents(Chesstype _chessType)
    {
        SyncFromCurrentBattle();

        List<Entity> targets = new List<Entity>();

        foreach (var grid in chessGrids)
        {
            if (!grid.HasChess() || grid.chesstype == _chessType)
                continue;

            Entity entity = grid.chess.GetComponent<Entity>();
            if (entity != null && entity.isAlive)
                targets.Add(entity);
        }

        return targets;
    }

    #endregion

    #region 敌方筛选（位置）

    /// <summary>
    /// 寻找敌方最前排棋子：按 posX 从 1→3、posY 从 1→3 扫描，返回第一个存活敌人。
    /// </summary>
    /// <param name="_ownerSide">施法者己方阵营</param>
    /// <returns>敌方 Entity；找不到则返回 null</returns>
    public Entity FindAnyOpponentFrontChess(Chesstype _ownerSide)
    {
        SyncFromCurrentBattle();

        for (int x = 1; x <= 3; x++)
        {
            for (int y = 1; y <= 3; y++)
            {
                foreach (var grid in chessGrids)
                {
                    if (grid.posX != x || grid.posY != y || grid.chesstype == _ownerSide)
                        continue;

                    if (!grid.HasChess())
                        continue;

                    Entity entity = grid.chess.GetComponent<Entity>();
                    if (entity != null && entity.isAlive)
                        return entity;
                }
            }
        }

        Debug.Log("---GridManager 未能找到合适的棋子---");
        return null;
    }

    /// <summary>
    /// 寻找敌方后排任意存活棋子：取敌方 posX 最大的一格上的棋子。
    /// </summary>
    /// <param name="_ownerSide">施法者己方阵营</param>
    /// <returns>敌方 Entity；找不到则返回 null</returns>
    public Entity FindAnyOpponentBackChess(Chesstype _ownerSide)
    {
        SyncFromCurrentBattle();

        Entity target = null;
        int maxX = -1;

        foreach (var grid in chessGrids)
        {
            if (!grid.HasChess() || grid.chesstype == _ownerSide)
                continue;

            Entity entity = grid.chess.GetComponent<Entity>();
            if (entity != null && entity.isAlive && grid.posX > maxX)
            {
                maxX = grid.posX;
                target = entity;
            }
        }

        return target;
    }

    #endregion

    #region 敌方筛选（属性极值）

    /// <summary>
    /// 敌方中 battleHP 最高者；HP 相同则选 battleATK 更高者。
    /// </summary>
    public Entity FindOpponentWithMaxHP(Chesstype _mySide)
    {
        List<Entity> opponents = GetAllOpponents(_mySide);
        if (opponents.Count == 0)
            return null;

        Entity target = opponents[0];
        foreach (var enemy in opponents)
        {
            if (enemy.battleHP > target.battleHP
            || (enemy.battleHP == target.battleHP && enemy.battleATK > target.battleATK))
            {
                target = enemy;
            }
        }

        return target;
    }

    /// <summary>
    /// 敌方中 battleATK 最高者；ATK 相同则选 battleHP 更低者。
    /// </summary>
    public Entity FindOpponentWithMaxATK(Chesstype _mySide)
    {
        List<Entity> opponents = GetAllOpponents(_mySide);
        if (opponents.Count == 0)
            return null;

        Entity target = opponents[0];
        foreach (var enemy in opponents)
        {
            if (enemy.battleATK > target.battleATK
            || (enemy.battleATK == target.battleATK && enemy.battleHP < target.battleHP))
            {
                target = enemy;
            }
        }

        return target;
    }

    /// <summary>
    /// 敌方中 battleHP 最低者；HP 相同则选 battleATK 更高者。
    /// </summary>
    public Entity FindOpponentWithMinHP(Chesstype _mySide)
    {
        List<Entity> opponents = GetAllOpponents(_mySide);
        if (opponents.Count == 0)
            return null;

        Entity target = opponents[0];
        foreach (var enemy in opponents)
        {
            if (enemy.battleHP < target.battleHP
            || (enemy.battleHP == target.battleHP && enemy.battleATK > target.battleATK))
            {
                target = enemy;
            }
        }

        return target;
    }

    /// <summary>
    /// 敌方中 battleATK 最低者；ATK 相同则选 battleHP 更低者。
    /// </summary>
    public Entity FindOpponentWithMinATK(Chesstype _mySide)
    {
        List<Entity> opponents = GetAllOpponents(_mySide);
        if (opponents.Count == 0)
            return null;

        Entity target = opponents[0];
        foreach (var enemy in opponents)
        {
            if (enemy.battleATK < target.battleATK
            || (enemy.battleATK == target.battleATK && enemy.battleHP < target.battleHP))
            {
                target = enemy;
            }
        }

        return target;
    }

    #endregion
}
