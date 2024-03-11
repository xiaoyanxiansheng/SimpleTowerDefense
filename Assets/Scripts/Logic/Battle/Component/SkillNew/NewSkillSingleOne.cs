using System.Collections.Generic;

using UnityEngine;

public class NewSkillSingleOne : NewSkillBase
{
    private Dictionary<SkillHit, EntityBase> _skillHitEntitys = new Dictionary<SkillHit, EntityBase>();

    protected override void OnHitStart(SkillHit skillHit)
    {
        base.OnHitStart(skillHit);

        EntityBase bullet = GameApp.Instance.entityManager.CreateEntity<EntityBase>(EntityType.SKill, GetSkillConfig().entityId);
        _skillHitEntitys[skillHit] = bullet;
    }

    protected override void OnHit(SkillHit skillHit , EntityBase entity)
    {
        base.OnHit(skillHit, entity);
        if (_skillHitEntitys.ContainsKey(skillHit))
        {
            GameApp.Instance.entityManager.RemoveEntity(_skillHitEntitys[skillHit].GetEntityInstanceId());
            _skillHitEntitys.Remove(skillHit);
        }
    }

    protected override void OnHitUpdate(SkillHit skillHit)
    {
        base.OnHitUpdate(skillHit);

        if (_skillHitEntitys.ContainsKey(skillHit))
        {
            _skillHitEntitys[skillHit].SetPosition(skillHit.GetCurPos());
        }
    }
}
