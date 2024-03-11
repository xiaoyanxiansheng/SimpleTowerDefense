using System;

public abstract class BattleBase
{
    private LevelConfigData _levelConfigData;

    private int _timerId;

    public BattleBase(LevelConfigData levelConfigData)
    {
        _levelConfigData = levelConfigData;
    }

    public void StartBattle()
    {
        Timer.Instance.AddTimer(0.001f, Update);
    }

    public void ExitBattle()
    {
        Timer.Instance.RemoveTimer(_timerId);
    }

    public void PauseBattle()
    {
        Timer.Instance.PauseTimer(_timerId);
    }

    public abstract bool Update(float delta);

    public LevelConfigData GetLevelConfigData()
    {
        return _levelConfigData;
    }
}
