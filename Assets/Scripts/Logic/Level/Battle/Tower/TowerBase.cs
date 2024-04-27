
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : EntityBase
{
    protected int skillId;
    protected int attckNum;
    protected float attackInvertal;
    protected float attackDistance;
    
    protected int buffId;
    protected int buffLevel;

    public float doSkillPassTime = 0;
    private List<int> _enemys = new List<int>();    // 搜索到的敌人
    // 索敌

    private StateMachine skillMachine;  // 防御塔只能触发一个技能(目前)

    public virtual float GetDataAttackContinue()
    {
        return dataConfig.ps[4];
    }

    protected override void OnInitEntity()
    {
        base.OnInitEntity();

        // 数据解析 SkillID 索敌数量 攻击间隔 攻击距离 buffId buffLevel
        skillId = (int)dataConfig.ps[0];
        attckNum = (int)dataConfig.ps[1];
        attackInvertal = dataConfig.ps[2];
        attackDistance = dataConfig.ps[3];

        buffId = (int)dataConfig.ps[4];
        buffLevel = (int)dataConfig.ps[5];
    }

    private void InitSkillMachine()
    {
        int levelId = GetBuffSkillId();
        skillMachine = new StateMachine("DoSkill" + levelId.ToString());
        skillMachine.RegisterState(new SkillMachineStateReady(SKILLSTATE.Ready.ToString(), skillMachine));
        skillMachine.RegisterState(new SkillMachineStateTrack(SKILLSTATE.Track.ToString(), skillMachine));
        skillMachine.RegisterState(new SkillMachineStateEnd(SKILLSTATE.End.ToString(), skillMachine));
        StateMachineManager.Instace.RegisterMachine(skillMachine);
    }

    public override void Attack(int beAttackMonoId)
    {
        EntityBase entity = EntityManager.Instance.GetEntity(beAttackMonoId);
        if (entity != null) entity.AddBuffer(buffId, buffLevel);
    }

    public void EnterBattle(Vector2 pos)
    {
        InitSkillMachine();

        ShowSelf(pos);

        base.EnterBattle();
    }

    protected override void OnUpdate(float delta)
    {
        if (!skillMachine.IsFinish()) return;   // TODO 目前只能释放一次技能

        doSkillPassTime += delta;
        if (doSkillPassTime < GetBuffAttackInverval()) return;

        // 已经超出攻击范围
        for (int i = _enemys.Count - 1; i >= 0; i--)
        {
            EntityBase entity = EntityManager.Instance.GetEntity(_enemys[i]);
            if (entity != null)
            {
                if (Vector2.Distance(entity.GetPos(), GetPos()) > GetBuffAttackDistance())
                {
                    _enemys.RemoveAt(i);
                }
            }
        }

        int needSearchCount = Mathf.Max(0, GetBuffAttackNum() - _enemys.Count);
        if(needSearchCount > 0)
        {
            EntityManager.Instance.SearchClosestNEntity(ref _enemys, GetPos(), EntityManager.Instance.GetEnemies(), GetBuffAttackDistance(), needSearchCount);
        }

        if (_enemys.Count == 0) return;

        DoSKill();
    }

    public void DoSKill()
    {
        doSkillPassTime = 0;

        foreach(var enemy in _enemys)
        {
            DoSkill(enemy);
        }
    }

    public void DoSkill(int entity)
    {
        skillMachine.InitData(GetBuffSkillId(), GetEntityMonoId(), entity);
        skillMachine.Enter(SKILLSTATE.Ready.ToString());
    }

    public override int GetBuffSkillId()
    {
        return skillId;
    }
    public override void SetBuffSkillId(int skillId)
    {
        this.skillId = skillId;
    }

    public override void SetBuffAttackNum(int num)
    {
        attckNum = num;
    }

    public override int GetBuffAttackNum()
    {
        return attckNum;
    }

    public override float GetBuffAttackDistance()
    {
        return attackDistance;
    }
    public override void SetBuffAttackDistance(float distance)
    {
        this.attackDistance = distance;
    }

    public override float GetBuffAttackInverval()
    {
        return attackInvertal;
    }
    public override void SetBuffAttackInverval(float invertal)
    {
        attackInvertal = invertal;
    }
}