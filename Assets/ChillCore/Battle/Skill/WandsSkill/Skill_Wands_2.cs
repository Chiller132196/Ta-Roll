// ... existing code ...
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Wands_2 : Skill
{
    public string skillName = "焚身咒";

    internal override bool CastSkill(Entity _owner)
    {
        BattleEvent selfCost = new BattleEvent();
        selfCost.deltaHP = -3;
        _owner.GetBattleEvent(selfCost);

        targets.Clear();

        Entity highestATK = GridManager.gridManager.FindOpponentWithMaxATK(_owner.chesstype);

        if (highestATK == null)
        {
            return false;
        }

        targets.Add(highestATK);

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
// ... existing code ...