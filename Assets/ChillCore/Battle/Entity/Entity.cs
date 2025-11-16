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

    #endregion

    #region 战斗属性

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

    #endregion

    #region 对外属性
    public System.Tuple<int, int> position;

    /// <summary>
    /// 棋子的普通攻击
    /// </summary>
    public Skill attack;

    /// <summary>
    /// 棋子的移动技能
    /// </summary>
    public Skill move;

    /// <summary>
    /// 棋子的特殊技能
    /// </summary>
    public Skill spell;

    /// <summary>
    /// 棋子的特殊增益
    /// </summary>
    public Skill buff;

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

    internal void GetBattleEvent(BattleEvent _battleEvent)
    {
            tempEvents.Enqueue(_battleEvent);
    }

    internal void ReadBattleEvent()
    {

    }

    internal void RespondBattleEvent()
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

    void Start()
    {
        
    }

    void Update()
    {
        ReadBattleEvent();
    }

    public void OnEnable()
    {
        BattleManager.OnNewRoundBegin += RespondToNewRound;
    }

    public void OnDisable()
    {
        BattleManager.OnNewRoundBegin -= RespondToNewRound;
    }
}
