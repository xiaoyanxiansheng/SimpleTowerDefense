using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirRangeAttackSkill))]
public class SingleAttckDirRangeTower : TowerBase
{

    private DirRangeAttackSkill _dirRangeAttackSkill;

    private void Awake()
    {
        _dirRangeAttackSkill = GetComponent<DirRangeAttackSkill>();
    }

    private void OnEnable()
    {
        AttackSkillRangeBaseData data = new AttackSkillRangeBaseData();
        data.attackEntity = this;
        data.skillId = 30003;
        data.skillLevel = 1;
        data.attackDistance = 400;
        data.attackInterval = 0.1f;
        _dirRangeAttackSkill.Play(data);
    }
}
