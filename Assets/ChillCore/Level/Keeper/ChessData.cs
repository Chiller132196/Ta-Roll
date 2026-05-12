using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessData
{
    /// <summary>
    /// 棋子的X坐标
    /// </summary>
    public int posX;

    /// <summary>
    /// 棋子的Y坐标
    /// </summary>
    public int posY;

    /// <summary>
    /// 棋子的预制件
    /// </summary>
    public GameObject chessPrefab;

    /// <summary>
    /// 棋子身上发生过的变化
    /// </summary>
    public BattleEvent chessEdit;
}
