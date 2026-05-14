using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// P-Swords-4 技能：乱刃斩
/// 特性：专打最弱攻击者，1.5倍快速清除；ATK平局时优先选生命值最低者
/// </summary>
public class Skill_Sword_4 : Skill
{
    public string skillName = "乱刃斩";

    internal override bool CastSkill(Entity _owner)
    {
        targets.Clear();

        // 寻找敌方攻击力最低的棋子（平局时优先选生命值最低者）
        Entity target = GridManager.gridManager.FindOpponentWithMinATK(_owner.chesstype);

        if (target == null)
        {
            return false;
        }

        targets.Add(target);

        // 计算伤害：基础攻击力 * 1.5 倍率
        int damage = (int)(_owner.battleATK * 1.5f);

        BattleEvent skillEffect = new BattleEvent();
        skillEffect.deltaHP = -damage;
        skillEffect.owner = _owner;

        foreach (Entity _target in targets)
        {
            _target.GetBattleEvent(skillEffect);
            Debug.Log(_owner.gameObject.name + " 对 " + _target.gameObject.name + " 释放了 " + skillName + "，造成 " + damage + " 点伤害");
        }

        return true;
    }
}