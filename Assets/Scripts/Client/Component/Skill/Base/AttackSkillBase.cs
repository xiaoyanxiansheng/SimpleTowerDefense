using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkillBaseData : SkillBaseData
{
    public Vector2 startPos;
    public Action<EntityBase,EntityBase> hitCall;
}

public class AttackSkillBase : SkillBase
{
    // 技能命中敌人后所附加的效果集合
    //private List<Dictionary<string,int>> _effects = new List<Dictionary<string, int>>();
    private AttackSkillBaseData _attackSkillBaseData;

    public void Play(AttackSkillBaseData attackSkillBaseData)
    {
        _attackSkillBaseData = attackSkillBaseData;
        base.Play(attackSkillBaseData);
    }

    private void OnEnable()
    {
    }

    ///// <summary>
    ///// 技能释放的过程中/释放完成后的效果
    ///// </summary>
    ///// <param name="effectSkillBases"></param>
    //public void DoEffects(List<EffectSkillBase> effectSkillBases)
    //{
    //    for (int i = 0; i < effectSkillBases.Count; i++) 
    //    {
    //        for (int j = 0; j < _effects.Count; j++)
    //        {
    //            effectSkillBases[i].Play(_effects[i]["skillid"] , _effects[i]["skillLevel"]);
    //        } 
    //    }
    //}

    protected bool CheckAttackOne()
    {
        EntityBase hitEntity = GetHitEntityOne();
        
        if(hitEntity != null)
        {
            if (Vector2.Distance(hitEntity.GetPosition(), _attackSkillBaseData.startPos) > GetAttackDistance())
            {
                hitEntity = null;
            }
        }

        if (hitEntity == null)
        {
            hitEntity = EntityManager.Instance.GetEntityByDistance(EntityType.Enemy, _attackSkillBaseData.startPos, attackDistance);
            if(hitEntity != null)
            {
                hitEntitys.Clear();
                hitEntitys.Add(hitEntity);
            }
        }

        return hitEntity != null;
    }

    protected EntityBase GetHitEntityOne()
    {
        ClearNullReference();
        if (hitEntitys.Count == 0) return null;
        return hitEntitys[0];
    }

    protected bool CheckAttckList()
    {
        hitEntitys = EntityManager.Instance.GetEntityByDistances(EntityType.Enemy, _attackSkillBaseData.startPos, attackDistance);
        return hitEntitys.Count > 0;
    }

    protected List<EntityBase> GetHitEntityList()
    {
        return hitEntitys;
    }
}
