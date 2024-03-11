using UnityEngine;
public class CommonBattle : BattleBase
{
    public CommonBattle(LevelConfigData levelConfigData) : base(levelConfigData)
    {
    }

    public override bool Update(float delta)
    {
        Debug.Log("Battle Update");
        return true;
    }
}