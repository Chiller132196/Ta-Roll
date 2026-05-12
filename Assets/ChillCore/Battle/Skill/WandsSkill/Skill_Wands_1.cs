using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Wands_1 : Skill
{
    public string skillName = "烈焰冲";

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

        // 计算伤害: ATK * 1.5 倍率，直接强转 int
        int damage = (int)(_owner.battleATK * 1.5f);

        BattleEvent skillEffect = new BattleEvent();
        skillEffect.deltaHP = -damage; // 负数表示扣血
        skillEffect.owner = _owner;

        foreach (Entity _target in targets)
        {
            _target.GetBattleEvent(skillEffect);
            Debug.Log(_owner.gameObject.name + " 对 " + _target.gameObject.name + " 释放了 " + skillName + "，造成 " + damage + " 点伤害");
        }

        return true;
    }
}