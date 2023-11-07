using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SingleAttckSkill))]
public class SingleAttckTower : TowerBase
{
    private SingleAttckSkill _singleAttckSkill;
    private DirRangeAttackSkill _dirRangeAttackSkill;

    private void Awake()
    {
        _singleAttckSkill = GetComponent<SingleAttckSkill>();
        _dirRangeAttackSkill = gameObject.AddComponent<DirRangeAttackSkill>();
    }

    protected override void OnRealEnable()
    {
        SingleAttckSkillData data = new SingleAttckSkillData();
        data.attackEntity = this;
        data.skillId = 30001;
        data.skillLevel = 1;
        data.attackDistance = 400;
        data.attackInterval = 0.5f;
        data.flySpeed = 4;
        data.startPos = GetPosition();
        data.hitCall = (EntityBase attackEntity, EntityBase hitEntity) =>
        {
            // 移动到了目的地
            DirRangeAttackSkillData data = new DirRangeAttackSkillData();
            data.attackEntity = this;
            data.skillId = 30003;
            data.skillLevel = 1;
            data.attackDistance = 400;
            data.attackInterval = 100f;
            data.startPos = hitEntity.GetPosition();
            DirRangeAttackSkill dirRangeAttackSkill = gameObject.AddComponent<DirRangeAttackSkill>();
            dirRangeAttackSkill.Play(data);
        };
        _singleAttckSkill.Play(data);
    }
}
