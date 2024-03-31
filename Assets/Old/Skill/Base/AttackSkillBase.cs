//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AttackSkillBaseData : SkillBaseData
//{
//    public Vector2 startPos;
//    public Action<BattleEntityBase,BattleEntityBase> hitCall;
//}

//public class AttackSkillBase : SkillBase
//{
//    private AttackSkillBaseData _attackSkillBaseData;

//    public void Play(AttackSkillBaseData attackSkillBaseData)
//    {
//        _attackSkillBaseData = attackSkillBaseData;
//        base.Play(attackSkillBaseData);
//    }

//    protected bool CheckAttackOne()
//    {
//        BattleEntityBase hitEntity = GetHitEntityOne();
        
//        if(hitEntity != null)
//        {
//            if (Vector2.Distance(hitEntity.GetPosition(), _attackSkillBaseData.startPos) > GetAttackDistance())
//            {
//                hitEntity = null;
//            }
//        }

//        if (hitEntity == null)
//        {
//            hitEntity = GameApp.Instance.entityManager.GetEntityByDistance(EntityType.Enemy, _attackSkillBaseData.startPos, GetAttackDistance());
//            if(hitEntity != null)
//            {
//                hitEntitys.Clear();
//                hitEntitys.Add(hitEntity);
//            }
//        }

//        return hitEntity != null;
//    }

//    protected BattleEntityBase GetHitEntityOne()
//    {
//        ClearNullReference();
//        if (hitEntitys.Count == 0) return null;
//        return hitEntitys[0];
//    }

//    protected bool CheckAttckList()
//    {
//        //hitEntitys = GameApp.Instance.entityManager.GetEntityByDistances(EntityType.Enemy, _attackSkillBaseData.startPos, GetAttackDistance());
//        //return hitEntitys.Count > 0;
//        return false;
//    }

//    protected List<BattleEntityBase> GetHitEntityList()
//    {
//        return hitEntitys;
//    }
//}
