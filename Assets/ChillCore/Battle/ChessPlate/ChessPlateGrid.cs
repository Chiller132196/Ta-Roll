using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlateGrid : MonoBehaviour
{
    public int x;

    public int y;

    /// <summary>
    /// 自身的世界坐标，用于校准棋子位置
    /// </summary>
    public Vector3 worldPosition;

    /// <summary>
    /// 获取格子的棋盘坐标
    /// </summary>
    /// <returns>棋盘坐标</returns>
    public System.Tuple<int, int> GetGridLocation()
    {
        System.Tuple<int, int> location = new System.Tuple<int, int>(x, y);

        return location; 
    }

    public Vector3 GetWorldPosition()
    {
        return worldPosition;
    }

    void Start()
    {
        worldPosition = GetComponent<Transform>().position;
    }
}
