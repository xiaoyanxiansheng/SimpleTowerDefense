using System;
using UnityEngine;

/*
    BattleBase �Ĺ�������
    1 Update
    2 Entity
    3 ս�����ݣ����ƶ����ɰڷţ�
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

        // ע���ⲿ��Ϣ
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

        // ս����ʱ�� ����
        MessageManager.Instance.SendMessage(MessageConst.Battle_BattleStart);
        
        Timer.Instance.AddTimer(0.001f, BaseUpdate);

        // �����볡
        entityManager.StartBattle();
    }

    private bool BaseUpdate(float delta)
    {
        // ���������ս�����ټ���
        _battleContinueTime += delta;

        entityManager.Update(delta, _battleContinueTime);
        stateMachineManager.Update(delta);

        return Update(delta);
    }

    public void EndBattle()
    {
        _battleStatusType = BattleStatusType.EndBattle;

        Timer.Instance.RemoveTimer(_timerId);

        // TODO ��Ҫ���ֳɹ�����ʧ��
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
