// π‹¿Ìπÿø®

using UnityEngine;

public class LevelManager
{
    public static LevelManager Instance;

    public LevelManager() {
        Instance = this;
    }

    public BattleBase battle;

    public void Init()
    {
    }

    public void CreateBattle(GameObject battleRoot,int battleId)
    {
        BattleConfig battleConfig = GameApp.Instance.LevelConfig.GetBattleConfig(battleId);
        battle = new CommonBattle(battleRoot, battleConfig);
    }

    public void StartBattle(int levelId = 0)
    {
        battle.StartBattle();
    }
}
