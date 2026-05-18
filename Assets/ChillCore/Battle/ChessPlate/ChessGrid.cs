using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ?????????????????????????????????????????
/// GridManager ??? chessGrids ?????????HasChess / chess ????????????????????????????
/// </summary>
public class ChessGrid : MonoBehaviour
{
    /// <summary>
    /// 格子上的棋子
    /// </summary>
    public GameObject chess;

    /// <summary>
    /// 占位用的棋子，仅在无法返回棋子时调用
    /// </summary>
    public GameObject emptyChess;

    /// <summary>
    /// 这个格子只能由什么阵营站立
    /// </summary>
    public Chesstype chesstype;

    public int posX;

    public int posY;

    void Start()
    {
        if (chess == null) return;

        Entity entity = chess.GetComponent<Entity>();
        if (entity == null) return;

        entity.posX = posX;
        entity.posY = posY;
    }

    public GameObject GetChess()
    {
        if (!chess)
        {
            return emptyChess;
        }
        else if (!chess.GetComponent<Entity>().isAlive)
        {
            return emptyChess;
        }

        return chess;
    }

    /// <summary>
    /// 查询是否有棋子在此格子上
    /// </summary>
    /// <returns>是/否</returns>
    public bool HasChess()
    {
        if (!chess)
        {
            //Debug.Log("-----" + gameObject.name + " : 我没有棋子-----");

            return false;
        }
        /*
        if (!chess.GetComponent<Entity>().isAlive)
        {
        return false;
        }*/

        return true;
    }

    /// <summary>
    /// 将棋子传送到这个格子上
    /// </summary>
    /// <param name="_chess"></param>
    /// <returns></returns>
    public bool TeleportToMe(GameObject _chess)
    {
        if (HasChess())
        {
            return false;
        }

        else
        {
            Entity entity = _chess.GetComponent<Entity>();

            _chess.transform.position = gameObject.transform.position;

            chess = _chess;
            entity.posX = posX;
            entity.posY = posY;

            Debug.Log(_chess.name + "移动到了 " + gameObject.name + " 的位置");

            return true;
        }
    }

    public void ChessLeave()
    {
        chess = null;
    }

}