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
            }
        }
    }

    public void LoadChessResource(string _type)
    {
        Debug.Log("开始装载类型为"+_type+"的棋子");

        var loadResource = Resources.LoadAll<GameObject>("Prefabs/Character/" + _type);


    }

    #endregion

    #region 备战部分
    public void EditChess(int _posX, int _posY, BattleEvent _edit)
    {
        
    }


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
