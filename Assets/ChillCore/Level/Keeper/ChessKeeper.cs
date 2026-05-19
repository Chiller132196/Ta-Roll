using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChessKeeper : Singleton<ChessKeeper>
{
    /// <summary>
    /// 可以生成的棋子池
    /// </summary>
    public List<GameObject> SpawnChessPool;

    /// <summary>
    /// 当前玩家已有的棋子
    /// </summary>
    public List<ChessData> KeptChessPool;

    /// <summary>
    /// 用于查询棋子ID对应预制体的字典
    /// </summary>
    private Dictionary<string, GameObject> SpawnChessDict;

    #region 资源加载

    public void LoadAllChessResource()
    {
        Debug.Log("开始装载全部棋子");

        var loadResource = Resources.LoadAll<GameObject>("Prefabs/Character");

        foreach (var resource in loadResource)
        {
            if (!SpawnChessPool.Contains(resource))
            {
                SpawnChessPool.Add(resource);
            }

            if (!SpawnChessDict.ContainsKey(resource.GetComponent<Entity>().chessID))
            {
                SpawnChessDict.Add(resource.GetComponent<Entity>().chessID, resource);
            }
        }
    }

    public void LoadChessResource(string _type)
    {
        Debug.Log("开始装载类型为"+_type+"的棋子");

        var loadResource = Resources.LoadAll<GameObject>("Prefabs/Character/" + _type);

        foreach (var resource in loadResource)
        {
            if (resource.GetComponent<Entity>() != null)
            {
                if (!SpawnChessPool.Contains(resource))
                {
                    SpawnChessPool.Add(resource);
                }

                if (!SpawnChessDict.ContainsKey(resource.GetComponent<Entity>().chessID))
                {
                    SpawnChessDict.Add(resource.GetComponent<Entity>().chessID, resource);
                }

            }
        }
    }

    #endregion

    #region 备战部分

    /// <summary>
    /// 在最靠前的空位生成指定玩家棋子
    /// </summary>
    /// <param name="_chessID">棋子的ID</param>
    /// <returns></returns>
    public bool SummonPlayerChess(string _chessID)
    {
        int _posX = 1;
        int _posY = 1;

        var targetGrid = GridManager.gridManager.GetGridByXY(_posX, _posY, Chesstype.Player);

        if (!SpawnChessDict.ContainsKey(_chessID))
        {
            Debug.Log("---请求生成的棋子资源不存在或未加载---");

            return false;
        }

        if (targetGrid.HasChess())
        {
            Debug.Log("---该位置已被占用---");

            return false;
        }

        else
        {
            GameObject newChess = Instantiate(SpawnChessDict[_chessID]);

            ChessData newData = new();
            newChess.GetComponent<Entity>().Spawn(_posX, _posY);
            newChess.GetComponent<Entity>().myData = newData;

            newData.chessID = _chessID;
            newData.chessEdit = new();

            targetGrid.TeleportToMe(newChess);
        }

        return true;
    }

    public bool SummonPlayerChess(int _posX, int _posY, string _chessID)
    {
        var targetGrid = GridManager.gridManager.GetGridByXY(_posX, _posY, Chesstype.Player);

        if (!SpawnChessDict.ContainsKey(_chessID))
        {
            Debug.Log("---请求生成的棋子资源不存在或未加载---");

            return false;
        }

        if (targetGrid.HasChess())
        {
            Debug.Log("---该位置已被占用---");

            return false;
        }

        else
        {
            GameObject newChess = Instantiate(SpawnChessDict[_chessID]);

            ChessData newData = new();
            newChess.GetComponent<Entity>().Spawn(_posX, _posY);
            newChess.GetComponent<Entity>().myData = newData;

            newData.chessID = _chessID;
            newData.chessEdit = new();

            targetGrid.TeleportToMe(newChess);
        }

            return true;
    }

    /// <summary>
    /// 更新全部棋子信息
    /// </summary>
    /// <returns></returns>
    public void UpdateChessData()
    {
        if (GridManager.gridManager == null)
        {
            Debug.Log("未检测到棋盘");

            return;
        }

        foreach (ChessGrid grid in GridManager.gridManager.chessGrids)
        {
            if (grid.HasChess())
            {
                grid.chess.GetComponent<Entity>().UpdateMyData();
            }
        }

        Debug.Log("全局棋盘已保存，更新了 "+KeptChessPool.Count+" 枚棋子的信息");

        return;
    }

    #endregion

    #region 与战斗关联部分

    /// <summary>
    /// 战斗开始时，安放棋子
    /// </summary>
    public void ReleaseChess()
    {
        if (GridManager.gridManager == null)
        {
            Debug.Log("未检测到棋盘");

            return;
        }

        // 存在棋盘的条件下，开始放置棋子
        foreach(ChessData data in KeptChessPool)
        {
            var targetGrid = GridManager.gridManager.GetGridByXY(data.posX, data.posY, Chesstype.Player);

            //GameObject playerChess = Instantiate(data.chessPrefab);

            //playerChess.GetComponent<Entity>().GetBattleEvent(data.chessEdit);
        }

    }

    #endregion

    #region 调试部分

    public void SummonAnyPlayerChess()
    {
        //Debug.Log("尝试生成棋子");

        GameObject newChess = Instantiate(SpawnChessPool[0]);

        newChess.GetComponent<Entity>().Spawn(1, 1);

        GridManager.gridManager.GetGridByXY(1, 1, Chesstype.Player).TeleportToMe(newChess);
    }

    #endregion

    void Start()
    {
        KeptChessPool = new();

        SpawnChessDict = new();
    }

    void Update()
    {
        
    }
}
