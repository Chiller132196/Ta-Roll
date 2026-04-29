using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Sword_1 : Skill
{
    public string skillName;

    internal override bool CastSkill(Entity _owner)
    {
        targets.Clear();

        targets.Add(GridManager.gridManager.FindAnyFrontChess(Chesstype.Enemy));

        if (targets.Count <= 0)
        {
            Debug.Log(gameObject.name + "无法找到合适的目标！");
            return false;
        }

        BattleEvent originSkillEffect = new BattleEvent();
        originSkillEffect.deltaHP = _owner.battleATK * -1;
        originSkillEffect.owner = _owner;

        foreach(Entity _target in targets)
        {
            // 如果目标有弱点，造成两倍伤害
            if (_target.hasFlaw)
            {
                BattleEvent skillEffect = new BattleEvent();

                skillEffect.deltaATK = originSkillEffect.deltaHP * 2;
                skillEffect.consumedFlaw = true;

                _target.GetBattleEvent(skillEffect);
            }

            // 如果目标没有弱点，则无特殊效果
            else
            {
                _target.GetBattleEvent(originSkillEffect);
            }

            Debug.Log(_owner.gameObject.name + " 对 " + _target.gameObject.name + "  释放了 " + skillName);
        }

        return true;
    }

}
