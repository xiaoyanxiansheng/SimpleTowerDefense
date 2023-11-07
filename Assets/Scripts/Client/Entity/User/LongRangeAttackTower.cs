using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SingleAttckSkill))]
public class LongRangeAttackTower : TowerBase
{
    private SingleAttckSkill _singleAttckSkill;
    private NearRangeAttckSkill _nearRangeAttackSkill;

    private void Awake()
    {
        _singleAttckSkill = GetComponent<SingleAttckSkill>();
        _nearRangeAttackSkill = gameObject.AddComponent<NearRangeAttckSkill>();
    }

    protected override void OnRealEnable()
    {
        SingleAttckSkillData data = new SingleAttckSkillData();
        data.attackEntity = this;
        data.skillId = 30001;
        data.skillLevel = 1;
        data.attackDistance = 400;
        data.attackInterval = 2f;
        data.flySpeed = 4;
        data.startPos = GetPosition();
        data.hitCall = (EntityBase attackEntity, EntityBase hitEntity) =>
        {
            NearRangeAttckSkillData data = new NearRangeAttckSkillData();
            data.attackEntity = this;
            data.skillId = 30004;
            data.skillLevel = 1;
            data.attackDistance = 150;
            data.attackInterval = 1f;
            data.startPos = hitEntity.GetPosition();
            data.rangeDistance = 300f;
            data.continueTime = 1f;
            _nearRangeAttackSkill.Play(data);
        };
        _singleAttckSkill.Play(data);
    }
}
