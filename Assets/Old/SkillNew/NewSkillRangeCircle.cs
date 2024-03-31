//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class NewSkillRangeCircle : NewSkillBase
//{
//    private BattleEntityBase _bullet = null;

//    public override void InitSkill(int skillId, int skillLevel, int skillIndex)
//    {
//        base.InitSkill(skillId, skillLevel, skillIndex);
//        _bullet = GameApp.Instance.entityManager.CreateEntity<BattleEntityBase>(EntityType.SKill, GetSkillConfig().entityId);
//    }

//    private void OnDestroy()
//    {
//        if (_bullet)
//            GameApp.Instance.entityManager.RemoveEntity(_bullet.GetEntityInstanceId());
//        _bullet = null;
//    }

//    protected override void OnAttackEnd(SkillHit skillHit)
//    {
//        if (GetContinueTime() != 0)
//        {
//            OnDestroy();
//        }
//    }

//    protected override bool CheckAttack()
//    {
//        BattleEntityBase followEntity = GameApp.Instance.entityManager.GetEntityByDistance(EntityType.Enemy, GetPosition(), GetDistance());
//        SetFollowEntity(followEntity);
//        return followEntity != null;
//    }

//    protected override void OnAttackUpdate()
//    {
//        base.OnAttackUpdate();

//        if (_bullet == null) return;

//        _bullet.gameObject.SetActive(GetFollowEntity());

//        if (!GetFollowEntity()) return;

//        float distance = GetDistance() / Define.CELL_SIZE;
//        _bullet.SetPosition(GetPosition());
//        _bullet.transform.localScale = new Vector3(distance,distance, distance);
//    }
//}
