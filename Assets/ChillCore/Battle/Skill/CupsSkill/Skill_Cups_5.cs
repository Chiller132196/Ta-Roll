using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// P-Cups-5 技能：冰封箭
/// 特性：自带破绽，专打最弱攻击者（ATK最低）；ATK平局时优先选生命值最低者
/// </summary>
public class Skill_Cups_5 : Skill
{
    // 技能名称
    public string skillName = "冰封箭";

    /// <summary>
    /// 释放技能的核心逻辑
    /// </summary>
    /// <param name="_owner">施法者实体</param>
    /// <returns>是否成功释放</returns>
    internal override bool CastSkill(Entity _owner)
    {
        // 清空之前的目标列表
        targets.Clear();

        // 调用 GridManager 的方法，寻找敌方攻击力最低的棋子
        Entity target = GridManager.gridManager.FindOpponentWithMinATK(_owner.chesstype);

        // 如果找不到合适的目标，则释放失败
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

        // 遍历所有目标并应用效果
        foreach (Entity _target in targets)
        {
            // 让目标接收战斗事件，处理扣血逻辑
            _target.GetBattleEvent(skillEffect);
            
            // 在控制台输出日志，方便调试和观察战斗过程
            Debug.Log(_owner.gameObject.name + " 对 " + _target.gameObject.name + " 释放了 " + skillName + "，造成 " + damage + " 点伤害");
        }

        // 返回 true 表示技能成功执行
        return true;
    }
}