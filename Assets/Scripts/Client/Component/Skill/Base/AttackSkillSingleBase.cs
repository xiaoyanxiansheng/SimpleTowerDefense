using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackSkillSingleBaseData : AttackSkillBaseData
{
    public float flySpeed = 1.0f;
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

        Vector2 dir = (entity.GetPosition() - attackSkillSingleBaseData.startPos).normalized;
        EntityBase bulletEntity = EntityManager.Instance.CreateEntity<EntityBase>(EntityType.SKill ,GetSkillId());
        DirMoveComponent dirMoveComponent = bulletEntity.GetComponent<DirMoveComponent>();
        // ÒÆ¶¯
        dirMoveComponent.SetPosition(attackSkillSingleBaseData.startPos);
        dirMoveComponent.Move(dir, attackSkillSingleBaseData.flySpeed, null,() =>
        {
            EntityManager.Instance.RemoveEntity(bulletEntity.GetEntityInstanceId());
        }, (EntityBase moveHitEntity) =>
        {
            if (attackSkillSingleBaseData.hitCall != null) 
            {
                EntityManager.Instance.RemoveEntity(bulletEntity.GetEntityInstanceId());
                attackSkillSingleBaseData.hitCall(bulletEntity, moveHitEntity);
            }
        });
    }
}
