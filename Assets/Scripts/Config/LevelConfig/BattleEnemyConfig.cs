using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Mathematics;

[Serializable]
public class BattleEntityConfigData
{
    [Serializable]
    public class Buff
    {
        public int BuffId;
        public int Level;
    }

    public string desc = "";
    public int entityId = 0;
    public float enterTime = 0;         // ����ʱ��
    public int2 startPoint;             // ��wayStarts������Ӧ��
    public int2 endPoint;               // ��wayEnds������Ӧ��

    [SerializeField]
    public List<Buff> buff;
}

[CreateAssetMenu(fileName = "LevelEnemyConfig", menuName = "Config/LevelEnemyConfig", order = 1)]
public class BattleEnemyConfig : ScriptableObject
{
    [SerializeField]
    public List<BattleEntityConfigData> battleEnemyConfigDatas = new List<BattleEntityConfigData>();
    public BattleEntityConfigData GetBattleEntityConfigData(int EntityId)
    {
        for (int i = 0; i < battleEnemyConfigDatas.Count; i++)
        {
            if (battleEnemyConfigDatas[i].entityId == EntityId)
                return battleEnemyConfigDatas[i];
        }
        return null;
    }
}