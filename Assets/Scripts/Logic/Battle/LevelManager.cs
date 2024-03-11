// π‹¿Ìπÿø®

public class LevelManager
{
    public static LevelManager Instance;

    public LevelManager() {
        Instance = this;
    }

    private LevelConfig _levelConfig;
    private BattleBase _battle;

    public void Init(LevelConfig levelConfig)
    {
        _levelConfig = levelConfig;
    }

    public void CreateBattle(int levelId)
    {
        LevelConfigData levelConfigData= _levelConfig.GetLevelConfigData(levelId);
        _battle = new CommonBattle(levelConfigData);
    }

    public void StartBattle(int levelId = 0)
    {
        _battle.StartBattle();
    }
}
