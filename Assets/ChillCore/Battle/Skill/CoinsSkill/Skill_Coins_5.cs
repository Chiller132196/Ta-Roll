using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// P-Coins-5 技能：黄金护体
/// 特性：充能最快的土系，直取前排
/// </summary>
public class Skill_Coins_5 : Skill
{
    public string skillName = "黄金护体";

    internal override bool CastSkill(Entity _owner)
    {
        targets.Clear();

        Entity target = GridManager.gridManager.FindAnyOpponentFrontChess(_owner.chesstype);

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