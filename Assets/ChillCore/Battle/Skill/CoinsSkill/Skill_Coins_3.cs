using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// P-Coins-3 技能：岩盾反击
/// 特性：自带破绽，HP极高抗风险；慢充能换2倍重击最强敌人；平局优先选生命值最低者
/// </summary>
public class Skill_Coins_3 : Skill
{
    public string skillName = "岩盾反击";

    internal override bool CastSkill(Entity _owner)
    {
        targets.Clear();

        Entity target = GridManager.gridManager.FindOpponentWithMaxATK(_owner.chesstype);

        if (target == null)
        {
            return false;
        }

        targets.Add(target);

        int damage = (int)(_owner.battleATK * 2.0f);

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