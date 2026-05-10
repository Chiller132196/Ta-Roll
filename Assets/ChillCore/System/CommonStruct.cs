using System;

public class ChessPosition
{
    public int x;

    public int y;
}

/// <summary>
/// 棋子的阵营
/// </summary>
public enum Chesstype
{
    Player,
    Enemy,
    Neutral
}

/// <summary>
/// 棋子的元素
/// </summary>
public enum ChessElement
{
    /// <summary>
    /// 风元素
    /// </summary>
    Wind,
    /// <summary>
    /// 水元素
    /// </summary>
    Water,
    /// <summary>
    /// 火元素
    /// </summary>
    Fire,
    /// <summary>
    /// 土元素
    /// </summary>
    Soil
}

/// <summary>
/// 棋子的数字阶层
/// </summary>
public enum ChessClass
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    /// <summary>
    /// 王后
    /// </summary>
    Queen,
    /// <summary>
    /// 骑士
    /// </summary>
    Knight,
    /// <summary>
    /// 国外
    /// </summary>
    King,
    /// <summary>
    /// 侍从
    /// </summary>
    Page,
}