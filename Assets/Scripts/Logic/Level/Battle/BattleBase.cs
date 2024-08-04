using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using static BattleBase;
using static BuffSystem;

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

    public class InitData
    {
        public int chapterIndex;
        public int levelIndex;
        public bool isFixPaths;
        public List<int2> canWalkPoints;
        public List<int2> canPlacePoints;
        public List<LevelEnemyData> enemys;
    }

    private BattleStatusType _battleStatusType = BattleStatusType.None;
    private float _battleContinueTime = 0;
    private int _timerId;
    public EntityManager entityManager;
    public BuffSystem buffSystem;
    public BattleCellData battleCellManager;
    public StateMachineManager stateMachineManager;
    public SkillManager skillManager;
    public GameObject BattleRoot;
    public GameObject Battle3DRoot;

    public BattleBase(GameObject battleRoot, InitData initData)
    {
        BattleRoot = battleRoot;

        entityManager = new EntityManager();
        battleCellManager = new BattleCellData();
        stateMachineManager = new StateMachineManager();
        skillManager = new SkillManager();
        buffSystem = new BuffSystem();

        // 注册外部消息
        MessageManager.Instance.RegisterMessage(MessageConst.Battle_UI_TowerPlayDownOrUp, MessageTowerPlayDownOrUp);
        MessageManager.Instance.RegisterMessage(MessageConst.Battle_UI_CusionPlayDownOrUp, MessageCusionPlayDownOrUp);
        MessageManager.Instance.RegisterMessage(MessageConst.Battle_Collision, MessageBattleCollision);
        MessageManager.Instance.RegisterMessage(MessageConst.Battle_EnemyDie, MessageEnemyDie);
        MessageManager.Instance.RegisterMessage(MessageConst.Battle_EnemyExit, MessageEnemyExit);

        AddData(initData);
    }

    public void AddData(InitData initData)
    {
        entityManager.AddEnemys(initData.enemys);
        battleCellManager.AddCell(initData.isFixPaths, initData.canWalkPoints, initData.canPlacePoints);
    }


    private void MessageTowerPlayDownOrUp(MessageManager.Message m)
    {
        int x = (int)m.ps[0];
        int y = (int)m.ps[1];
        bool play = (bool)m.ps[2];
        int towerId = (int)m.ps[3];
        entityManager.CreateTower(towerId, (towerBase) =>
        {
            Tower tower = (Tower)towerBase;
            tower.EnterBattle(new Vector2(x * Define.CELL_SIZE, y * Define.CELL_SIZE));
            AddCellBuff(x, y, towerId);
        });
        battleCellManager.UpdateWeight(towerId,x, y, play);
        entityManager.UpdateAllEnemyCellPaths();
    }

    private void MessageCusionPlayDownOrUp(MessageManager.Message m)
    {
        int x = (int)m.ps[0];
        int y = (int)m.ps[1];
        bool play = (bool)m.ps[2];
        int towerCusionId = (int)m.ps[3];
        entityManager.CreateTowerCusion(towerCusionId, (towerBase) =>
        {
            TowerCusion towerCusion = (TowerCusion)towerBase;
            towerCusion.EnterBattle(new Vector2(x * Define.CELL_SIZE, y * Define.CELL_SIZE));
            AddCellBuff(x, y, towerCusionId);
        });
        battleCellManager.UpdateWeight(towerCusionId, x, y, play, CellType.Place);
        entityManager.UpdateAllEnemyCellPaths();
    }

    private void MessageEnemyDie(MessageManager.Message m)
    {
        EntityManager.Instance.DestoryEnemy((int)m.ps[0]);
    }

    private void MessageEnemyExit(MessageManager.Message m)
    {
        EntityManager.Instance.DestoryEnemy((int)m.ps[0]);
    }
    
    private void MessageBattleCollision(MessageManager.Message m)
    {
        EntityBase attackEntity = EntityManager.Instance.GetEntity(((SkillTrackEntity)EntityManager.Instance.GetEntity((int)m.ps[0])).GetOwerEntityMonoId());
        EntityBase attackedEntity = EntityManager.Instance.GetEntity((int)m.ps[1]);

        attackEntity.Attack(attackedEntity.GetEntityMonoId());
        attackedEntity.Attacked(attackEntity.GetEntityMonoId());

        //List<EntityBase> entitys = entityManager.GetEntitys();

        //var config = GameApp.Instance.SkillConfig.GetSkillConfigData(skillConfig.SkillId).trackConfig;
        //if (config.IsBreak) // 打到不是目标是敌人
        //{
        //    skillMachine.Enter(SKILLSTATE.End.ToString());
        //    // TODO
        //    Debug.Log("攻击");
        //}
        //else
        //{
        //    foreach (EnemyBase enemy in _enemys)
        //    {
        //        if (hitEntity == enemy)
        //        {
        //            skillMachine.Enter(SKILLSTATE.End.ToString());
        //            // TODO
        //            Debug.Log("攻击");
        //        }
        //    }
        //}
    }

    public void StartBattle()
    {
        _battleStatusType = BattleStatusType.StartBattle;

        // 战斗计时器 启动
        MessageManager.Instance.SendMessage(MessageConst.Battle_BattleStart);

        _timerId = Timer.Instance.AddTimer(0.001f, BaseUpdate);

        // 敌人入场
        battleCellManager.StartBattle();
        entityManager.StartBattle();
    }

    private bool BaseUpdate(float delta)
    {
        // 这里可以做战斗加速减速
        _battleContinueTime += delta;

        entityManager.Update(delta, _battleContinueTime);
        stateMachineManager.Update(delta);
        buffSystem.Update(delta);

        return Update(delta);
    }

    public void ExitBattle()
    {
        _battleStatusType = BattleStatusType.EndBattle;

        Timer.Instance.RemoveTimer(_timerId);
        MessageManager.Instance.RemoveMessage(MessageConst.Battle_UI_TowerPlayDownOrUp, MessageTowerPlayDownOrUp);
        MessageManager.Instance.RemoveMessage(MessageConst.Battle_Collision, MessageBattleCollision);
        MessageManager.Instance.RemoveMessage(MessageConst.Battle_EnemyDie, MessageEnemyDie);

        EntityManager.Instance.ExitBattle();
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

    public Vector3 GetBattleRoot3DPosition()
    {
        return Battle3DRoot.transform.position;
    }

    // 给某个位置上的Tower增加Buff
    private void AddCellBuff(int x , int y , int entityId)
    {
        int towerCusionId = battleCellManager.GetTowerCusion(x, y);
        if (towerCusionId == 0) return;

        EntityBase entity = EntityManager.Instance.GetEntity(towerCusionId);
        if (entity != null)
        {
            TowerCusion cusion = (TowerCusion)entity;
            foreach (int buffId in cusion.GetBuffs())
            {
                BuffSystem.Instance.AddBuff(entityId, entityId, buffId, 1);  // TODO
            }
        }
    }
}
