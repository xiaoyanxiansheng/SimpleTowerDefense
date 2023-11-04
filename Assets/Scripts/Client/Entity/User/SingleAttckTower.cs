using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(SingleAttckSkill))]
public class SingleAttckTower : TowerBase
{
    private SingleAttckSkill _singleAttckSkill;

    private void Awake()
    {
        _singleAttckSkill = GetComponent<SingleAttckSkill>();
    }

    private void OnEnable()
    {
        AttackSkillSingleBaseData data = new AttackSkillSingleBaseData();
        data.attackEntity = this;
        data.skillId = 30001;
        data.skillLevel = 1;
        data.attackDistance = 400;
        data.attackInterval = 0.5f;
        data.flySpeed = 4;
        data.startPos = GetPosition();
        _singleAttckSkill.Play(data);
    }
}
