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
    public Entity target;

    /// <summary>
    /// 多目标时，访问此变量
    /// </summary>
    public List<Entity> targets;

    /// <summary>
    /// 技能所需的能量
    /// </summary>
    public int energyCost;

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
    internal virtual BattleEvent OnCastSkill()
    {
        // 技能使用次数用完
        if (skillCounter == 0)
        {
            return null;
        }

        BattleEvent skillCost = new BattleEvent();

        CastSkill(owner);

        return skillCost;
    }

    /// <summary>
    /// 技能释放
    /// </summary>
    /// <param name="_owner">释放者</param>
    internal virtual void CastSkill(Entity _owner)
    {
        BattleEvent skillEffect = new BattleEvent();

    }

    /// <summary>
    /// 移除自身以及实体
    /// </summary>
    public virtual void DestroySkill()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 技能结束时的效果
    /// </summary>
    internal virtual void OnSkillEnd()
    {

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
