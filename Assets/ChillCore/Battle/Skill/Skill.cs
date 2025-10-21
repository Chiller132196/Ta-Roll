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
    /// 技能的激活状态
    /// </summary>
    private bool isActived;

    /// <summary>
    /// 请求释放技能
    /// </summary>
    internal virtual bool OnCastSkill(Entity _owner)
    {


        return isActived;
    }

    /// <summary>
    /// 技能准备释放
    /// </summary>
    internal virtual void OnSkillActived()
    {

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
