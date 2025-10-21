using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    internal int HP;

    internal int ATK;

    /// <summary>
    /// 接收的战斗信息
    /// </summary>
    internal BattleEvent battleEvent;

    internal Queue<BattleEvent> tempEvents;

    internal void GetBattleEvent(BattleEvent _battleEvent)
    {
            tempEvents.Enqueue(_battleEvent);
    }

    internal void ReadBattleEvent(BattleEvent _battleEvent)
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        if (tempEvents.Count > 0)
        {
            ReadBattleEvent(tempEvents.Dequeue());
        }
    }
}
