using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

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
            _chess.transform.position = gameObject.transform.position;

            chesstype = _chess.GetComponent<Entity>().chesstype;

            Debug.Log(_chess.name + "移动到了 " + gameObject.name + " 的位置");

            return true;
        }
    }

    public void ChessLeave()
    {
        chess = null;
    }

}
