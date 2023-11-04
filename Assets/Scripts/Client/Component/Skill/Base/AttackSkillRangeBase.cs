using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkillRangeBaseData : AttackSkillBaseData
{
    public Action hitCall;
}

public class AttackSkillRangeBase : AttackSkillBase
{
    private AttackSkillRangeBaseData _attackSkillRangeBaseData = null;
    private List<EntityBase> entityBases = new List<EntityBase>();
    private List<float> entityShowCDs = new List<float>();

    public void Play(AttackSkillRangeBaseData attackSkillRangeBaseData)
    {
        _attackSkillRangeBaseData = attackSkillRangeBaseData;
        base.Play(_attackSkillRangeBaseData);
    }

    protected override bool OnCheckAttackUpdate()
    {
        return CheckAttackOne();
    }

    protected override void OnAttack()
    {
        EntityBase entity = GetHitEntityOne();
        if (entity == null) { return; }

        Vector2 dir = (entity.GetPosition() - GetPosition()).normalized;

        EntityBase bulletEntity = EntityManager.Instance.CreateEntity<EntityBase>(EntityType.SKill, GetSkillId());
        entityBases.Add(bulletEntity);
        entityShowCDs.Add(0.3f);
        PointMoveComponent pointMoveComponent = bulletEntity.GetComponent<PointMoveComponent>();
        // 移动
        pointMoveComponent.MoveDir(GetAttackEntity().GetPosition(), dir, () =>
        {
            // 移动到了目的地
            // EntityManager.Instance.RemoveEntity(GetEntityId(), GetEntityInstanceId());
        }, () =>
        {
            // 移动到目标为止 （out area）
            //_attackSkillRangeBaseData.outAreaCall(_bulletEntity.GetEntityId(), _bulletEntity.GetEntityInstanceId());
        });
    }

    protected override void OnUpdate()
    {
        for(int i = entityShowCDs.Count - 1; i >= 0; i--)
        {
            entityShowCDs[i] -= Time.deltaTime;
            if(entityShowCDs[i] <= 0)
            {
                EntityManager.Instance.RemoveEntity(entityBases[i].GetEntityInstanceId());
                entityShowCDs.RemoveAt(i);
                entityBases.RemoveAt(i);
            }
        }
    }
}
