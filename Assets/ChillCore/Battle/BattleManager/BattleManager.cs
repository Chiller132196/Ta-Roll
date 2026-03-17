using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public static BattleManager battleManager;

    /// <summary>
    /// 当前回合数
    /// </summary>
    public int round;

    /// <summary>
    /// 每个回合之间的时间间隔
    /// </summary>
    public float roundTimer;

    /// <summary>
    /// 战斗状态, -1静默, 0战斗, 1玩家胜利, 2玩家失败
    /// </summary>
    public int battleState;

    /// <summary>
    /// 本回合的单位
    /// </summary>
    public List<GameObject> entitysThisRound;

    #region 战斗控制

    /// <summary>
    /// 声明函数指针，通过事件收集每回合的棋子
    /// </summary>
    /// <returns>棋子的gameobject</returns>
    public delegate GameObject NewRoundBegin(int _round);

    /// <summary>
    /// 函数数组
    /// </summary>
    public static event NewRoundBegin OnNewRoundBegin;

    /// <summary>
    /// 新一局战斗开始时触发
    /// </summary>
    public void NewGameStart()
    {
        Debug.Log("New Battle Begin");

        battleState = 0;

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

    /// <summary>
    /// 检查战斗是否需要继续进行
    /// </summary>
    /// <returns>是否获胜</returns>
    public int CheckBattleState()
    {
        int playerChessAlive = 0;

        int enemyChessAlive = 0;

        // 检查在场玩家、敌人的棋子数量
        foreach(GameObject entity in entitysThisRound)
        {
            if (entity.GetComponent<Entity>().isPlayerChess)
            {
                playerChessAlive += 1;
            }

            else
            {
                enemyChessAlive += 1;
            }
        }

        // 分析是否结算战斗
        if (playerChessAlive > 0 && enemyChessAlive > 0)
        {
            return 0;
        }
        else
        {
            if (playerChessAlive <= 0)
            {
                BattleLose();
                return -1;
            }
            else
            {
                BattleWin();
                return -1;
            }
        }
    }

    /// <summary>
    /// 战斗胜利的逻辑
    /// </summary>
    public void BattleWin()
    {
        Debug.Log("玩家胜利！");
        battleState = -1;
    }

    /// <summary>
    /// 战斗失败的逻辑
    /// </summary>
    public void BattleLose()
    {
        Debug.Log("敌军胜利！");
        battleState = -1;
    }

    #endregion

    #region 回合控制

    public void NewRoundStart()
    {
        entitysThisRound = GetEntitys(round);

        if (entitysThisRound == null)
        {
            Debug.Log("!!!无在场单位，战斗无法继续!!!");
            return;
        }

        else
        {
            // 结算战场，如果-1即代表结束
            if (CheckBattleState() == -1)
            {
                return;
            }
        }

        round += 1;

        // 技能阶段，所有棋子尝试释放技能
        foreach (GameObject entity in entitysThisRound)
        {
            entity.GetComponent<Entity>().CastChessSkill();
        }

        // 结算阶段，所有棋子结算自己受到的技能效果
        foreach (GameObject entity in entitysThisRound)
        {
            entity.GetComponent<Entity>().CastStateCheck();
        }

        // 补给阶段，所有棋子进行回复
        foreach (GameObject entity in entitysThisRound)
        {
            entity.GetComponent<Entity>().CastSupply();
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
