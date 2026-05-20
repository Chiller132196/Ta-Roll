using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public static BattleManager battleManager => Instance;

    /// <summary>
    /// 当前回合数
    /// </summary>
    public int round;

    /// <summary>
    /// 每个回合之间的时间间隔
    /// </summary>
    public float roundTimer;

    /// <summary>
    /// 战斗状态, -1静默, 0战斗, 1玩家胜利, 2玩家失败
    /// </summary>
    public int battleState;

    /// <summary>
    /// 本回合的单位
    /// </summary>
    public List<GameObject> entitysThisRound;

    /// <summary>
    /// 判断当前有多少棋子处于它的演出阶段
    /// </summary>
    public int chessOnAnimation;

    /// <summary>
    /// 当前战斗是否暂停
    /// </summary>
    public bool isBattlePaused;

    /// <summary>
    /// 战斗结算面板
    /// </summary>
    public BattleResultPanel resultPanel;

    #region 战斗控制

    /// <summary>
    /// 声明函数指针，通过事件收集每回合的棋子
    /// </summary>
    /// <returns>棋子的gameobject</returns>
    public delegate GameObject NewRoundBegin(int _round);

    /// <summary>
    /// 函数数组
    /// </summary>
    public static event NewRoundBegin OnNewRoundBegin;

    /// <summary>
    /// 暂停/继续战斗，给暂停按钮 OnClick 调用。
    /// </summary>
    public void TogglePause()
    {
        SetBattlePaused(!isBattlePaused);
    }

    /// <summary>
    /// 设置战斗暂停状态。
    /// </summary>
    public void SetBattlePaused(bool paused)
    {
        if (battleState != 0)
        {
            return;
        }

        isBattlePaused = paused;
        Time.timeScale = isBattlePaused ? 0f : 1f;

        Debug.Log(isBattlePaused ? "战斗暂停" : "战斗继续");
    }

    /// <summary>
    /// 恢复正常时间流速。
    /// </summary>
    private void ResetBattleTimeScale()
    {
        isBattlePaused = false;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// 新一局战斗开始时触发
    /// </summary>
    public void NewGameStart()
    {
        Debug.Log("New Battle Begin");

        ResetBattleTimeScale();

        battleState = 0;

        round = 0;

        entitysThisRound = GetEntitys(round);

        if (entitysThisRound == null)
        {
            Debug.Log("!!!无在场单位，战斗无法继续!!!");
            return;
        }

        StartCoroutine(RoundLoopCoroutine());
    }

    /// <summary>
    /// 获取场上的棋子
    /// </summary>
    /// <param name="_round">当前回合</param>
    /// <returns>全部实体的列表</returns>
    public List<GameObject> GetEntitys(int _round)
    {
        List<GameObject> entitys = new List<GameObject>();

        if (OnNewRoundBegin != null)
        {
            System.Delegate[] invocators = OnNewRoundBegin.GetInvocationList();

            foreach (NewRoundBegin entity in invocators)
            {
                GameObject entityObject = entity(_round);
                if (entityObject != null)
                {
                    entitys.Add(entityObject);
                }
            }

            return entitys;
        }

        Debug.Log("!!!无实体在场，检查是否设置错误!!!");
        return null;
    }

    /// <summary>
    /// 检查战斗是否需要继续进行
    /// </summary>
    /// <returns>是否获胜</returns>
    public int CheckBattleState()
    {
        int playerChessAlive = 0;
        int enemyChessAlive = 0;

        foreach (GameObject entityObject in entitysThisRound)
        {
            Entity entity = entityObject.GetComponent<Entity>();
            if (entity == null || !entity.isAlive)
                continue;

            if (entity.chesstype == Chesstype.Player)
            {
                playerChessAlive += 1;
            }
            else
            {
                enemyChessAlive += 1;
            }
        }

        if (playerChessAlive > 0 && enemyChessAlive > 0)
        {
            return 0;
        }

        if (playerChessAlive <= 0)
        {
            BattleLose();
            return -1;
        }

        BattleWin();
        return -1;
    }

    /// <summary>
    /// 战斗胜利的逻辑
    /// </summary>
    public void BattleWin()
    {
        Debug.Log("玩家胜利！");
        battleState = -1;
        ResetBattleTimeScale();
        ShowBattleResult(true);
    }

    /// <summary>
    /// 战斗失败的逻辑
    /// </summary>
    public void BattleLose()
    {
        Debug.Log("敌军胜利！");
        battleState = -1;
        ResetBattleTimeScale();
        ShowBattleResult(false);
    }

    private void ShowBattleResult(bool isVictory)
    {
        if (resultPanel == null)
        {
            resultPanel = FindObjectOfType<BattleResultPanel>(true);
        }

        if (resultPanel == null)
        {
            Debug.LogWarning("未找到 BattleResultPanel，无法显示战斗结算面板");
            return;
        }

        resultPanel.Show(isVictory, round);
    }

    #endregion

    #region 回合控制

    /// <summary>
    /// 控制战斗回合循环的协程
    /// </summary>
    /// <returns></returns>
    public IEnumerator RoundLoopCoroutine()
    {
        while (battleState == 0)
        {
            yield return StartCoroutine(NewRoundStart());

            if (battleState != 0)
                break;
        }

        Debug.Log("回合循环结束，战斗正式结束");
    }

    public IEnumerator NewRoundStart()
    {
        entitysThisRound = GetEntitys(round);

        if (entitysThisRound == null)
        {
            Debug.Log("!!!无在场单位，战斗无法继续!!!");
            yield break;
        }

        if (round > 0 && CheckBattleState() == -1)
        {
            yield break;
        }

        round += 1;

        if (round == 1)
        {
            foreach (GameObject entityObject in entitysThisRound)
            {
                Entity entity = entityObject.GetComponent<Entity>();
                if (entity != null)
                {
                    entity.Spawn();
                }
            }
        }

        if (GridManager.gridManager != null)
        {
            GridManager.gridManager.SyncGridChessReferences(entitysThisRound);
        }

        if (CheckBattleState() == -1)
        {
            yield break;
        }

        foreach (GameObject entityObject in entitysThisRound)
        {
            Entity entity = entityObject.GetComponent<Entity>();
            if (entity != null)
            {
                entity.CastChessSkill();
            }
        }

        yield return new WaitUntil(() => chessOnAnimation == 0);

        foreach (GameObject entityObject in entitysThisRound)
        {
            Entity entity = entityObject.GetComponent<Entity>();
            if (entity == null || !entity.isAlive)
                continue;

            Debug.Log(entityObject.name + "开始补给");
            entity.CastSupply();
        }

        yield return new WaitForSeconds(2.5f);
    }

    #endregion

    void Start()
    {
        chessOnAnimation = 0;
    }

    private void OnDestroy()
    {
        ResetBattleTimeScale();

        if (_instance == this)
        {
            _instance = null;
        }
    }

    internal override void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
