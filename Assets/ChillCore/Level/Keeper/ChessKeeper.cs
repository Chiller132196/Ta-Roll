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
    public bool GetChess(List<ChessData> _datas)
    {
        foreach (var data in _datas)
        {
            // 想生成的格子上有棋子了
            if (GridManager.gridManager.GetGridByXY(data.posX, data.posY, Chesstype.Player).HasChess())
            {
                return false;
            }

        }

        return true;
    }

    /// <summary>
    /// 战斗开始时，安放棋子
    /// </summary>
    public void ReleaseChess()
    {
        foreach(ChessData data in KeptChessPool)
        {
            Debug.Log("尝试生成" + data.chessPrefab.name);

            GameObject playerChess = Instantiate(data.chessPrefab);

            playerChess.GetComponent<Entity>().GetBattleEvent(data.chessEdit);
        }

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
