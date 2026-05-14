using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// P-Swords-2 技能：旋风刃
/// 特性：自带破绽，消耗2HP加速；精准打击生命值最低目标1.5倍收割；平局优先选ATK最高者（先清威胁）
/// </summary>
public class Skill_Sword_2 : Skill
{
    public string skillName = "旋风刃";

    internal override bool CastSkill(Entity _owner)
    {
        targets.Clear();

        // 调用 GridManager 的方法，寻找敌方生命值最低的棋子（平局选ATK最高）
        Entity target = GridManager.gridManager.FindOpponentWithMinHP(_owner.chesstype);

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