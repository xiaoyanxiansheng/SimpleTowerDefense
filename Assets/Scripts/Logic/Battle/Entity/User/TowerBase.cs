using UnityEngine;

public class TowerBase : EntityBase
{
    public void DoSkill()
    {
        DoSkill(GetPosition());
    }

    public void DoSkillFollow()
    {
        DoSkill(this);
    }

    public void DoSkill(Vector2 position)
    { 
        EntityConfigData config = GameApp.Instance.GetEntityConfig(GetEntityId());
        NewSkillBase skill = NewSkillBase.CreateSKill(gameObject, config.SkillId, config.SkillLevel);
        skill.Play(position);
    }

    public void DoSkill(EntityBase attackEnity)
    {
        EntityConfigData config = GameApp.Instance.GetEntityConfig(GetEntityId());
        NewSkillBase skill = NewSkillBase.CreateSKill(gameObject, config.SkillId, config.SkillLevel);
        skill.Play(attackEnity);
    }
}
