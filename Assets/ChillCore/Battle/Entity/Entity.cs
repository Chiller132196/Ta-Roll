using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
 #region 基础属性
    /// <summary>
    /// 棋子的生命值
    /// </summary>
    internal int maxHP;

    /// <summary>
    /// 棋子的攻击力
    /// </summary>
    internal int maxATK;

    /// <summary>
    /// 棋子的基础护盾
    /// </summary>
    internal int maxDF;

    /// <summary>
    /// 棋子的最大充能
    /// </summary>
    internal int maxMP;

    /// <summary>
    /// 棋子的充能速度
    /// </summary>
    internal int chargeSpeed;
    #endregion

    #region 战斗属性
    /// <summary>
    /// 是否处于交战状态
    /// </summary>
    public bool inBattle;

    /// <summary>
    /// 战斗的生命值
    /// </summary>
    public int battleHP;

    /// <summary>
    /// 战斗的攻击力
    /// </summary>
    public int battleATK;

    /// <summary>
    /// 战斗的护盾值
    /// </summary>
    public int battleDF;

    /// <summary>
    /// 战斗中的充能量
    /// </summary>
    public int battleMP;

    /// <summary>
    /// 战斗中棋子的充能速度
    /// </summary>
    public int battleChargeSpeed;

    /// <summary>
    /// 战斗中，根据初始技能实例化的技能对象
    /// </summary>
    public Skill battleSkill;
    #endregion

    #region 对外属性
    /// <summary>
    /// 是否属于玩家阵容
    /// </summary>
    public bool isPlayerChess;

    /// <summary>
    /// 棋子在棋盘的位置
    /// </summary>
    public ChessPosition chessPosition;

    /// <summary>
    /// 棋子的特殊技能
    /// </summary>
    public GameObject skill;

    /// <summary>
    /// 棋子的特殊增益
    /// </summary>
    public GameObject buff;

    /// <summary>
    /// 接收的战斗信息
    /// </summary>
    internal Queue<BattleEvent> battleEvents;

    /// <summary>
    /// 当前的回合数
    /// </summary>
    internal int nowRound;
    #endregion

    #region 外观

    public GameObject stateBar;

    #endregion

    /// <summary>
    /// 接受战斗中的信息
    /// </summary>
    /// <param name="_battleEvent"></param>
    internal void GetBattleEvent(BattleEvent _battleEvent)
    {
            battleEvents.Enqueue(_battleEvent);
    }

    /// <summary>
    /// 发生状态变化时，修改状态栏的属性
    /// </summary>
    internal void EditStateBar()
    {

    }

    /// <summary>
    /// 释放技能行为
    /// </summary>
    /// <returns>是否释放成功</returns>
    internal bool CastChessSkill()
    {
        if (skill.GetComponent<Skill>().OnCastSkill() == null)
        {
            return false;
        }



        return true;
    }

    /// <summary>
    /// 结算战场中的状态变化行为
    /// </summary>
    internal void CastStateCheck()
    {
        foreach(BattleEvent battleEvent in battleEvents)
        {

        }
    }

    /// <summary>
    /// 补给行为
    /// </summary>
    /// <returns>补给后产生的状态变化</returns>
    internal BattleEvent CastSupply()
    {
        return null;
    }

    /// <summary>
    /// 棋子阵亡
    /// </summary>
    internal void Dead()
    {

    }

    /// <summary>
    /// 新回合开始时，将物体返回给BattleManager
    /// </summary>
    /// <returns>挂载的物体</returns>
    internal GameObject RespondToNewRound(int _round)
    {
        nowRound = _round;

        return gameObject;
    }

    public void OnEnable()
    {
        if (battleSkill == null)
        {
            battleSkill = Instantiate(skill).GetComponent<Skill>();
        }

        BattleManager.OnNewRoundBegin += RespondToNewRound;
    }

    public void OnDisable()
    {
        if (battleSkill != null)
        {
            battleSkill.DestroySkill();
        }

        BattleManager.OnNewRoundBegin -= RespondToNewRound;
    }
}
