using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// P-Cups-4 技能：深渊涌现
/// 特性：锁定最脆弱目标（HP最低）补刀；若目标有破绽则触发×2爆发并消耗破绽；平局优先选ATK最低者
/// </summary>
public class Skill_Cups_4 : Skill
{
    // 技能名称
    public string skillName = "深渊涌现";

    /// <summary>
    /// 释放技能的核心逻辑
    /// </summary>
    /// <param name="_owner">施法者实体</param>
    /// <returns>是否成功释放</returns>
    internal override bool CastSkill(Entity _owner)
    {
        // 清空之前的目标列表
        targets.Clear();

        // 调用 GridManager 的方法，寻找敌方生命值最低的棋子
        Entity target = GridManager.gridManager.FindOpponentWithMinHP(_owner.chesstype);

        // 如果找不到合适的目标，则释放失败
        if (target == null)
        {
            return false;
        }

        // 将找到的目标添加到本次技能的目标列表中
        targets.Add(target);

        // 计算基础伤害：基础攻击力 * 1.0 倍率
        int baseDamage = (int)(_owner.battleATK * 1.0f);
        
        // 创建战斗事件对象
        BattleEvent skillEffect = new BattleEvent();
        skillEffect.owner = _owner; // 记录伤害来源

        // 遍历所有目标并应用效果
        foreach (Entity _target in targets)
        {
            // 检查目标是否具有“破绽”状态
            if (_target.hasFlaw)
            {
                // 如果有破绽：造成双倍伤害，并标记需要消耗破绽
                skillEffect.deltaHP = -(baseDamage * 2);
                skillEffect.consumedFlaw = true;
            }
            else
            {
                // 如果没有破绽：造成基础伤害，不消耗破绽
                skillEffect.deltaHP = -baseDamage;
                skillEffect.consumedFlaw = false;
            }

            // 让目标接收战斗事件，处理扣血和破绽状态的变更
            _target.GetBattleEvent(skillEffect);
            
            // 计算实际造成的伤害值用于日志输出
            int actualDamage = _target.hasFlaw ? baseDamage * 2 : baseDamage;
            Debug.Log(_owner.gameObject.name + " 对 " + _target.gameObject.name + " 释放了 " + skillName + "，造成 " + actualDamage + " 点伤害");
        }

        // 返回 true 表示技能成功执行
        return true;
    }
}