using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearRangeAttckSkillData : AttackSkillRangeBaseData
{
}

public class NearRangeAttckSkill : AttackSkillRangeBase
{
    private NearRangeAttckSkillData _nearRangeAttckSkillData = null;
    private List<EntityBase> entityBases = new List<EntityBase>();
    private List<float> entityShowCDs = new List<float>();

    public void Play(NearRangeAttckSkillData nearRangeAttckSkillData)
    {
        _nearRangeAttckSkillData = nearRangeAttckSkillData;
        base.Play(_nearRangeAttckSkillData);
    }
}
