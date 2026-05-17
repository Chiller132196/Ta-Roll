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
    /// 棋子的ID
    /// </summary>
    public int chessID;

    /// <summary>
    /// 棋子修改后的属性值
    /// </summary>
    public BattleEvent chessEdit;

    /// <summary>
    /// 棋子身上的修改差值
    /// </summary>
    public BattleEvent chessEditMemory;
}
