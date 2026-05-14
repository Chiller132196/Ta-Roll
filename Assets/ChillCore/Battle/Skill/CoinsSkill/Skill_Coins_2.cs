using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// P-Coins-2 技能：大地裂
/// 特性：全场最高HP，输出极弱，专盯最厚目标做长线消耗；平局优先选ATK最高者
/// </summary>
public class Skill_Coins_2 : Skill
{
    public string skillName = "大地裂";

    internal override bool CastSkill(Entity _owner)
    {
        targets.Clear();

        Entity target = GridManager.gridManager.FindOpponentWithMaxHP(_owner.chesstype);

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