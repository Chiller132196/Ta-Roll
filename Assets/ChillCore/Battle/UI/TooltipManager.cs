using UnityEngine;

/// <summary>
/// 全局悬浮提示管理器（单例）
/// 统一控制棋子悬浮信息面板的显示与隐藏
/// </summary>
public class TooltipManager : MonoBehaviour
{
    // 全局唯一实例，外部直接通过TooltipManager.Instance调用
    public static TooltipManager Instance { get; private set; }

    // 拖拽赋值：场景中做好的悬浮提示面板物体
    public TooltipPanel tooltipPanel;

    private void Awake()
    {
        // 单例防重复：如果已经存在实例且不是自己，直接销毁多余物体
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 把当前对象设为全局唯一实例
        Instance = this;

        // 游戏一开始默认隐藏提示面板
        if (tooltipPanel != null)
        {
            tooltipPanel.Hide();
        }
    }

    /// <summary>
    /// 显示悬浮提示
    /// </summary>
    /// <param name="entity">传入当前选中棋子的实体数据</param>
    public void Show(Entity entity)
    {
        // 判空防护，避免空报错
        if (tooltipPanel == null || entity == null)
        {
            return;
        }
        // 调用面板自身方法，传入数据并刷新显示
        tooltipPanel.Show(entity);
    }

    /// <summary>
    /// 隐藏悬浮提示面板
    /// </summary>
    public void Hide()
    {
        if (tooltipPanel != null)
        {
            tooltipPanel.Hide();
        }
    }
}