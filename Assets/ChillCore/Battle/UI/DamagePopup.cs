using System.Collections;
using TMPro;
using UnityEngine;

// 伤害飘字脚本：负责显示伤害数字 + 向上飘 + 渐隐 + 自动销毁
public class DamagePopup : MonoBehaviour
{
    [Header("伤害文字设置")]
    // 显示伤害数字的文本组件
    public TMP_Text damageText;
    // 飘字动画持续时间
    public float lifeTime = 0.8f;
    // 飘字总共向上移动的距离
    public float moveDistance = 0.6f;
    // 伤害数字的颜色（默认白色）
    public Color damageColor = Color.white;

    // 主相机，用于让文字永远朝向相机
    private Camera mainCamera;
    // 动画开始时的初始位置
    private Vector3 startPosition;

    // 游戏启动时执行
    private void Awake()
    {
        // 获取场景里的主相机
        mainCamera = Camera.main;

        // 如果没手动拖入damageText，就自动找子物体里的TMP文本
        if (damageText == null)
        {
            damageText = GetComponentInChildren<TMP_Text>();
        }
    }

    // 外部调用这个方法，显示伤害数字
    // 比如：角色受伤时，调用 Play(10) 显示 10 点伤害
    public void Play(int damageAmount)
    {
        // 设置文本显示伤害数值
        if (damageText != null)
        {
            damageText.text = damageAmount.ToString();
            damageText.color = damageColor;
        }

        // 记录开始位置
        startPosition = transform.position;
        // 启动飘字动画协程
        StartCoroutine(PlayAnimation());
    }

    // 每帧最后执行：让伤害文字永远朝向相机（3D UI必加）
    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            transform.forward = mainCamera.transform.forward;
        }
    }

    // 飘字动画协程
    private IEnumerator PlayAnimation()
    {
        // 已经过的时间
        float elapsedTime = 0f;

        // 在动画时间内，每帧执行
        while (elapsedTime < lifeTime)
        {
            // 累加时间
            elapsedTime += Time.deltaTime;
            // 计算动画进度（0~1）
            float progress = Mathf.Clamp01(elapsedTime / lifeTime);

            // 让文字向上移动
            transform.position = startPosition + Vector3.up * (moveDistance * progress);

            // 让文字渐渐透明（fade out）
            if (damageText != null)
            {
                Color color = damageText.color;
                color.a = 1f - progress; // 进度越大，透明度越高
                damageText.color = color;
            }

            // 等待一帧，继续循环
            yield return null;
        }

        // 动画结束，销毁自己
        Destroy(gameObject);
    }
}