using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkillSingleBaseData : AttackSkillBaseData
{
    public float flySpeed = 1.0f;
    public Vector2 startPos = Vector2.zero;
}

public class AttackSkillSingleBase : AttackSkillBase
{
    private AttackSkillSingleBaseData attackSkillSingleBaseData;

    public void Play(AttackSkillSingleBaseData attackSkillSingleBaseData)
    {
        this.attackSkillSingleBaseData = attackSkillSingleBaseData;
        base.Play(attackSkillSingleBaseData);
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
        EntityBase bulletEntity = EntityManager.Instance.CreateEntity<EntityBase>(EntityType.SKill ,GetSkillId());
        DirMoveComponent dirMoveComponent = bulletEntity.GetComponent<DirMoveComponent>();
        // 移动
        dirMoveComponent.SetPosition(GetPosition());
        dirMoveComponent.Move(dir, attackSkillSingleBaseData.flySpeed, () =>
        {
            // 移动到了目的地
            // EntityManager.Instance.RemoveEntity(GetEntityId(), GetEntityInstanceId());
        },() =>
        {
            EntityManager.Instance.RemoveEntity(bulletEntity.GetEntityInstanceId());
        });
    }
}
