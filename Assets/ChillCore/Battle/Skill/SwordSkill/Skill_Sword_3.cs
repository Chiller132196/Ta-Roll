using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// P-Swords-3 技能：凌厉突刺
/// 特性：快频直取前排，HP风系最高支撑持续作战
/// </summary>
public class Skill_Sword_3 : Skill
{
    public string skillName = "凌厉突刺";

    internal override bool CastSkill(Entity _owner)
    {
        targets.Clear();

        // 寻找敌方最前排的棋子
        Entity target = GridManager.gridManager.FindAnyOpponentFrontChess(_owner.chesstype);

        if (target == null)
        {
            return false;
        }

        targets.Add(target);

        // 计算伤害：基础攻击力 * 1.0 倍率
        int damage = (int)(_owner.battleATK * 1.0f);

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