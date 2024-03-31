//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AttackSkillRangeBaseData : AttackSkillBaseData
//{
//    public float rangeDistance = 1f;
//    public float continueTime = 0f;
//}

//public class AttackSkillRangeBase : AttackSkillBase
//{
//    private AttackSkillRangeBaseData _attackSkillRangeBaseData = null;
//    private List<BattleEntityBase> entityBases = new List<BattleEntityBase>();
//    protected BattleEntityBase _bulletEntity = null;
//    private float _continueTimeCount = 0;

//    public void Play(AttackSkillRangeBaseData attackSkillRangeBaseData)
//    {
//        _attackSkillRangeBaseData = attackSkillRangeBaseData;
//        base.Play(_attackSkillRangeBaseData);

//        _continueTimeCount = _attackSkillRangeBaseData.continueTime;
//    }

//    protected override bool OnCheckAttackUpdate()
//    {
//        return CheckAttckList();
//    }

//    protected override void OnAttack()
//    {
//        List<BattleEntityBase> entity = GetHitEntityList();
//        if (entity.Count == 0) { return; }
//        // 创建显示实体
//        if (_bulletEntity == null)
//        {
//            _bulletEntity = GameApp.Instance.entityManager.CreateEntity<BattleEntityBase>(EntityType.SKill, GetSkillId());
//        }
//        _bulletEntity.SetPosition(_attackSkillRangeBaseData.startPos);
//        float s = _attackSkillRangeBaseData.rangeDistance / Define.CELL_SIZE;
//        _bulletEntity.transform.localScale = Vector3.one * s;
//    }

//    protected override void OnUpdate()
//    {
//        if (_attackSkillRangeBaseData.continueTime == 0) return;

//        if(_continueTimeCount <= 0)
//        {
//            if(_bulletEntity != null)
//                GameApp.Instance.entityManager.RemoveEntity(_bulletEntity.GetEntityInstanceId());
//        }
//        _continueTimeCount -= Time.deltaTime;
//    }
//}
