using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// P-Cups-3 技能：水幕罩
/// 特性：中等频率1.5倍伤害，专打最强威胁（ATK最高）；攻击力平局时优先选生命值最低者
/// </summary>
public class Skill_Cups_3 : Skill
{
    // 技能名称
    public string skillName = "水幕罩";

    /// <summary>
    /// 释放技能的核心逻辑
    /// </summary>
    /// <param name="_owner">施法者实体</param>
    /// <returns>是否成功释放</returns>
    internal override bool CastSkill(Entity _owner)
    {
        // 清空之前的目标列表，确保每次释放都是全新的选择
        targets.Clear();

        // 调用 GridManager 的方法，寻找敌方攻击力最高的棋子
        Entity target = GridManager.gridManager.FindOpponentWithMaxATK(_owner.chesstype);

        // 如果找不到合适的目标（例如场上没有敌人），则释放失败
        if (target == null)
        {
            return false;
        }

        // 将找到的目标添加到本次技能的目标列表中
        targets.Add(target);

        // 计算伤害：基础攻击力 * 1.5 倍率，并强制转换为整数
        int damage = (int)(_owner.battleATK * 1.5f);

        // 创建战斗事件对象，用于传递伤害信息
        BattleEvent skillEffect = new BattleEvent();
        skillEffect.deltaHP = -damage; // 负数表示扣除生命值
        skillEffect.owner = _owner;    // 记录伤害来源

        // 遍历所有目标（此处通常只有一个）并应用效果
        foreach (Entity _target in targets)
        {
            // 让目标接收战斗事件，处理扣血、破绽消耗等逻辑
            _target.GetBattleEvent(skillEffect);
            
            // 在控制台输出日志，方便调试和观察战斗过程
            Debug.Log(_owner.gameObject.name + " 对 " + _target.gameObject.name + " 释放了 " + skillName + "，造成 " + damage + " 点伤害");
        }

        // 返回 true 表示技能成功执行
        return true;
    }
}