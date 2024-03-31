using System;
using UnityEngine;

/*
    BattleBase 的管理责任
    1 Update
    2 Entity
    3 战斗数据（可移动，可摆放）
 */

public enum BattleStatusType
{
    None = -1,
    StartBattle,
    PauseBattle,
    EndBattle,
}

public abstract class BattleBase
{
    private BattleStatusType _battleStatusType = BattleStatusType.None;
    private float _battleContinueTime = 0;
    private int _timerId;
    public EntityManager entityManager;
    public BattleCellData battleCellManager;
    public StateMachineManager stateMachineManager;
    public SkillManager skillManager;
    public GameObject BattleRoot;

    public BattleBase(GameObject battleRoot, BattleConfig levelConfigData)
    {
        BattleRoot = battleRoot;
        entityManager = new EntityManager(levelConfigData.battleEnemyConfig);
        battleCellManager = new BattleCellData(levelConfigData.battleWaysConfig);
        stateMachineManager = new StateMachineManager();
        skillManager = new SkillManager();

        // 注册外部消息
        MessageManager.Instance.RegisterMessage(MessageConst.Battle_TowerPlayDownOrUp, MessageTowerPlayDownOrUp);
    }


    private void MessageTowerPlayDownOrUp(MessageManager.Message m)
    {
        int x = (int)m.ps[0];
        int y = (int)m.ps[1];
        bool play = (bool)m.ps[2];
        int towerId = (int)m.ps[3];

        battleCellManager.UpdateWeight(x, y, play);
        entityManager.CreateTower(towerId, (towerBase) =>
        {
            TowerBase tower = (TowerBase)towerBase;
            tower.SetEntityId(towerId);
            tower.EnterBattle(new Vector2(x * Define.CELL_SIZE, y * Define.CELL_SIZE));
        });
        entityManager.UpdateAllEnemyCellPaths();
    }

    public void StartBattle()
    {
        _battleStatusType = BattleStatusType.StartBattle;

        // 战斗计时器 启动
        MessageManager.Instance.SendMessage(MessageConst.Battle_BattleStart);
        
        Timer.Instance.AddTimer(0.001f, BaseUpdate);

        // 敌人入场
        entityManager.StartBattle();
    }

    private bool BaseUpdate(float delta)
    {
        // 这里可以做战斗加速减速
        _battleContinueTime += delta;

        entityManager.Update(delta, _battleContinueTime);
        stateMachineManager.Update(delta);

        return Update(delta);
    }

    public void EndBattle()
    {
        _battleStatusType = BattleStatusType.EndBattle;

        Timer.Instance.RemoveTimer(_timerId);

        // TODO 需要区分成功还是失败
        MessageManager.Instance.SendMessage(MessageConst.Battle_BattleSuccess);
    }

    public void PauseBattle()
    {
        _battleStatusType = BattleStatusType.PauseBattle;

        Timer.Instance.PauseTimer(_timerId);
    }

    public abstract bool Update(float delta);

    public BattleCellData GetBattleCellManager() 
    {
       return battleCellManager;
    }

    public BattleStatusType GetBattleStatus()
    {
        return _battleStatusType;
    }
}
