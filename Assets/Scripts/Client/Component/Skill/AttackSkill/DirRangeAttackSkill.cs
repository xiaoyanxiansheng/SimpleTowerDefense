using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirRangeAttackSkillData : AttackSkillBaseData { }

public class DirRangeAttackSkill : AttackSkillBase
{
    private DirRangeAttackSkillData _dirRangeAttackSkillData = null;
    private List<EntityBase> entityBases = new List<EntityBase>();
    private List<float> entityShowCDs = new List<float>();

    public void Play(DirRangeAttackSkillData dirRangeAttackSkillData)
    {
        _dirRangeAttackSkillData = dirRangeAttackSkillData;
        base.Play(_dirRangeAttackSkillData);
    }

    protected override bool OnCheckAttackUpdate()
    {
        return CheckAttackOne();
    }

    protected override void OnAttack()
    {
        EntityBase entity = GetHitEntityOne();
        if (entity == null) { return; }

        Vector2 dir = (entity.GetPosition() - _dirRangeAttackSkillData.startPos).normalized;

        EntityBase bulletEntity = EntityManager.Instance.CreateEntity<EntityBase>(EntityType.SKill, GetSkillId());
        entityBases.Add(bulletEntity);
        entityShowCDs.Add(0.2f);    //TODO
        PointMoveComponent pointMoveComponent = bulletEntity.GetComponent<PointMoveComponent>();
        // 移动
        pointMoveComponent.MoveDir(_dirRangeAttackSkillData.startPos, dir, () =>
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
        for (int i = entityShowCDs.Count - 1; i >= 0; i--)
        {
            entityShowCDs[i] -= Time.deltaTime;
            if (entityShowCDs[i] <= 0)
            {
                EntityManager.Instance.RemoveEntity(entityBases[i].GetEntityInstanceId());
                entityShowCDs.RemoveAt(i);
                entityBases.RemoveAt(i);
            }
        }
    }
}
