using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerConfig", menuName = "Config/TowerConfig", order = 1)]
public class TowerConfig : ScriptableObject
{
    [Serializable]
    public class Skill
    {
        public int SkillId;
        public int SkillLevel;
        public bool IsContinue = false;
        public int SkillInvertal = 1;
        public int AttackDistance = 200;
        public int AttackNum = 1;
    }

    [Serializable]
    public class TowerConfigData
    {
        public string desc = "";
        public int TowerId = 0;
        public string prefabPath;
        public Skill skill;
    }

    [SerializeField]
    public List<TowerConfigData> TowerConfigDatas = new List<TowerConfigData>();

    public TowerConfigData GetTowerConfigData(int towerId)
    {
        for (int i = 0; i < TowerConfigDatas.Count; i++)
        {
            if (TowerConfigDatas[i].TowerId == towerId)
                return TowerConfigDatas[i];
        }
        return null;
    }
}