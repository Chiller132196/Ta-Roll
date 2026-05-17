using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Dictionary<string, GameObject> SpawnChessDict = new();

    #region 资源加载

    public void LoadAllChessResource()
    {
        Debug.Log("开始装载全部棋子");

        var loadResource = Resources.LoadAll<GameObject>("Prefabs/Character");

        foreach (var resource in loadResource)
        {
            if (resource.GetComponent<Entity>() != null)
            {
                SpawnChessPool.Add(resource);

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
                SpawnChessPool.Add(resource);

                SpawnChessDict.Add(resource.GetComponent<Entity>().chessID, resource);
            }
        }
    }

    #endregion

    #region 备战部分

    /// <summary>
    /// 更新全部棋子信息
    /// </summary>
    /// <returns></returns>
    public bool UpdateChessData(ChessData _data)
    {
        foreach (ChessData chess in KeptChessPool)
        {
            
        }

        return true;
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
