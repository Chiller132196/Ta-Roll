using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessSpawner : MonoBehaviour
{
    public List<GameObject> SpawnChessPool;

    public bool SpawnChess(int _posX, int _posY, ChessElement _element, ChessClass _class, Chesstype _needSide)
    {
        // 想生成的格子上有棋子了
        if (GridManager.gridManager.GetGridByXY(_posX, _posY, _needSide).HasChess())
        {
            return false;
        }

        return false;
    }
}
