using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("淡入淡出")]
    public Image fadeImage;
    public string sceneName = "LevelTest";

    [Header("设置面板")]
    public GameObject settingPanel;    // 设置面板
    public Slider volumeSlider;        // 音量滑条
    public TMP_Dropdown resolutionDropdown; // 分辨率下拉框

    void Start()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
        }

        // 初始化音量
        volumeSlider.value = AudioListener.volume;

        // 初始化分辨率选项
        resolutionDropdown.ClearOptions();
        System.Collections.Generic.List<string> options = new System.Collections.Generic.List<string>();
        options.Add("1920 x 1080");
        options.Add("1280 x 720");
        options.Add("800 x 600");
        resolutionDropdown.AddOptions(options);
    }

    //=======================
    // 开始游戏
    //=======================
    public void StartGame()
    {
        StartCoroutine(FadeToScene());
    }

    //=======================
    // 退出游戏
    //=======================
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("退出游戏");
    }

    //=======================
    // 设置面板 打开 / 关闭
    //=======================
    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        settingPanel.SetActive(false);
    }

    //=======================
    // 音量调节
    //=======================
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    //=======================
    // 分辨率设置
    //=======================
    public void SetResolution(int index)
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
                break;
            case 1:
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                break;
            case 2:
                Screen.SetResolution(800, 600, FullScreenMode.Windowed);
                break;
        }
    }

    //=======================
    // 场景切换淡入淡出
    //=======================
    IEnumerator FadeToScene()
    {
        fadeImage.gameObject.SetActive(true);
        Color c = fadeImage.color;

        for (float a = 0; a <= 1; a += 0.02f)
        {
            c.a = a;
            fadeImage.color = c;
            yield return new WaitForSeconds(0.01f);
        }

        Debug.Log("转到下一个场景");
        CoreManager.Core.JumpToScene("LevelTest");
    }
}