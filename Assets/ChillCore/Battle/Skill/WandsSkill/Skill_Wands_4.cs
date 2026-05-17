// ... existing code ...
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Wands_4 : Skill
{
    public string skillName = "爆裂弹";

    internal override bool CastSkill(Entity _owner)
    {
        targets.Clear();

        Entity backRowTarget = GridManager.gridManager.FindAnyOpponentBackChess(_owner.chesstype);

        if (backRowTarget == null)
        {
            return false;
        }

        targets.Add(backRowTarget);

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
// ... existing code ...