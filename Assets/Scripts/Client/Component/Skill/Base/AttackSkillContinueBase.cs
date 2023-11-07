using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
/// ��ʾ�ϳ���
/// �˺��ϼ��������̣ܶ�Ҳ��Ϊ�˺���ļ������
/// </summary>
public class AttackSkillContinueBaseData : AttackSkillBaseData
{
    
}

public class AttackSkillContinueBase : AttackSkillBase
{
    private AttackSkillContinueBaseData _attackSkillContinueBaseData = null;
    protected EntityBase _bulletEntity = null;

    public void Play(AttackSkillContinueBaseData attackSkillContinueBaseData)
    {
        _attackSkillContinueBaseData = attackSkillContinueBaseData;
        base.Play(attackSkillContinueBaseData);

        // ������ʾʵ��
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
        //Debug.Log("����");
    }

    protected override void OnUpdate()
    {
        EntityBase entity = GetHitEntityOne();
        if(entity == null) { return; }

        Vector2 dir = (entity.GetPosition() - _attackSkillContinueBaseData.startPos).normalized;
        float length = Vector2.Distance(entity.GetPosition(), _attackSkillContinueBaseData.startPos);

        PointMoveComponent pointMoveComponent = _bulletEntity.GetComponent<PointMoveComponent>();
        // �ƶ�
        pointMoveComponent.Move(_attackSkillContinueBaseData.startPos, entity.GetPosition(), () =>
        {
            // �ƶ�����Ŀ�ĵ�
            // EntityManager.Instance.RemoveEntity(GetEntityId(), GetEntityInstanceId());
        }, () =>
        {
            // �ƶ���Ŀ��Ϊֹ ��out area��
            //attackSkillContinueBaseData.outAreaCall(_bulletEntity.GetEntityId(), _bulletEntity.GetEntityInstanceId());
        });
    }
}
