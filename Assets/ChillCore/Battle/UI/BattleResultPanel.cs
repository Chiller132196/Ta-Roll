using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 战斗结算面板（核心功能）
/// 功能：
/// 1. 战斗胜利 / 失败时弹出界面
/// 2. 显示对应文字、颜色、回合数
/// 3. 播放弹出动画（渐显 + 缩放）
/// 4. 点击按钮返回主菜单
/// 5. 自动修复引用，防止报错
/// </summary>
public class BattleResultPanel : MonoBehaviour
{
    [Header("-------- 面板结构绑定 --------")]
    // 整个面板的根物体（控制显示/隐藏）
    public GameObject root;
    // 画布组：控制整体透明度、是否可点击
    public CanvasGroup canvasGroup;
    // 中间弹窗的盒子（用来做缩放动画）
    public RectTransform resultBox;

    [Header("-------- 文本显示 --------")]
    // 大标题：VICTORY / DEFEAT
    public TMP_Text titleText;
    // 提示信息：战斗胜利！/ 战斗失败
    public TMP_Text messageText;
    // 回合数显示
    public TMP_Text roundText;

    [Header("-------- 按钮 --------")]
    // 返回主界面按钮
    public Button backToMainButton;

    [Header("-------- 场景设置 --------")]
    // 要返回的主菜单场景名（可在Inspector修改）
    public string mainMenuSceneName = "StartMenu";



    // 正在播放的动画协程（用来防止重复播放）
    private Coroutine showCoroutine;

    /// <summary>
    /// 唤醒时执行（游戏一开始就运行）
    /// 作用：自动补全引用、绑定按钮事件、隐藏面板
    /// </summary>
    private void Awake()
    {
        // 如果没指定根物体，就用自己这个物体
        if (root == null)
        {
            root = gameObject;
        }

        // 如果没拖CanvasGroup，自动从物体上获取
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // 如果没设置弹窗盒子，就把自己当作盒子
        if (resultBox == null)
        {
            resultBox = transform as RectTransform;
        }

        // 给返回按钮绑定点击事件（自动绑，不用在UI里手动设置）
        if (backToMainButton != null)
        {
            // 先清空旧事件，防止重复绑定
            backToMainButton.onClick.RemoveListener(BackToMainMenu);
            backToMainButton.onClick.AddListener(BackToMainMenu);
        }

        // 一开始必须隐藏面板
        HideImmediate();
    }

    /// <summary>
    /// 外部调用：显示胜利界面
    /// </summary>
    /// <param name="round">当前回合数</param>
    public void ShowVictory(int round)
    {
        Show(true, round);
    }

    /// <summary>
    /// 外部调用：显示失败界面
    /// </summary>
    /// <param name="round">当前回合数</param>
    public void ShowDefeat(int round)
    {
        Show(false, round);
    }

    /// <summary>
    /// 统一显示逻辑（根据胜利/失败设置内容）
    /// </summary>
    /// <param name="isVictory">true=胜利，false=失败</param>
    /// <param name="round">回合数</param>
    public void Show(bool isVictory, int round)
    {
        // 确保根物体存在
        if (root == null)
            root = gameObject;

        // 激活面板（显示出来）
        root.SetActive(true);

        // 设置标题文字 + 颜色
        if (titleText != null)
        {
            titleText.text = isVictory ? "VICTORY!" : "DEFEAT!";
        }

        // 设置提示信息
        if (messageText != null)
        {
            messageText.text = isVictory ? "战斗胜利！" : "战斗失败";
        }

        // 显示回合数
        if (roundText != null)
        {
            roundText.text = $"回合数：{round}";
        }

        // 如果动画正在播放，先停掉，避免重叠BUG
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        // 开始播放弹出动画
        showCoroutine = StartCoroutine(PlayShowAnimation());
    }

    /// <summary>
    /// 返回主菜单（按钮点击执行）
    /// 功能：恢复游戏时间 → 切换场景
    /// </summary>
    public void BackToMainMenu()
    {
        // 恢复游戏速度（因为战斗暂停时timeScale=0）
        Time.timeScale = 1f;
        // 加载主菜单场景
        BattleManager.battleManager.QuitToMenu();
    }

    /// <summary>
    /// 立刻隐藏面板（游戏初始化时用）
    /// </summary>
    private void HideImmediate()
    {
        // 透明度设为0（看不见）
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        // 大小恢复正常
        if (resultBox != null)
            resultBox.localScale = Vector3.one;

        // 直接关闭物体
        if (root != null)
            root.SetActive(false);
    }

    /// <summary>
    /// 弹出动画（渐显 + 从小放大）
    /// 效果：流畅、现代、不突兀
    /// </summary>
    private IEnumerator PlayShowAnimation()
    {
        // 动画总时长
        float duration = 0.25f;
        // 已过去的时间
        float elapsed = 0f;

        // 动画开始前：透明 + 缩小
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        if (resultBox != null)
            resultBox.localScale = Vector3.one * 0.85f;

        // 循环执行动画，直到时间结束
        while (elapsed < duration)
        {
            // 使用unscaledDeltaTime：即使游戏暂停（timeScale=0），动画也能正常播放
            elapsed += Time.unscaledDeltaTime;

            // 计算动画进度 0~1
            float progress = Mathf.Clamp01(elapsed / duration);
            // 平滑曲线（让动画更自然，不是机械匀速）
            float eased = 1f - Mathf.Pow(1f - progress, 3f);

            // 渐显效果
            if (canvasGroup != null)
                canvasGroup.alpha = eased;

            // 从小放大效果
            if (resultBox != null)
                resultBox.localScale = Vector3.one * Mathf.Lerp(0.85f, 1f, eased);

            // 等待下一帧
            yield return null;
        }

        // 动画结束，强制设置为最终状态
        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        if (resultBox != null)
            resultBox.localScale = Vector3.one;
    }
}