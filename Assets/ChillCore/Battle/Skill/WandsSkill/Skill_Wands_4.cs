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

        // 寻找敌方后排 (X 最大) 的棋子
        Entity backRowTarget = FindAnyOpponentBackChess(_owner.chesstype);

        if (backRowTarget == null)
        {
            return false;
        }

        targets.Add(backRowTarget);

        // 计算伤害: ATK * 2.0 倍率，直接强转 int
        int damage = (int)(_owner.battleATK * 2.0f);

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

    /// <summary>
    /// 查找敌方后排 (X 坐标最大) 的任意一个棋子
    /// </summary>
    /// <param name="_ownerSide">拥有者（己方）的阵营</param>
    /// <returns>敌方后排的一个棋子，如果没有则返回 null</returns>
    private Entity FindAnyOpponentBackChess(Chesstype _ownerSide)
    {
        Entity target = null;
        int max_X = -1; // 初始化为无效值

        foreach (ChessGrid grid in GridManager.gridManager.chessGrids)
        {
            // 检查格子上是否有棋子，且是否为敌方棋子
            if (grid.HasChess() && grid.chesstype != _ownerSide)
            {
                // 更新找到的最大 X 坐标和对应棋子
                if (grid.posX > max_X)
                {
                    max_X = grid.posX;
                    target = grid.chess.GetComponent<Entity>();
                }
            }
        }

        return target;
    }
}
// ... existing code ...