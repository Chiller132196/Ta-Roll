using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实体（棋子）核心脚本
/// 功能：所有棋子（我方/敌方）的属性管理、受伤、放技能、死亡、状态同步
/// 挂载在每一个棋子预制体上
/// </summary>
public class Entity : MonoBehaviour
{
    #region 基础属性（配置表/面板填写的初始属性）
    /// <summary>
    /// 最大生命值
    /// </summary>
    public int maxHP;

    /// <summary>
    /// 基础攻击力
    /// </summary>
    public int basicATK;

    /// <summary>
    /// 基础护盾/防御
    /// </summary>
    public int basicDF;

    /// <summary>
    /// 最大能量（满蓝）
    /// </summary>
    public int maxMP;

    /// <summary>
    /// 基础充能速度（每回合回蓝速度）
    /// </summary>
    public int chargeSpeed;
    #endregion

    #region 战斗属性（战斗中实时变化的属性）
    /// <summary>
    /// 是否在战斗中
    /// </summary>
    public bool inBattle;

    /// <summary>
    /// 当前剩余生命值
    /// </summary>
    public int battleHP;

    /// <summary>
    /// 当前攻击力（含加成）
    /// </summary>
    public int battleATK;

    /// <summary>
    /// 当前护盾值
    /// </summary>
    public int battleDF;

    /// <summary>
    /// 当前能量值（蓝量）
    /// </summary>
    public int battleMP;

    /// <summary>
    /// 当前充能速度（含加成）
    /// </summary>
    public int battleChargeSpeed;

    /// <summary>
    /// 战斗中实例化出来的技能对象（运行时用）
    /// </summary>
    public Skill battleSkill;

    /// <summary>
    /// 是否存活
    /// </summary>
    public bool isAlive;
    #endregion

    #region 对外标识属性（阵营、位置、类型）
    /// <summary>
    /// 阵营：玩家 / 敌人
    /// </summary>
    public Chesstype chesstype;

    /// <summary>
    /// 当前所在格子 X 坐标
    /// </summary>
    public int posX;

    /// <summary>
    /// 当前所在格子 Y 坐标
    /// </summary>
    public int posY;

    /// <summary>
    /// 技能预制体（配置拖拽）
    /// </summary>
    public GameObject skill;

    /// <summary>
    /// 增益效果预制体
    /// </summary>
    public GameObject buff;

    /// <summary>
    /// 是否处于【破绽/易伤】状态
    /// </summary>
    public bool hasFlaw;

    /// <summary>
    /// 战斗事件队列（受伤、加蓝、加盾等都会进队列）
    /// </summary>
    internal Queue<BattleEvent> battleEvents;

    /// <summary>
    /// 当前回合数
    /// </summary>
    internal int nowRound;

    /// <summary>
    /// 棋子唯一ID
    /// </summary>
    public string chessID;

    /// <summary>
    /// 对应棋子表的数据
    /// </summary>
    public ChessData myData;

    /// <summary>
    /// 元素属性：风/水/火/土
    /// </summary>
    public ChessElement chessElement;

    /// <summary>
    /// 职业：战士/法师/射手等
    /// </summary>
    public ChessClass chessClass;
    #endregion

    #region 外观/飘字设置
    /// <summary>
    /// 头顶状态栏（血条+蓝条）
    /// </summary>
    public GameObject stateBar;
    /// <summary>
    /// 伤害飘字的位置偏移量（调整飘字在角色头顶的位置）
    /// </summary>
    public Vector3 damagePopupOffset = new Vector3(0f, -0.35f, 0f);
    #endregion

    #region 战斗核心逻辑
    /// <summary>
    /// 接收战斗事件（受伤、加蓝、加盾、破绽等）
    /// 外部（技能/攻击）调用这个方法让棋子掉血/加蓝
    /// </summary>
    internal void GetBattleEvent(BattleEvent _battleEvent)
    {
        // 如果是掉血（deltaHP 是负数），显示伤害飘字
        if (_battleEvent.deltaHP < 0)
        {
            ShowDamagePopup(_battleEvent.deltaHP, _battleEvent.consumedFlaw);
        }

        // 计算最新血量：不能超过最大血量
        battleHP = Mathf.Min(maxHP, battleHP + _battleEvent.deltaHP);
        Debug.Log(gameObject.name + " 生命值变化：" + _battleEvent.deltaHP + "，当前：" + battleHP);

        // 计算最新蓝量：不能超过最大蓝量
        battleMP = Mathf.Min(maxMP, battleMP + _battleEvent.deltaMP);
        Debug.Log(gameObject.name + " 能量变化：" + _battleEvent.deltaMP);

        // 如果这次事件要消耗破绽 → 清除破绽
        if (_battleEvent.consumedFlaw)
        {
            hasFlaw = false;
            EditFlawStateBar(hasFlaw);
        }

        // 如果这次事件要附加破绽 → 设置破绽
        if (_battleEvent.bringFlaw)
        {
            hasFlaw = _battleEvent.bringFlaw;
            EditFlawStateBar(hasFlaw);
        }

        // 更新UI血条/蓝条
        EditStateBar();

        // 血量 ≤ 0 → 死亡
        if (battleHP <= 0)
        {
            Dead();
        }
    }

    /// <summary>
    /// 更新血条、蓝条UI显示
    /// </summary>
    internal void EditStateBar()
    {
        // 计算血条百分比、蓝条百分比
        float nowHP = battleHP;
        float nowMP = battleMP;

        // 通知状态栏刷新显示
        stateBar.GetComponent<StateBar>().StateChanged(nowHP / maxHP, nowMP / maxMP);

        Debug.Log(gameObject.name + " 状态更新：HP=" + battleHP + " ATK=" + battleATK);
    }

