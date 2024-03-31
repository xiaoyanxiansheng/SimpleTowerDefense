using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillConfig", menuName = "Config/SkillConfig", order = 1)]
public class SkillConfig : ScriptableObject
{

    [SerializeField]
    public List<Config> configs = new List<Config>();

    public Config GetSkillConfigData(int skillId)
    {
        for (int i = 0; i < configs.Count; i++)
        {
            if (configs[i].skillId == skillId)
                return configs[i];
        }
        return null;
    }

    [Serializable]
    public class Config
    {
        public string desc;
        public int skillId;
        public ReadyConfig readyConfig;
        public TrackConfig trackConfig;
        public BuffConfig buffConfig;
    }

    [Serializable]
    public class ReadyConfig
    {
        public float continueTime;
        public string actionName;
        public string fxName;
    }

    [Serializable]
    public class TrackConfig
    {
        [Serializable]
        public class HitConfig
        {
            public enum EffectType
            {
                Buff,   // 包括伤害
                Skill,  // 释放新技能
            }
            [Serializable]
            public class Effect
            {
                public EffectType counterEffectType;
                public float p0;
            }
            public float continueTime;
            public float invertalTime;
            public List<Effect> effects;
        }
        [Serializable]
        public class TranslationConfig
        {
            public float mulSpeed = 1;
            public enum TranslationType
            {
                Point,
                Follow,
            }
            public TranslationType type;
            public AnimationCurve curve;
        }
        [Serializable]
        public class RotationConfig
        {
            public float mulSpeed = 360;
            public AnimationCurve curve;
        }
        [Serializable]
        public class ScaleConfig
        {
            public float mulSpeed = 1;
            public AnimationCurve curve;
        }
        public bool IsLoop = false;
        public bool IsBreak = false;
        public string ShapeAssetPath;
        public bool isUseTranslation;
        public TranslationConfig translationConfig;
        public bool isUseRotation;
        public RotationConfig rotationConfig;
        public bool isUseScale;
        public ScaleConfig scaleConfig;
        public bool isUseHit;
        public HitConfig hitConfig;
    }

    [Serializable]
    public class BuffConfig
    {
        public int[] buffIds;
    }
}