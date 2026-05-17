using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// P-Swords-5 技能：破空刺
/// 特性：自带破绽，消耗3HP刺穿棋盘中心；(2,2)空时依次检查(2,3)(2,1)(1,2)(3,2)，全空不释放；有破绽触发×2=8点
/// </summary>
public class Skill_Sword_5 : Skill
{
    public string skillName = "破空刺";

    internal override bool CastSkill(Entity _owner)
    {
        targets.Clear();

        // 定义优先检查的坐标列表
        int[] checkX = { 2, 2, 2, 1, 3 };
        int[] checkY = { 2, 3, 1, 2, 2 };

        Entity target = null;

        // 按优先级检查每个坐标
        for (int i = 0; i < checkX.Length; i++)
        {
            GameObject chessObj = GridManager.gridManager.GetGridChessByXY(checkX[i], checkY[i], GetOpponentSide(_owner.chesstype));

            if (chessObj != null)
            {
                Entity entity = chessObj.GetComponent<Entity>();
                if (entity != null && entity.isAlive)
                {
                    target = entity;
                    break;
                }
            }
        }

        // 如果所有坐标都为空或目标已死亡，技能释放失败
        if (target == null)
        {
            Debug.Log(_owner.gameObject.name + " 释放 " + skillName + " 失败：目标位置均为空或目标已死亡");
            return false;
        }

        targets.Add(target);

        // 计算基础伤害
        int baseDamage = (int)(_owner.battleATK * 1.0f);

        BattleEvent skillEffect = new BattleEvent();
        skillEffect.owner = _owner;

        foreach (Entity _target in targets)
        {
            // 检查目标是否有破绽
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

    /// <summary>
    /// 获取敌方阵营类型
    /// </summary>
    private Chesstype GetOpponentSide(Chesstype _mySide)
    {
        return _mySide == Chesstype.Player ? Chesstype.Enemy : Chesstype.Player;
    }
}