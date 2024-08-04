
using System.Collections.Generic;
using UnityEngine;

public class Tower : TowerBase
{
    protected int skillId;
    protected int attckNum;
    protected float attackInvertal;
    protected float attackDistance;

    // 击中Buff
    protected int hitBuffId;
    protected int hitBuffLevel;

    public float doSkillPassTime = 0;
    private List<int> _enemys = new List<int>();    // 搜索到的敌人
    // 索敌

    private StateMachine skillMachine;  // 防御塔只能触发一个技能(目前)

    public virtual float GetDataAttackContinue()
    {
        return dataConfig.ps[4];
    }

    protected override void OnInitEntityInstance()
    {
        base.OnInitEntityInstance();
    }

    protected override void OnInitEntity()
    {
        base.OnInitEntity();

        // 数据解析 SkillID 索敌数量 攻击间隔 攻击距离 buffId buffLevel
        skillId = (int)dataConfig.ps[0];
        attckNum = (int)dataConfig.ps[1];
        attackInvertal = dataConfig.ps[2];
        attackDistance = dataConfig.ps[3];

        hitBuffId = (int)dataConfig.ps[4];
        hitBuffLevel = (int)dataConfig.ps[5];

        GetGameObject("Range").transform.localScale = Vector3.one * attackDistance * 2;
    }

    private void InitSkillMachine()
    {
        int levelId = GetSkillId();
        skillMachine = new StateMachine("DoSkill" + levelId.ToString());
        skillMachine.RegisterState(new SkillMachineStateReady(SKILLSTATE.Ready.ToString(), skillMachine));
        skillMachine.RegisterState(new SkillMachineStateTrack(SKILLSTATE.Track.ToString(), skillMachine));
        skillMachine.RegisterState(new SkillMachineStateEnd(SKILLSTATE.End.ToString(), skillMachine));
        StateMachineManager.Instace.RegisterMachine(skillMachine);
    }

    public override void Attack(int beAttackMonoId)
    {
        base.Attack(beAttackMonoId);
        BuffSystem.Instance.AddBuff(GetEntityMonoId(), beAttackMonoId, hitBuffId , hitBuffLevel);
    }

    public override void EnterBattle(Vector2 pos)
    {
        base.EnterBattle(pos);

        RegisterMessage(MessageConst.Battle_EnemyDie, MessageEnemyDie);
        RegisterMessage(MessageConst.Battle_EnemyExit, MessageEnemyExit);

        InitSkillMachine();
        doSkillPassTime = GetAttackInverval();
    }

    private void MessageEnemyDie(MessageManager.Message m)
    {
        _enemys.Remove((int)m.ps[0]);
    }

    private void MessageEnemyExit(MessageManager.Message m)
    {
        _enemys.Remove((int)m.ps[0]);
    }

    protected override void OnUpdate(float delta)
    {
        if (!skillMachine.IsFinish()) return;   // TODO 目前只能释放一次技能

        // 保持方向
        if(_enemys.Count > 0 && GetAnimationState() == FrameAnimation.FrameAnimationType.Attack)
        {
            EntityBase entity0 = EntityManager.Instance.GetEntity(_enemys[0]);
            if (entity0 != null) 
            {
                Vector2 dir = new Vector2(entity0.GetPos().x - GetPos().x, entity0.GetPos().y - GetPos().y);
                float angle = CommonUtil.GetAngle(dir.normalized, Vector2.up);
                SetRotationZ(angle);
            }
        }

        doSkillPassTime += delta;
        if (doSkillPassTime < GetAttackInverval()) return;

        // 已经超出攻击范围
        for (int i = _enemys.Count - 1; i >= 0; i--)
        {
            EntityBase entity = EntityManager.Instance.GetEntity(_enemys[i]);
            if (entity != null)
            {
                if (Vector2.Distance(entity.GetPos(), GetPos()) > GetAttackDistance())
                {
                    _enemys.RemoveAt(i);
                }
            }
        }

        int needSearchCount = Mathf.Max(0, GetAttackNum() - _enemys.Count);
        if(needSearchCount > 0)
        {
            EntityManager.Instance.SearchClosestNEntity(ref _enemys, GetPos(), EntityManager.Instance.GetEnemies(), GetAttackDistance(), needSearchCount);
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

    public void DoSkill(int entityId)
    {
        //EntityBase entity = EntityManager.Instance.GetEntity(entityId);
        //Vector2 dir = new Vector2(entity.GetPos().x - GetPos().x , entity.GetPos().y - GetPos().y);
        //float angle = CommonUtil.GetAngle(dir.normalized , Vector2.up);
        //SetRotationZ(angle);

        PlayAnimation(FrameAnimation.FrameAnimationType.Attack);

        Timer.Instance.AddTimer(0.5f, (delta) => 
        {
            skillMachine.InitData(GetSkillId(), GetEntityMonoId(), entityId);
            skillMachine.Enter(SKILLSTATE.Ready.ToString());
            return true; 
        });
    }

    public int GetSkillId()
    {
        return skillId;
    }

    public int GetAttackNum()
    {
        return attckNum;
    }

    public float GetAttackDistance()
    {
        return attackDistance;
    }

    public float GetAttackInverval()
    {
        return attackInvertal;
    }

    public override void AddBuffValue(BuffType buffType, float value)
    {
        base.AddBuffValue(buffType, value);

        if (buffType == BuffType.ChangeSkill)
        {
            skillId = (int)value;
        }
    }
}