using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SkillType
{
    None,
    SkillSingleOne,
    SkillSingleOneContinue,
    SkillRangeCircle,
}

public enum SkillLockType
{
    None,
    Enemy,
    Team,
}

public enum SkillReleaseType 
{ 
    None,
    Point,
    Dir,
    Follow
}

public enum SkillRangeType
{
    None,
    Circle,
    Rect
}

public enum SkillEffectType
{
    None = 0,

    Health,
    MoveSpeed,
}

public enum AddType
{
    None = 0,
    Add = 1,
    Reduce = 2,
}

public enum SkillEffectValueType
{
    None = 0,
    Value,      // 数值星
    Percent     // 百分比
}

[Serializable]
public class SKillConfigDataComboItem
{
    public string desc = "";
    public int skillId = -1;
    public int skillLevel = -1;
    public int entityId = -1;
    public SkillType skillType = SkillType.None;
    public SkillLockType skillLockType = SkillLockType.None;
    public SkillReleaseType skillReleaseType = SkillReleaseType.None;
    public SkillRangeType skillRangeType = SkillRangeType.None;
    public int attackNum = -1; 
    public float speed = -1;
    public float distanceX = -1;
    public float distanceY = -1;
    public float intervalTime = -1;
    public float continueTime = -1;
    public float intervalHitTime = -1;
    public List<SKillEffectConfigDataItem> effects = new List<SKillEffectConfigDataItem>();

    public void InitData(SKillConfigDataComboItem item1 , SKillConfigDataComboItem item2)
    {
        skillId = item2.skillId;
        skillLevel = item1.skillLevel != -1 ? item1.skillLevel : item2.skillLevel;
        entityId = item1.entityId != -1 ? item1.entityId : item2.entityId;
        skillType = item1.skillType != SkillType.None ? item1.skillType : item2.skillType;
        skillLockType = item1.skillLockType != SkillLockType.None ? item1.skillLockType: item2.skillLockType;
        skillReleaseType = item1.skillReleaseType != SkillReleaseType.None ? item1.skillReleaseType: item2.skillReleaseType;
        skillRangeType = item1.skillRangeType != SkillRangeType.None ? item1.skillRangeType: item2.skillRangeType;
        attackNum = item1.attackNum != -1 ? item1.attackNum: item2.attackNum;
        speed = item1.speed != -1 ? item1.speed: item2.speed;
        distanceX = item1.distanceX != -1 ? item1.distanceX: item2.distanceX;
        distanceY = item1.distanceY != -1 ? item1.distanceY: item2.distanceY;
        intervalTime = item1.intervalTime != -1 ? item1.intervalTime: item2.intervalTime;
        continueTime = item1.continueTime != -1 ? item1.continueTime: item2.continueTime;
        intervalHitTime = item1.intervalHitTime != -1 ? item1.intervalHitTime: item2.intervalHitTime;
        if (item1.effects.Count == 0)
        {
            effects = item2.effects;
        }
        else if (item1.effects[0].skillEffectType != SkillEffectType.None)
        {
            effects = item1.effects;
        }
    }
}

[Serializable]
public class SKillEffectConfigDataItem
{
    public string desc = "";
    public SkillEffectValueType skillEffectValueType = SkillEffectValueType.None;
    public SkillEffectType skillEffectType = SkillEffectType.None;
    public AddType addType = AddType.None;
    public float value = -1;
    public float continueTime = -1;
    public float intervalTime = -1;
}

[Serializable]
public class SKillConfigDataCombo
{
    public string desc = "";
    public List<SKillConfigDataComboItem> skills = new List<SKillConfigDataComboItem>();
}

[Serializable]
public class SKillConfigData
{
    public string desc = "技能描述";
    public int skillId = 0;
    public List<SKillConfigDataCombo> skillLevels = new List<SKillConfigDataCombo>();
}

[Serializable]
public class SkillEffectData
{
    public string desc = "技能效果描述";
    public int effectId = 0;
    public List<SKillEffectConfigDataItem> effects;
}

[CreateAssetMenu(fileName = "SkillConfig", menuName = "Config/SkillConfig", order = 1)]
public class SkillConfig : ScriptableObject
{
    [SerializeField]
    public List<SKillConfigData> SkillConfigDatas = new List<SKillConfigData>();

    public SKillConfigDataCombo GetSKillConfigDataCombo(int skillId , int skillLevelId)
    {
        int index = skillLevelId - 1;
        for (int i = 0; i < SkillConfigDatas.Count; i++)
        {
            if (SkillConfigDatas[i].skillId == skillId)
                return SkillConfigDatas[i].skillLevels[index];
        }
        return null;
    }

    public int GetSkillIndexCount(int skillId, int skillLevelId)
    {
        SKillConfigDataCombo combo = GetSKillConfigDataCombo(skillId, skillLevelId);
        return combo.skills.Count;
    }

    public SKillConfigDataComboItem GetSKillConfigDataComboItem(int skillId , int skillLevelId , int skillIndex)
    {
        if (skillIndex > GetSkillIndexCount(skillId, skillLevelId)) return null;

        SKillConfigDataComboItem rComboItem;

        SKillConfigDataCombo combo = GetSKillConfigDataCombo(skillId , skillLevelId);
        SKillConfigDataComboItem comboItem = combo.skills[skillIndex];
        if (comboItem.skillId != skillId)
        {
            rComboItem = new SKillConfigDataComboItem();
            rComboItem.InitData(comboItem,GetSKillConfigDataComboItem(comboItem.skillId, skillLevelId, 0));
        }
        else
        {
            rComboItem = comboItem;
        }
        return rComboItem;
    }
}

[CreateAssetMenu(fileName = "SkillEffect", menuName = "Config/SkillEffect", order = 1)]
public class SkillEffect : ScriptableObject
{
    [SerializeField]
    public List<SkillEffectData> skillEffectConfigDatas = new List<SkillEffectData>();

    public SKillEffectConfigDataItem GetSkillEffectConfigData(int effectId, int effectLevel)
    {
        for (int i = 0; i < skillEffectConfigDatas.Count; i++)
        {
            if (skillEffectConfigDatas[i].effectId == effectId)
                return skillEffectConfigDatas[i].effects[effectLevel-1];
        }
        return null;
    }
}