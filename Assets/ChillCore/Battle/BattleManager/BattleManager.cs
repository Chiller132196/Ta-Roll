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

    #region 战斗控制
    public void NewGameStart()
    {
        Debug.Log("New Battle Begin");

        round = 0;

        entitysThisRound = GetEntitys(round);

        if (entitysThisRound == null)
        {
            Debug.Log("!!!无在场单位，战斗无法继续!!!");
            return;
        }

        else
        {
            NewRoundStart();
        }

    }

    #endregion

    #region 回合控制

    public void NewRoundStart()
    {
        round += 1;

        entitysThisRound = GetEntitys(round);

        if (entitysThisRound == null)
        {
            Debug.Log("!!!无在场单位，战斗无法继续!!!");
            return;
        }

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

        if (OnNewRoundBegin != null)
        {
            System.Delegate[] invocators = OnNewRoundBegin.GetInvocationList();

            foreach (NewRoundBegin entity in invocators)
            {
                entitys.Add(entity(_round));
            }

            return entitys;
        }

        else
        {
            Debug.Log("!!!无实体在场，检查是否设置错误!!!");
            return null;
        }
    }

    #endregion

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
