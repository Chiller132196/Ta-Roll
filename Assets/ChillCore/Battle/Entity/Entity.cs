using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
 #region 基础属性
    /// <summary>
    /// 棋子的生命值
    /// </summary>
    internal int staticHP;

    /// <summary>
    /// 棋子的临时生命值
    /// </summary>
    internal int tempHP;

    /// <summary>
    /// 棋子的攻击力
    /// </summary>
    internal int staticATK;

    /// <summary>
    /// 棋子的临时攻击力
    /// </summary>
    internal int tempATK;

    /// <summary>
    /// 棋子的防御力
    /// </summary>
    internal int staticDF;

    /// <summary>
    /// 棋子的临时防御力
    /// </summary>
    internal int tempDF;

    /// <summary>
    /// 棋子的预充能
    /// </summary>
    internal int tempCharge;

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
    /// 战斗的防御力
    /// </summary>
    public int battleDP;

    /// <summary>
    /// 战斗中的充能量
    /// </summary>
    public int battleCharge;

    /// <summary>
    /// 战斗中，根据初始技能实例化的技能对象
    /// </summary>
    public Skill battleSkill;
    #endregion

    #region 对外属性
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
    internal BattleEvent battleEvent;

    internal Queue<BattleEvent> tempEvents;

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
            tempEvents.Enqueue(_battleEvent);
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
