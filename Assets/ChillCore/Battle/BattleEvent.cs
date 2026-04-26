/// <summary>
/// 战斗中，实体间传递的信息
/// </summary>
public class BattleEvent
{
    public int deltaHP;

    public int deltaATK;

    public int deltaMP;

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
}
