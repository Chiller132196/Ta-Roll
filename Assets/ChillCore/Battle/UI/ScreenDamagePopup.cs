using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 屏幕伤害飘字管理器 (2D 纯UI版)
/// 功能：将3D世界位置的伤害，转换成屏幕UI文字，向上飘出并渐隐
/// 特点：自动创建Canvas、不占场景物体、全屏显示、永远最上层
/// </summary>
public class ScreenDamagePopup : MonoBehaviour
{
    // 静态全局Canvas，所有飘字共用一个画布
    private static Canvas popupCanvas;
    // 默认字体（备用）
    private static Font defaultFont;

    // 当前飘字的文本组件
    private TextMeshProUGUI damageText;
    // 当前飘字的矩形变换组件（控制位置大小）
    private RectTransform rectTransform;
    // 动画起始的锚点位置
    private Vector2 startPosition;

    /// <summary>
    /// 【静态调用方法】显示伤害飘字（外部唯一入口）
    /// </summary>
    /// <param name="damageAmount">伤害数值</param>
    /// <param name="worldPosition">3D世界中的目标位置（角色头顶）</param>
    public static void Show(int damageAmount, Vector3 worldPosition, bool isBreakDamage = false)
    {
        // 获取主相机
        Camera mainCamera = Camera.main;
        // 获取或创建全局UI画布
        Canvas canvas = GetOrCreateCanvas();
        // 将3D世界坐标 → 转换为 2D屏幕坐标
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // 如果物体在相机背后（Z<0），不显示飘字
        if (screenPosition.z < 0f)
        {
            return;
        }

        // 限制飘字位置，防止跑到屏幕外面去
        screenPosition.x = Mathf.Clamp(screenPosition.x, 40f, Screen.width - 40f);
        screenPosition.y = Mathf.Clamp(screenPosition.y, 40f, Screen.height - 40f);

        // 新建一个GameObject作为飘字载体
        GameObject popupObject = new GameObject("ScreenDamagePopup", typeof(RectTransform));
        // 设置父物体为全局Canvas，保持UI层级
        popupObject.transform.SetParent(canvas.transform, false);

        // 添加自身脚本，并初始化显示内容
        ScreenDamagePopup popup = popupObject.AddComponent<ScreenDamagePopup>();
        popup.Initialize(damageAmount, screenPosition, isBreakDamage);
    }

    /// <summary>
    /// 获取全局唯一的Canvas，如果没有就自动创建
    /// </summary>
    private static Canvas GetOrCreateCanvas()
    {
        // 如果已经创建过画布，直接返回
        if (popupCanvas != null)
            return popupCanvas;

        // 新建Canvas物体
        GameObject canvasObject = new GameObject("ScreenDamagePopupCanvas");
        popupCanvas = canvasObject.AddComponent<Canvas>();
        
        // 设置为【屏幕覆盖模式】，永远显示在最上层
        popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // 设置优先级为999，确保在所有UI之上
        popupCanvas.sortingOrder = 999;

        // 添加画布缩放适配，适应不同分辨率
        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        // 添加UI射线检测（虽然这里不需要，但标准Canvas必备）
        canvasObject.AddComponent<GraphicRaycaster>();
        // 切换场景不销毁这个画布
        DontDestroyOnLoad(canvasObject);

        return popupCanvas;
    }

    /// <summary>
    /// 初始化单个飘字的样式、位置、文本
    /// </summary>
    private void Initialize(int damageAmount, Vector2 screenPosition, bool isBreakDamage)
    {
        // 获取RectTransform组件
        rectTransform = GetComponent<RectTransform>();
        // 设置文本框大小
        rectTransform.sizeDelta = new Vector2(160f, 80f);
        // 设置屏幕位置，并加一点随机偏移，防止数字重叠
        rectTransform.position = screenPosition + new Vector2(Random.Range(-25f, 25f), Random.Range(-8f, 12f));
        // 记录起始位置
        startPosition = rectTransform.anchoredPosition;

        // 添加TMP文本组件
        damageText = gameObject.AddComponent<TextMeshProUGUI>();
        // 显示伤害数字
       damageText.text = isBreakDamage ? $"{damageAmount}<size=35> BREAK!</size>" : damageAmount.ToString();
        // 字体大小
        damageText.fontSize = 70f;
        // 加粗
        damageText.fontStyle = FontStyles.Bold;
        // 居中对齐
        damageText.alignment = TextAlignmentOptions.Center;
        // 白色文字
        damageText.color = Color.red;
        // 关闭射线投射，不阻挡点击
        damageText.raycastTarget = false;

        // 添加黑色描边，让文字更清晰
        Outline outline = gameObject.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(2f, -2f);

        // 开始播放动画
        StartCoroutine(PlayAnimation());
    }

    /// <summary>
    /// 飘字动画：向上移动 + 渐隐
    /// </summary>
    private IEnumerator PlayAnimation()
    {
        // 动画持续时间
        float lifeTime = 1.4f;
        // 向上移动的距离（像素）
        float moveDistance = 70f;
        // 已消耗时间
        float elapsed = 0f;
        // 记录初始颜色
        Color startColor = damageText.color;

        // 在生命周期内每帧执行
        while (elapsed < lifeTime)
        {
            elapsed += Time.deltaTime;
            // 计算动画进度 0~1
            float progress = Mathf.Clamp01(elapsed / lifeTime);

            // 更新位置：向上移动
            rectTransform.anchoredPosition = startPosition + Vector2.up * (moveDistance * progress);
            // 更新透明度：随时间逐渐消失
            damageText.color = new Color(startColor.r, startColor.g, startColor.b, 1f - progress);

            // 等待下一帧
            yield return null;
        }

        // 动画结束，销毁自己
        Destroy(gameObject);
    }
}