using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// P-Coins-4 技能：碎岩击
/// 特性：锁定最脆弱目标；有破绽触发×2=4点，土系唯一爆发机会；平局优先选ATK最低者
/// </summary>
public class Skill_Coins_4 : Skill
{
    public string skillName = "碎岩击";

    internal override bool CastSkill(Entity _owner)
    {
        targets.Clear();

        Entity target = GridManager.gridManager.FindOpponentWithMinHP(_owner.chesstype);

        if (target == null)
        {
            return false;
        }

        targets.Add(target);

        int baseDamage = (int)(_owner.battleATK * 1.0f);

        BattleEvent skillEffect = new BattleEvent();
        skillEffect.owner = _owner;

        foreach (Entity _target in targets)
        {
            if (_target.hasFlaw)
            {
                skillEffect.deltaHP = -(baseDamage * 2);
                skillEffect.consumedFlaw = true;
            }
            else
            {
                skillEffect.deltaHP = -baseDamage;
                skillEffect.consumedFlaw = false;
            }

            _target.GetBattleEvent(skillEffect);

            int actualDamage = _target.hasFlaw ? baseDamage * 2 : baseDamage;
            Debug.Log(_owner.gameObject.name + " 对 " + _target.gameObject.name + " 释放了 " + skillName + "，造成 " + actualDamage + " 点伤害");
        }

        return true;
    }
}