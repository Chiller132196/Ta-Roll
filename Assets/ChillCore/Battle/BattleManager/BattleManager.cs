using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public static BattleManager battleManager;
    
    /// <summary>
    /// 声明函数指针，通过事件收集每回合的棋子
    /// </summary>
    /// <returns>棋子的gameobject</returns>
    public delegate GameObject NewRoundBegin(int _round);

    public static event NewRoundBegin OnNewRoundBegin;

    public int round;

    public List<GameObject> entitysThisRound;

    public void NewGameStart()
    {
        round = 0;

        NewRoundStart();
    }

    public void NewRoundStart()
    {
        round += 1;

        entitysThisRound = GetEntitys(round);

        foreach (GameObject entity in entitysThisRound)
        {

        }
    }

    /// <summary>
    /// 获取场上的棋子
    /// </summary>
    /// <param name="_round">当前回合</param>
    /// <returns>全部实体的列表</returns>
    public List<GameObject> GetEntitys(int _round)
    {
        List<GameObject> entitys = new List<GameObject>();

        foreach (NewRoundBegin entity in OnNewRoundBegin.GetInvocationList())
        {
            entitys.Add(entity(_round));
        }

        return entitys;
    }

    void Start()
    {
        NewGameStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
