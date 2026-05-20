using UnityEngine;

/// <summary>
/// 棋子悬浮提示触发器
/// 功能：鼠标悬停在棋子上时，显示信息面板；鼠标移开时隐藏面板
/// </summary>
public class ChessTooltipTrigger : MonoBehaviour
{
    // 存储当前棋子的实体数据（血量、攻击、技能等信息）
    private Entity entity;

    /// <summary>
    /// 游戏启动时自动执行
    /// 作用：找到当前物体身上的 Entity 脚本
    /// </summary>
    private void Awake()
    {
        // 先尝试从自己身上获取 Entity 组件
        entity = GetComponent<Entity>();

        // 如果自己身上没有，就去父物体身上找
        if (entity == null)
        {
            entity = GetComponentInParent<Entity>();
        }
    }

    /// <summary>
    /// 当鼠标 进入 棋子区域时自动调用
    /// 作用：显示悬浮信息面板
    /// </summary>
    private void OnMouseEnter()
    {
        // 只有当 Entity 存在 + 提示管理器存在时，才执行显示
        if (entity != null && TooltipManager.Instance != null)
        {
            // 调用提示管理器，把当前棋子的信息传过去并显示面板
            TooltipManager.Instance.Show(entity);
        }
    }

    /// <summary>
    /// 当鼠标 离开 棋子区域时自动调用
    /// 作用：隐藏悬浮信息面板
    /// </summary>
    private void OnMouseExit()
    {
        // 只有提示管理器存在时，才执行隐藏
        if (TooltipManager.Instance != null)
        {
            // 隐藏悬浮面板
            TooltipManager.Instance.Hide();
        }
    }
}