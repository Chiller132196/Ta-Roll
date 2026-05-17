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
    /// 收纳一个新棋子
    /// </summary>
    /// <returns></returns>
    public bool GetChess(ChessData _data)
    {

        return true;
    }

    public void LoadAllChessResource()
    {
        var loadResource = Resources.LoadAll<GameObject>("Prefabs/Character");

        foreach (var resource in loadResource)
        {
            if (resource.GetComponent<Entity>() != null)
            {
                SpawnChessPool.Add(resource);
            }
        }
    }

    public void LoadChessResource(string _type)
    {
        var loadResource = Resources.LoadAll<GameObject>("Prefabs/Character/" + _type);


    }

    public void EditChess()
    {
        
    }

    #region 与战斗关联部分

    /// <summary>
    /// 战斗开始时，安放棋子
    /// </summary>
    public void ReleaseChess()
    {
        foreach(ChessData data in KeptChessPool)
        {

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
