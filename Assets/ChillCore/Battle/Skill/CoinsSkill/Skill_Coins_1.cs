using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// P-Coins-1 技能：石壁压
/// 特性：如山挡住最强威胁，专盯ATK最高敌人；平局优先选生命值最低者
/// </summary>
public class Skill_Coins_1 : Skill
{
    public string skillName = "石壁压";

    internal override bool CastSkill(Entity _owner)
    {
        targets.Clear();

        Entity target = GridManager.gridManager.FindOpponentWithMaxATK(_owner.chesstype);

        if (target == null)
        {
            return false;
        }

        targets.Add(target);

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