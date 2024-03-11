using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBaseData
{
    public EntityBase attackEntity;
    public int skillId;
    public int skillLevel;
    public float baseAttackDistance;
    public float baseAttackInterval;
    public float baseAttackDamaged;

    public virtual float GetAttackDistance()
    {
        return baseAttackDistance;
    }

    public virtual float GetAttackInterval()
    {
        return baseAttackInterval;
    }

    public virtual float GetAttackDamaged()
    {
        return baseAttackDamaged;
    }
}

/// <summary>
/// 技能基类
/// </summary>
public class SkillBase : EntityBase
{
    protected SkillBaseData skillBaseData = null;  
    protected List<EntityBase> hitEntitys = new List<EntityBase>();

    private bool _canAttack = false;
    private float _attackIntervalCount = 0;

    public void Play(SkillBaseData skillBaseData)
    {
        _canAttack = true;
        this.skillBaseData = skillBaseData;
    }

    public void Update()
    {
        if (!_canAttack) return;

        OnUpdate();

        _attackIntervalCount += Time.deltaTime;
        if (_attackIntervalCount < skillBaseData.GetAttackInterval()) return;

        if (OnCheckAttackUpdate())
        {
            _attackIntervalCount = 0;
            OnAttack();
        }
    }

    public int GetSkillId()
    {
        return skillBaseData.skillId;
    }

    protected float GetAttackDistance()
    {
        return skillBaseData.GetAttackDistance();
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
