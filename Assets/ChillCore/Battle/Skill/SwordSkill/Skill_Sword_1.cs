using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Sword_1 : Skill
{
    internal override bool CastSkill()
    {
        targets.Clear();

        GridManager.gridManager.FindAnyFrontChess(Chesstype.Enemy);



        return false;
    }




}
