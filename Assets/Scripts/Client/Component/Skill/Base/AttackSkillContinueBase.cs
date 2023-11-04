using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 持续单体
/// 显示上持续
/// 伤害上间隔：间隔很短，也是为了后面的技能组合
/// </summary>
public class AttackSkillContinueBaseData : AttackSkillBaseData
{
    public Vector2 startPos = Vector2.zero;
    public Action hitCall;
}

public class AttackSkillContinueBase : AttackSkillBase
{
    private AttackSkillContinueBaseData attackSkillContinueBaseData = null;
    protected EntityBase _bulletEntity = null;

    public void Play(AttackSkillContinueBaseData attackSkillContinueBaseData)
    {
        this.attackSkillContinueBaseData = attackSkillContinueBaseData;
        base.Play(attackSkillContinueBaseData);

        // 创建显示实体
        if (_bulletEntity == null)
        {
            _bulletEntity = EntityManager.Instance.CreateEntity<EntityBase>(EntityType.SKill, GetSkillId());
        }
    }

    protected override bool OnCheckAttackUpdate()
    {
        return CheckAttackOne();
    }

    protected override void OnAttack()
    {
        Debug.Log("攻击");
    }

    protected override void OnUpdate()
    {
        EntityBase entity = GetHitEntityOne();
        if(entity == null) { return; }

        Vector2 dir = (entity.GetPosition() - GetPosition()).normalized;
        float length = Vector2.Distance(entity.GetPosition(), GetPosition());

        PointMoveComponent pointMoveComponent = _bulletEntity.GetComponent<PointMoveComponent>();
        // 移动
        pointMoveComponent.Move(GetAttackEntity().GetPosition(),entity.GetPosition(), () =>
        {
            // 移动到了目的地
            // EntityManager.Instance.RemoveEntity(GetEntityId(), GetEntityInstanceId());
        }, () =>
        {
            // 移动到目标为止 （out area）
            //attackSkillContinueBaseData.outAreaCall(_bulletEntity.GetEntityId(), _bulletEntity.GetEntityInstanceId());
        });
    }
}
