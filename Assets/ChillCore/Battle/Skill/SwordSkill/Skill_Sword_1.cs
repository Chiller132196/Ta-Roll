using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Sword_1 : Skill
{
    public string skillName;

    internal override bool CastSkill(Entity _owner)
    {
        targets.Clear();

        targets.Add(GridManager.gridManager.FindAnyOpponentFrontChess(_owner.chesstype));

        Debug.Log(gameObject.name + "???????????? " + _owner.chesstype + " ??????");

        if (targets.Count <= 0 || targets.Contains(null))
        {
            //Debug.Log(gameObject.name + "??????????????");

            return false;
        }

        BattleEvent originSkillEffect = new BattleEvent();
        originSkillEffect.deltaHP = _owner.battleATK * -1;
        originSkillEffect.owner = _owner;

        foreach (Entity _target in targets)
        {
            //Debug.Log(gameObject.name + " ????? " + _target.gameObject.name + " ??????");

            // ??????????????????????
            if (_target.hasFlaw)
            {
                BattleEvent skillEffect = new BattleEvent();

                skillEffect.deltaHP = originSkillEffect.deltaHP * 2;
                skillEffect.consumedFlaw = true;

                _target.GetBattleEvent(skillEffect);
            }

            // ?????????????????????��??
            else
            {
                _target.GetBattleEvent(originSkillEffect);
            }

            Debug.Log(_owner.gameObject.name + " ?? " + _target.gameObject.name + "  ????? " + skillName);
        }

        return true;
    }

}
