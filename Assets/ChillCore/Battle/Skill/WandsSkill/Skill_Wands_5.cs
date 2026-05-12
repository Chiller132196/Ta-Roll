// ... existing code ...
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Wands_5 : Skill
{
    public string skillName = "破绽点燃";

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

        // 计算基础伤害: ATK * 1.0 倍率，直接强转 int
        int baseDamage = (int)(_owner.battleATK * 1.0f);

        BattleEvent skillEffect = new BattleEvent();
        skillEffect.owner = _owner;

        foreach (Entity _target in targets)
        {
            // 检查目标是否有破绽
            if (_target.hasFlaw)
            {
                // 目标有破绽：造成双倍伤害，并标记消耗破绽
                int flawDamage = baseDamage * 2;
                skillEffect.deltaHP = -flawDamage; // 使用 deltaHP 造成双倍伤害
                skillEffect.consumedFlaw = true; // 标记破绽被消耗
            }
            else
            {
                // 目标无破绽：造成基础伤害
                skillEffect.deltaHP = -baseDamage; // 使用 deltaHP 造成基础伤害
                skillEffect.consumedFlaw = false; // 明确标记未消耗破绽 (非必需，但清晰起见)
            }

            _target.GetBattleEvent(skillEffect);

            int actualDamage = _target.hasFlaw ? baseDamage * 2 : baseDamage;
            Debug.Log(_owner.gameObject.name + " 对 " + _target.gameObject.name + " 释放了 " + skillName + "，造成 " + actualDamage + " 点伤害 (目标HasFlaw: " + _target.hasFlaw + ")");
        }

        return true;
    }
}
// ... existing code ...