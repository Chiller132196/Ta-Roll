using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessKeeper : MonoBehaviour
{
    /// <summary>
    /// 可以生成的棋子池
    /// </summary>
    public List<GameObject> SpawnChessPool;


    /// <summary>
    /// 当前玩家已有的棋子
    /// </summary>
    public List<ChessData> KeptChessPool;

    public bool SpawnChess(int _posX, int _posY, ChessElement _element, ChessClass _class, Chesstype _needSide)
    {
        // 想生成的格子上有棋子了
        if (GridManager.gridManager.GetGridByXY(_posX, _posY, _needSide).HasChess())
        {
            return false;
        }

        return false;
    }

    public void ReleaseChess()
    {
        foreach(ChessData data in KeptChessPool)
        {
            Debug.Log("尝试生成" + data.chessPrefab.name);

            BattleEvent releaseEvent = new BattleEvent();

            GameObject playerChess = Instantiate(data.chessPrefab);

            releaseEvent.deltaATK = data.deltaAtk;
            releaseEvent.deltaDF = data.deltaDF;
        }


    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
