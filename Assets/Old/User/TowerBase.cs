//using UnityEngine;

//public class TowerBase : BattleEntityBase
//{
//    public void DoSkill()
//    {
//        DoSkill(GetPosition());
//    }

//    public void DoSkillFollow()
//    {
//        DoSkill(this);
//    }

//    public void DoSkill(Vector2 position)
//    { 
//        EntityConfigData config = GameApp.Instance.GetEntityConfig(GetEntityId());
//        NewSkillBase skill = NewSkillBase.CreateSKill(gameObject, config.SkillId, config.SkillLevel);
//        skill.Play(position);
//    }

//    public void DoSkill(BattleEntityBase attackEnity)
//    {
//        EntityConfigData config = GameApp.Instance.GetEntityConfig(GetEntityId());
//        NewSkillBase skill = NewSkillBase.CreateSKill(gameObject, config.SkillId, config.SkillLevel);
//        skill.Play(attackEnity);
//    }
//}
