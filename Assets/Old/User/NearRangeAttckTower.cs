//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[RequireComponent(typeof(NearRangeAttckSkill))]
//public class NearRangeAttckTower : TowerBase
//{
//    private NearRangeAttckSkill attackSkill;

//    private void Awake()
//    {
//        attackSkill = GetComponent<NearRangeAttckSkill>();
//    }

//    protected override void OnRealEnable()
//    {
//        NearRangeAttckSkillData data = new NearRangeAttckSkillData();
//        data.attackEntity = this;
//        data.skillId = 30004;
//        data.skillLevel = 1;
//        data.baseAttackDistance = 150;
//        data.baseAttackInterval = 0.2f;
//        data.startPos = GetPosition();
//        data.rangeDistance = 300f;
//        attackSkill.Play(data);
//    }
//}