    /// <summary>
    /// 更新破绽状态UI
    /// </summary>
    internal void EditFlawStateBar(bool _delta)
    {
        if (stateBar != null)
        {
            stateBar.GetComponent<StateBar>().FlawChanged(_delta);
        }
        else
        {
            Debug.LogError("未绑定状态栏，无法更新破绽状态！");
        }
    }

    /// <summary>
    /// 显示伤害飘字（调用2D飘字）
    /// </summary>
    private void ShowDamagePopup(int deltaHP, bool isBreakDamage)
    {
        int damageValue = deltaHP;
        ScreenDamagePopup.Show(damageValue, GetDamagePopupBasePosition(), isBreakDamage);
    }

    /// <summary
    /// 计算伤害飘字应该出现的位置
    /// 优先用状态栏位置，没有就用模型顶部
    /// </summary>
    private Vector3 GetDamagePopupBasePosition()
    {
         // 直接返回血条位置，不叠加任何偏移 → 文字和血条完全贴在一起
        if (stateBar != null)
        {
            return stateBar.transform.position;
        }
        
        return transform.position;
    }

    /// <summary>
    /// 释放棋子技能
    /// </summary>
    internal bool CastChessSkill()
    {
        // 死了不能放技能
        if (!isAlive) return false;

        // 调用技能脚本的释放方法
        if (skill.GetComponent<Skill>().OnCastSkill(this))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 每回合补给：自动回蓝
    /// </summary>
    internal virtual void CastSupply()
    {
        battleMP += battleChargeSpeed;
        EditStateBar();
    }

    /// <summary>
    /// 死亡逻辑
    /// </summary>
    internal void Dead()
    {
        gameObject.SetActive(false); // 隐藏棋子
        isAlive = false;             // 标记死亡
    }

    /// <summary>
    /// 新回合开始时，棋子向战斗管理器报到
    /// </summary>
    internal GameObject RespondToNewRound(int _round)
    {
        inBattle = true;
        nowRound = _round;
        return gameObject;
    }
    #endregion

    #region 棋子移动（传送）
    /// <summary>
    /// 把自己传送到指定坐标
    /// </summary>
    public void TeleportMe(int _posX, int _posY)
    {
        GridManager.gridManager.TeleportChess(gameObject, _posX, _posY, chesstype);
    }
    #endregion

    #region 局外养成 / 生成逻辑
    /// <summary>
    /// 生成棋子：初始化基础属性
    /// </summary>
    public void Spawn()
    {
        isAlive = true;

        battleHP = maxHP;            // 满血
        battleDF = basicDF;          // 初始防御
        battleATK = basicATK;        // 初始攻击
        battleMP = 0;                // 初始0蓝
        battleChargeSpeed = chargeSpeed; // 初始回蓝

        Debug.Log(gameObject.name + " 生成完成！");
    }

    /// <summary>
    /// 带坐标生成
    /// </summary>
    public void Spawn(int _posX, int _posY)
    {
        isAlive = true;
        posX = _posX;
        posY = _posY;

        battleHP = maxHP;
        battleDF = basicDF;
        battleATK = basicATK;
        battleMP = 0;
        battleChargeSpeed = chargeSpeed;

        Debug.Log(gameObject.name + " 生成完成！坐标：" + posX + "," + posY);
    }

    /// <summary>
    /// 带事件（自定义属性）生成
    /// </summary>
    public void Spawn(BattleEvent _spawnEvent, int _posX, int _posY)
    {
        isAlive = true;
        posX = _posX;
        posY = _posY;

        maxHP = Mathf.Max(0, _spawnEvent.deltaMaxHP);
        battleHP = Mathf.Max(0, _spawnEvent.deltaHP);
        battleDF = _spawnEvent.deltaDF;
        battleATK = Mathf.Max(0, _spawnEvent.deltaATK);
        maxMP = Mathf.Max(0, _spawnEvent.deltaMaxMP);
        battleMP = Mathf.Max(0, _spawnEvent.deltaMP);
        battleChargeSpeed = Mathf.Max(0, _spawnEvent.deltaChargeSpeed);

        Debug.Log(gameObject.name + " 自定义生成完成！");
    }

    /// <summary>
    /// 把当前属性同步到数据表里
    /// </summary>
    public void UpdateMyData()
    {
        BattleEvent mySatus = new BattleEvent();

        mySatus.deltaMaxHP = maxHP;
        mySatus.deltaMaxMP = maxMP;
        mySatus.deltaATK = basicATK;
        mySatus.deltaDF = basicDF;
        mySatus.deltaChargeSpeed = chargeSpeed;

        myData.posX = posX;
        myData.posY = posY;
        myData.chessEdit = mySatus;
    }
    #endregion

    #region 生命周期
    /// <summary>
    /// 启用时：创建技能、注册回合事件
    /// </summary>
    public void OnEnable()
    {
        // 如果战斗中还没创建技能 → 创建一个
        if (battleSkill == null && inBattle)
        {
            battleSkill = Instantiate(skill).GetComponent<Skill>();
        }

        // 订阅“新回合开始”事件
        BattleManager.OnNewRoundBegin += RespondToNewRound;
    }

    /// <summary>
    /// 禁用时：销毁技能、取消事件订阅
    /// </summary>
    public void OnDisable()
    {
        if (battleSkill != null)
        {
            battleSkill.DestroySkill();
        }

        BattleManager.OnNewRoundBegin -= RespondToNewRound;
    }
    #endregion
}