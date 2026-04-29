using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    /// <summary>
    /// 技能的释放者
    /// </summary>
    public Entity owner;

    /// <summary>
    /// 技能的目标
    /// </summary>
    public List<Entity> targets;

    /// <summary>
    /// 技能花费的生命值
    /// </summary>
    public int healthCost;

    /// <summary>
    /// 技能所需的能量
    /// </summary>
    public int mpCost;

    /// <summary>
    /// 可以释放的次数，-1代表无次数限制
    /// </summary>
    public int skillCounter;

    /// <summary>
    /// 技能被激活了几次
    /// </summary>
    public int requestTime;

    public Skill()
    {
        skillCounter = -1;
    }

    /// <summary>
    /// 请求释放技能
    /// </summary>
    internal virtual bool OnCastSkill(Entity _owner)
    {
        // 技能使用次数用完
        if (skillCounter == 0)
        {
            return false;
        }

        // 仅释放者可以支付费用时释放
        if (CheckOwner(_owner))
        {
            CastSkill();

            CostOwner(_owner);

            return true;
        }

        else
        {
            return false;
        }

    }

    #region 支持重写部分

    /// <summary>
    /// 检查棋子是否可以支付释放此技能的费用
    /// </summary>
    /// <param name="_owner"></param>
    /// <returns></returns>
    internal virtual bool CheckOwner(Entity _owner)
    {
        if (owner.battleHP < healthCost)
        {
            return false;
        }
        else if (owner.battleMP < mpCost)
        {
            return false;
        }

        return true;
    }

    internal virtual BattleEvent CheckCost()
    {
        BattleEvent skillCost = new BattleEvent();

        skillCost.deltaHP = healthCost;
        skillCost.deltaMP = mpCost;

        return skillCost;
    }

    internal virtual void CostOwner(Entity _owner)
    {
        BattleEvent skillCost = CheckCost();

        owner.GetBattleEvent(skillCost);
    }

    /// <summary>
    /// 技能释放
    /// </summary>
    /// <param name="_owner">释放者</param>
    internal virtual bool CastSkill()
    {
        Debug.Log(gameObject.name + " 技能的效果为空，需要开发者重写！");

        return false;
    }

    /// <summary>
    /// 技能结束时的效果
    /// </summary>
    internal virtual void OnSkillEnd()
    {

    }

    #endregion

    /// <summary>
    /// 移除自身以及实体
    /// </summary>
    public virtual void DestroySkill()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {

    }

    void Update()
    {
        if (requestTime > 0)
        {

        }
    }
}
