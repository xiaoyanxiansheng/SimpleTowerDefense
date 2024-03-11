using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ContinueAttackSkill))]
public class ContinueSingleAttackTower : TowerBase
{
    private ContinueAttackSkill _continueAttackSkill;

    private void Awake()
    {
        _continueAttackSkill = GetComponent<ContinueAttackSkill>();
    }

    protected override void OnRealEnable()
    {
        AttackSkillContinueBaseData data = new AttackSkillContinueBaseData();
        data.attackEntity = this;
        data.skillId = 30002;
        data.skillLevel = 1;
        data.baseAttackDistance = 400;
        data.baseAttackInterval = 0.01f;
        data.startPos = GetPosition();
        _continueAttackSkill.Play(data);
    }
}
