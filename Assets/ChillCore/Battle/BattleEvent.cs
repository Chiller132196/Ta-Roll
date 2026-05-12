/// <summary>
/// 战斗中，实体间传递的信息
/// </summary>
public class BattleEvent
{
    public int deltaMaxHP;

    public int deltaMaxMP;

    public int deltaHP;

    public int deltaMP;

    public int deltaATK;

    public int deltaDF;

    public int deltaCharge;

    public int deltaChargeSpeed;

    /// <summary>
    /// 是否消耗破绽
    /// </summary>
    public bool consumedFlaw;

    /// <summary>
    /// 是否造成破绽
    /// </summary>
    public bool bringFlaw;

    /// <summary>
    /// 这个信息的来源
    /// </summary>
    public Entity owner;

    public int CompareTo(BattleEvent _battleEvent)
    {
        if (deltaHP < _battleEvent.deltaHP)
        {
            return -1;
        }

        if (deltaMP < _battleEvent.deltaMP)
        {
            return -1;
        }

        return 0;
    }
}
