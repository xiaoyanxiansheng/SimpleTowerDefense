using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBaseData
{
    public EntityBase attackEntity;
    public int skillId;
    public int skillLevel;
    public int attackDistance;
    public float attackInterval;
}

/// <summary>
/// 技能基类
/// </summary>
public class SkillBase : EntityBase
{
    protected int skillId;
    protected int skillLevel;
    protected float attackDistance = 0;
    protected float attackInterval = 0;

    protected EntityBase attackEntity;   
    protected List<EntityBase> hitEntitys = new List<EntityBase>();

    private bool _canAttack = false;
    private float _attackIntervalCount = 0;

    public void Play(SkillBaseData skillBaseData)
    {
        _canAttack = true;
        attackEntity = skillBaseData.attackEntity;
        skillId = skillBaseData.skillId;
        skillLevel = skillBaseData.skillLevel;
        attackDistance = skillBaseData.attackDistance;
        attackInterval = skillBaseData.attackInterval;
        _attackIntervalCount = attackInterval;
    }

    public void Update()
    {
        if (!_canAttack) return;

        OnUpdate();

        _attackIntervalCount += Time.deltaTime;
        if (_attackIntervalCount < attackInterval) return;

        if (OnCheckAttackUpdate())
        {
            _attackIntervalCount = 0;
            OnAttack();
        }
    }

    public int GetSkillId()
    {
        return skillId;
    }

    public int GetSkillLevelId()
    {
        return skillLevel;
    }



    public void SetAttackDistance(float attackDistance)
    {
        this.attackDistance = attackDistance;
    }

    public void SetAttackInterval(float attackInterval)
    {
        this.attackInterval = attackInterval;
    }

    protected float GetAttackDistance()
    {
        return attackDistance;
    }

    protected EntityBase GetAttackEntity()
    {
        return attackEntity;
    }

    protected List<EntityBase> GetBeatEntitys()
    {
        return hitEntitys;
    }

    public void ClearNullReference()
    {
        for (int i = hitEntitys.Count - 1; i >= 0; i--)
        {
            EntityBase entity = hitEntitys[i];
            if (entity == null)
            {
                hitEntitys.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 检测目标
    /// </summary>
    /// <returns></returns>
    protected virtual bool OnCheckAttackUpdate()
    {
        return false;
    }

    /// <summary>
    /// 检测到目标之后开始攻击
    /// </summary>
    protected virtual void OnAttack()
    {

    }

    protected virtual void OnUpdate()
    {

    }
}
