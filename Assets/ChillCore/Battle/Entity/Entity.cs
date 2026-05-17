using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region 基础属性
    /// <summary>
    /// 棋子的生命值
    /// </summary>
    public int maxHP;

    /// <summary>
    /// 棋子的攻击力
    /// </summary>
    public int basicATK;

    /// <summary>
    /// 棋子的基础护盾
    /// </summary>
    public int basicDF;

    /// <summary>
    /// 棋子的最大充能
    /// </summary>
    public int maxMP;

    /// <summary>
    /// 棋子的充能速度
    /// </summary>
    public int chargeSpeed;
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

    /// <summary>
    /// 棋子是否存活
    /// </summary>
    public bool isAlive;
    #endregion

    #region 对外属性
    /// <summary>
    /// 是否属于玩家阵容
    /// </summary>
    public Chesstype chesstype;

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
    /// 是否处于破绽状态
    /// </summary>
    public bool hasFlaw;

    /// <summary>
    /// 接收的战斗信息
    /// </summary>
    internal Queue<BattleEvent> battleEvents;

    /// <summary>
    /// 当前的回合数
    /// </summary>
    internal int nowRound;

    /// <summary>
    /// 棋子的标识符
    /// </summary>
    public string chessID;

    /// <summary>
    /// 在棋子管理器中对应的棋子数据
    /// </summary>
    public ChessData myData;

    public ChessElement chessElement;

    public ChessClass chessClass;
    #endregion

    #region 外观

    public GameObject stateBar;

    #endregion

    #region 战斗部分

    /// <summary>
    /// 接受战斗中的信息
    /// </summary>
    /// <param name="_battleEvent"></param>
    internal void GetBattleEvent(BattleEvent _battleEvent)
    {
        battleHP = Mathf.Min(maxHP, battleHP + _battleEvent.deltaHP);

        Debug.Log(gameObject.name + "生命值获得 " + _battleEvent.deltaHP + " 的变化量，变为 " + battleHP);

        battleMP = Mathf.Min(maxMP, battleMP + _battleEvent.deltaMP);

        Debug.Log(gameObject.name + "能量值获得 " + _battleEvent.deltaMP + " 的变化量");

        // 若此事件消耗弱点
        if (_battleEvent.consumedFlaw)
        {
            hasFlaw = false;
            EditFlawStateBar(hasFlaw);
        }

        // 若此事件不消耗弱点
        if (_battleEvent.bringFlaw)
        {
            hasFlaw = _battleEvent.bringFlaw;
            EditFlawStateBar(hasFlaw);
        }

        EditStateBar();

        if (battleHP <= 0)
        {
            Dead();
        }
    }

    /// <summary>
    /// 发生状态变化时，修改状态栏的属性
    /// </summary>
    internal void EditStateBar()
    {
        float nowHP = battleHP;

        float nowMP = battleMP;

        stateBar.GetComponent<StateBar>().StateChanged(nowHP / maxHP, nowMP / maxMP);

        Debug.Log(gameObject.name + " 初始化完毕, hp: " + battleHP + " df: " + battleDF + " atk: " + basicATK + " cs: " + battleChargeSpeed);
    }

    /// <summary>
    /// 破绽发生变动时，同步给状态栏
    /// </summary>
    internal void EditFlawStateBar(bool _delta)
    {
        if (stateBar != null)
        {
            stateBar.GetComponent<StateBar>().FlawChanged(_delta);
        }
        else
        {
            Debug.LogError("stateBar is null! Cannot update flaw state.");
        }
    }

    /// <summary>
    /// 释放技能行为
    /// </summary>
    /// <returns>是否释放成功</returns>
    internal bool CastChessSkill()
    {
        if (!isAlive)
        {
            return false;
        }

        if (skill.GetComponent<Skill>().OnCastSkill(this))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 补给行为
    /// </summary>
    /// <returns>补给后产生的状态变化</returns>
    internal virtual void CastSupply()
    {
        battleMP += battleChargeSpeed;

        EditStateBar();
    }

    /// <summary>
    /// 生成并加载初始属性
    /// </summary>
    public void Spawn()
    {
        isAlive = true;

        battleHP = maxHP;
        battleDF = basicDF;
        battleATK = basicATK;
        battleMP = 0;
        battleChargeSpeed = chargeSpeed;

        Debug.Log(gameObject.name + " 初始化完毕, hp: " + battleHP + " df: " + battleDF + " atk: " + basicATK + " mp: " + battleMP + " cs: " + battleChargeSpeed); ;
    }

    /// <summary>
    /// 附带修改的生成
    /// </summary>
    /// <param name="_spawnEvent"></param>
    public void Spawn(BattleEvent _spawnEvent)
    {
        isAlive = true;

        maxHP = Mathf.Max(0, _spawnEvent.deltaMaxHP);

        battleHP = Mathf.Max(0, _spawnEvent.deltaHP);

        battleDF = _spawnEvent.deltaDF;

        battleATK = Mathf.Max(0, _spawnEvent.deltaATK);

        maxMP = Mathf.Max(0, _spawnEvent.deltaMaxMP);

        battleMP = Mathf.Max(0, _spawnEvent.deltaMP);

        battleChargeSpeed = Mathf.Max(0, _spawnEvent.deltaChargeSpeed);
    }

    /// <summary>
    /// 棋子阵亡
    /// </summary>
    internal void Dead()
    {
        gameObject.SetActive(false);

        isAlive = false;
    }

    /// <summary>
    /// 新回合开始时，将物体返回给BattleManager
    /// </summary>
    /// <returns>挂载的物体</returns>
    internal GameObject RespondToNewRound(int _round)
    {
        inBattle = true;

        nowRound = _round;

        return gameObject;
    }

    #endregion

    #region 棋子站位

    public void TeleportMe(int posX, int posY)
    {
        GridManager.gridManager.TeleportChess(gameObject, posX, posY, chesstype);
    }

    #endregion

    #region 局外养成

    public void UpdateMyData()
    {
        BattleEvent mySatus = new BattleEvent();

        mySatus.deltaMaxHP = maxHP;
        mySatus.deltaMaxMP = maxMP;
        mySatus.deltaATK = basicATK;
        mySatus.deltaDF = basicDF;
        mySatus.deltaChargeSpeed = chargeSpeed;

        myData.chessEdit = mySatus;
    }

    #endregion

    public void OnEnable()
    {
        if (battleSkill == null && inBattle)
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
