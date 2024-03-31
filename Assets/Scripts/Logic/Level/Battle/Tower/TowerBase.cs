
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : EntityBase
{
    public float doSkillPassTime = 0;
    public TowerConfig.Skill skillConfig;
    private List<EnemyBase> _enemys = new List<EnemyBase>();    // 搜索到的敌人
    // 索敌

    private StateMachine skillMachine;  // 防御塔只能触发一个技能(目前)

    public TowerBase() : base() 
    {
        MessageManager.Instance.RegisterMessage(MessageConst.Battle_Collision, MessageBattleCollision);
    }
    public TowerBase(int instanceId, int entityId) : base(instanceId, entityId)
    {
        MessageManager.Instance.RegisterMessage(MessageConst.Battle_Collision, MessageBattleCollision);
    }

    private void MessageBattleCollision(MessageManager.Message m)
    {
        SkillTrackEntity skillEntity = (SkillTrackEntity)m.ps[0];
        EntityBase hitEntity = (EntityBase)m.ps[1];
        if ((TowerBase)(skillEntity.GetOwerEntity()) != this) return;

        var config = GameApp.Instance.SkillConfig.GetSkillConfigData(skillConfig.SkillId).trackConfig;
        if (config.IsBreak) // 打到不是目标是敌人
        {
            skillMachine.Enter(SKILLSTATE.End.ToString());
            // TODO
            Debug.Log("攻击");
        }
        else
        {
            foreach (EnemyBase enemy in _enemys)
            {
                if (hitEntity == enemy)
                {
                    skillMachine.Enter(SKILLSTATE.End.ToString());
                    // TODO
                    Debug.Log("攻击");
                }
            }
        }
    }

    public override void SetEntityId(int entityId)
    {
        base.SetEntityId(entityId);
        skillConfig = GameApp.Instance.TowerConfig.GetTowerConfigData(entityId).skill;
    }

    private void InitSkillMachine()
    {
        int levelId = skillConfig.SkillId;
        skillMachine = new StateMachine("DoSkill" + levelId.ToString());
        skillMachine.RegisterState(new SkillMachineStateReady(SKILLSTATE.Ready.ToString(), skillMachine));
        skillMachine.RegisterState(new SkillMachineStateTrack(SKILLSTATE.Track.ToString(), skillMachine));
        skillMachine.RegisterState(new SkillMachineStateEnd(SKILLSTATE.End.ToString(), skillMachine));
        StateMachineManager.Instace.RegisterMachine(skillMachine);
    }

    public void EnterBattle(Vector2 pos)
    {
        InitSkillMachine();

        ShowSelf(pos);
    }

    protected override void OnUpdate(float delta)
    {
        if (!skillMachine.IsFinish()) return;   // TODO 目前只能释放一次技能

        doSkillPassTime += delta;
        if (doSkillPassTime < skillConfig.SkillInvertal) return;

        // 已经超出攻击范围
        for (int i = _enemys.Count - 1; i >= 0; i--)
        {
            if (Vector2.Distance(_enemys[i].GetPos(),GetPos()) > skillConfig.AttackDistance)
            {
                _enemys.RemoveAt(i);
            }
        }

        int needSearchCount = Mathf.Max(0, skillConfig.AttackNum - _enemys.Count);
        if(needSearchCount > 0)
        {
            EntityManager.Instance.SearchClosestNEntity(ref _enemys, GetPos(), EntityManager.Instance.GetEnemies(), skillConfig.AttackDistance, needSearchCount);
        }

        if (_enemys.Count == 0) return;

        DoSKill();
    }

    public void ExitBattle()
    {
        StateMachineManager.Instace.AddWaitDeleteMachine(skillMachine);
    }

    public void DoSKill()
    {
        doSkillPassTime = 0;

        foreach(var enemy in _enemys)
        {
            DoSkill(enemy);
        }
    }

    public void DoSkill(EntityBase entity)
    {
        skillMachine.InitData(skillConfig.SkillId, this, entity);
        skillMachine.Enter(SKILLSTATE.Ready.ToString());
    }
}