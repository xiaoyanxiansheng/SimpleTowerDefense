//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class AttackSkillSingleBaseData : AttackSkillBaseData
//{
//    public float flySpeed = 1.0f;
//}

//public class AttackSkillSingleBase : AttackSkillBase
//{
//    private AttackSkillSingleBaseData attackSkillSingleBaseData;

//    public void Play(AttackSkillSingleBaseData attackSkillSingleBaseData)
//    {
//        this.attackSkillSingleBaseData = attackSkillSingleBaseData;
//        base.Play(attackSkillSingleBaseData);
//    }

//    protected override bool OnCheckAttackUpdate()
//    {
//        return CheckAttackOne();
//    }

//    protected override void OnAttack()
//    {
//        BattleEntityBase entity = GetHitEntityOne();
//        if (entity == null) { return; }

//        Vector2 dir = (entity.GetPosition() - attackSkillSingleBaseData.startPos).normalized;
//        BattleEntityBase bulletEntity = GameApp.Instance.entityManager.CreateEntity<BattleEntityBase>(EntityType.SKill ,GetSkillId());
//        DirMoveComponent dirMoveComponent = bulletEntity.GetComponent<DirMoveComponent>();
//        // ÒÆ¶¯
//        dirMoveComponent.SetPosition(attackSkillSingleBaseData.startPos);
//        dirMoveComponent.Move(dir, attackSkillSingleBaseData.flySpeed, null,() =>
//        {
//            GameApp.Instance.entityManager.RemoveEntity(bulletEntity.GetEntityInstanceId());
//        }, (BattleEntityBase moveHitEntity) =>
//        {
//            if (attackSkillSingleBaseData.hitCall != null) 
//            {
//                GameApp.Instance.entityManager.RemoveEntity(bulletEntity.GetEntityInstanceId());
//                attackSkillSingleBaseData.hitCall(bulletEntity, moveHitEntity);
//            }
//        });
//    }
//}
