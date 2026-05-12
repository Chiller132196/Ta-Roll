// ... existing code ...
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Wands_2 : Skill
{
    public string skillName = "焚身咒";

    internal override bool CastSkill(Entity _owner)
    {
        // HP消耗：先对自己造成3点伤害
        BattleEvent selfCost = new BattleEvent();
        selfCost.deltaHP = -3;
        _owner.GetBattleEvent(selfCost);

        targets.Clear();

        // 寻找敌方攻击力最高的棋子 (修改后的逻辑)
        Entity highestATK = null;
        foreach (Entity e in GridManager.gridManager.GetAllOpponents(_owner.chesstype))
        {
            if (highestATK == null || e.battleATK > highestATK.battleATK) // <--- 改为比较 battleATK，找最大值
                highestATK = e;
        }

        if (highestATK == null)
        {
            return false;
        }

        targets.Add(highestATK);

        // 计算伤害: ATK * 1.0 倍率，直接强转 int
        int damage = (int)(_owner.battleATK * 1.0f);

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
// ... existing code ...