using UnityEngine;
using TMPro;

/// <summary>
/// 棋子悬浮提示面板UI脚本
/// 负责填充所有棋子信息、跟随鼠标移动、显示/隐藏面板
/// </summary>
public class TooltipPanel : MonoBehaviour
{
    [Header("文本组件绑定")]
    // 棋子名称文本
    public TMP_Text chessNameText;
    // 元素类型文本
    public TMP_Text elementText;
    // 血量文本
    public TMP_Text hpText;
    // 攻击力文本
    public TMP_Text atkText;
    // 充能速度文本
    public TMP_Text chargeSpeedText;
    // 技能效果描述文本
    public TMP_Text skillDescriptionText;

    [Header("面板偏移距离")]
    // 面板相对于鼠标的偏移位置，避免挡住鼠标
    public Vector2 mouseOffset = new Vector2(20f, -20f);

    // 面板矩形位置组件
    private RectTransform rectTransform;
    // 父级画布组件
    private Canvas parentCanvas;

    private void Awake()
    {
        // 获取自身矩形变换组件
        rectTransform = GetComponent<RectTransform>();
        // 获取上层画布
        parentCanvas = GetComponentInParent<Canvas>();
        // 初始化直接隐藏面板
        Hide();
    }

    private void Update()
    {
        // 面板显示状态下，实时跟随鼠标
        if (gameObject.activeSelf)
        {
            FollowMouse();
        }
    }

    /// <summary>
    /// 显示提示面板并填充所有棋子数据
    /// </summary>
    /// <param name="entity">棋子实体数据</param>
    public void Show(Entity entity)
    {
        // 赋值棋子名称
        chessNameText.text = entity.gameObject.name;
        // 赋值元素中文名称
        elementText.text = "元素：" + GetElementName(entity.chessElement);
        // 赋值当前血量/最大血量
        hpText.text = "血量：" + entity.battleHP + " / " + entity.maxHP;
        // 赋值攻击力
        atkText.text = "攻击：" + entity.battleATK;
        // 赋值充能速度
        chargeSpeedText.text = "充能速度：" + entity.battleChargeSpeed;
        // 赋值技能效果描述
        skillDescriptionText.text = "技能效果：" + GetSkillDescription(entity);

        // 激活显示面板
        gameObject.SetActive(true);
        // 立刻对齐鼠标位置
        FollowMouse();
    }

    /// <summary>
    /// 隐藏悬浮提示面板
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 让面板实时跟随鼠标移动
    /// 适配屏幕画布与相机模式
    /// </summary>
    private void FollowMouse()
    {
        // 空值拦截，防止报错
        if (rectTransform == null || parentCanvas == null)
        {
            return;
        }

        // 获取画布矩形
        RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();
        // 判断画布渲染模式，获取对应相机
        Camera eventCamera = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;

        // 将鼠标屏幕坐标转为画布内局部坐标
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, eventCamera, out Vector2 localPoint))
        {
            // 设置面板位置 + 偏移量
            rectTransform.anchoredPosition = localPoint + mouseOffset;
        }
    }

    /// <summary>
    /// 获取棋子技能描述文本
    /// </summary>
    private string GetSkillDescription(Entity entity)
    {
        // 没有技能直接返回无
        if (entity.skill == null)
        {
            return "无";
        }
        // 获取技能基类脚本
        Skill skill = entity.skill.GetComponent<Skill>();
        if (skill == null)
        {
            return "暂无描述";
        }
        // 调用技能内方法拿到描述
        return skill.GetDescription();
    }

    /// <summary>
    /// 枚举元素转为中文文字
    /// </summary>
    private string GetElementName(ChessElement element)
    {
        switch (element)
        {
            case ChessElement.Wind:
                return "风";
            case ChessElement.Water:
                return "水";
            case ChessElement.Fire:
                return "火";
            case ChessElement.Soil:
                return "土";
            default:
                return "无";
        }
    }
}