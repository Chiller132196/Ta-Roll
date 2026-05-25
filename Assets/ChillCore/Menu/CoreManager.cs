using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CoreManager : Singleton<CoreManager>
{
    public int battleEndedNum = 2;

    public static CoreManager Core => Instance;

    public void GameOver()
    {
        Debug.LogWarning("---游戏已结束，清理数据---");

        battleEndedNum = 2;

        ChessKeeper.chessKeeper.Clear();

        JumpToScene("StartMenu");
    }

    /// <summary>
    /// 跳转至其他场景
    /// </summary>
    /// <param name="_sceneName"></param>
    public void JumpToScene(string _sceneName)
    {
        Debug.Log("跳转至" + _sceneName);

        SceneManager.LoadScene(_sceneName);
    }

    public void JumpToLevelScene()
    {
        StartCoroutine(LoadLevelScene());
    }

    public void JumpToBattleScene(string _sceneName)
    {
        StartCoroutine(LoadBattleScene(_sceneName + battleEndedNum));
    }

    public IEnumerator LoadBattleScene(string _sceneName)
    {
        Debug.Log("跳转至" + _sceneName);

        ChessKeeper.chessKeeper.UpdateChessData();

        SceneManager.LoadScene(_sceneName);

        yield return null;

        Debug.Log("---释放所有棋子---");
        ChessKeeper.chessKeeper.ReleaseChess();
    }

    public IEnumerator LoadLevelScene()
    {
        SceneManager.LoadScene("LevelTest");

        yield return null;

        Debug.Log("---释放所有棋子---");
        ChessKeeper.chessKeeper.ReleaseChess();
    }

    internal override void Awake()
    {
        if (_instance == null)
        {
            _instance = this as CoreManager;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Screen.fullScreen = false;
        PlayerPrefs.SetInt("Screenmanager Fullscreen mode", 0);
        PlayerPrefs.Save();

        Screen.SetResolution(720, 1080, FullScreenMode.Windowed);

    }
}
