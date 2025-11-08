using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    /// <summary>
    /// 技能的释放者
    /// </summary>
    private Entity owner;

    /// <summary>
    /// 技能的目标
    /// </summary>
    private Entity target;

    /// <summary>
    /// 多目标时，访问此变量
    /// </summary>
    private List<Entity> targets;

    /// <summary>
    /// 技能所需的能量
    /// </summary>
    private int energyCost;

    /// <summary>
    /// 限制技能冷却时用
    /// </summary>
    private int skillTimer;

    /// <summary>
    /// 限制次数时用，-1代表无次数限制
    /// </summary>
    private int skillCounter;

    /// <summary>
    /// 技能的激活状态
    /// </summary>
    private bool isActived;

    public Skill()
    {
        skillTimer = -1;

        skillCounter = -1;
    }

    /// <summary>
    /// 请求释放技能
    /// </summary>
    internal virtual bool OnCastSkill()
    {
        BattleEvent skillCost = new BattleEvent();

        owner.GetBattleEvent(skillCost);

        return isActived;
    }

    /// <summary>
    /// 技能释放
    /// </summary>
    /// <param name="_owner">释放者</param>
    internal virtual void SkillActive(Entity _owner)
    {
        BattleEvent skillEffect = new BattleEvent();

    }

    /// <summary>
    /// 技能结束时的效果
    /// </summary>
    internal virtual void OnSkillEnd()
    {

    }

    private void Awake()
    {
        isActived = false;
    }

    void Update()
    {
        
    }
}
