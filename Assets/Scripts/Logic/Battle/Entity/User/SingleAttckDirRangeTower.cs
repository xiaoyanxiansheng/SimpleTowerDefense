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

    protected override void OnRealEnable()
    {
        DirRangeAttackSkillData data = new DirRangeAttackSkillData();
        data.attackEntity = this;
        data.skillId = 30003;
        data.skillLevel = 1;
        data.baseAttackDistance = 400;
        data.baseAttackInterval = 1f;
        data.startPos = GetPosition();
        _dirRangeAttackSkill.Play(data);
    }
}
