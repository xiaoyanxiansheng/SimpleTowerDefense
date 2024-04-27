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

    public void CreateBattle(GameObject battleRoot,BattleBase.InitData initData)
    {
        battle = new CommonBattle(battleRoot, initData);
    }

    public void StartBattle(int levelId = 0)
    {
        battle.StartBattle();
    }
}
