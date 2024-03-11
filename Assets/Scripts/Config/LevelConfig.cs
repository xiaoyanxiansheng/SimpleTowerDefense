using System;
using System.Collections.Generic;
using UnityEngine;
using static LevelConfigData.Enemy;

[Serializable]
public class LevelConfigData
{
    [Serializable]
    public class Enemy
    {
        [Serializable]
        public class Buff
        {
            public int BuffId;
            public int Level;
        }
        
        public string desc = "";
        public int entityId = 0;
        public int wayStartPointIndex = -1;  // 和wayStarts索引对应上 -1代表随随机
        public int wayEndPointIndex = -1; // 和wayEnds索引对应上 -1代表随随机

        [SerializeField]
        public List<Buff> buff;
    }
    [Serializable]
    public class WayList
    {
        public Vector2 Point;
    }
    [Serializable]
    public class WayStart
    {
        public Vector2 Point;
    }
    [Serializable]
    public class WayEnd
    {
        public Vector2 Point;
    }

    public string desc = "";
    public int LevelId = 0;
    [SerializeField]
    public List<WayList> wayList;
    [SerializeField]
    public List<int> wayStarts; // 和wayList索引对应上
    [SerializeField]
    public List<int> wayEnds;   // 和wayList索引对应上
    [SerializeField]
    public List<Enemy> Enemys = new List<Enemy>();
}

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Config/LevelConfig", order = 1)]
public class LevelConfig : ScriptableObject
{
    [SerializeField]
    public List<LevelConfigData> LevelConfigDatas = new List<LevelConfigData>();

    public LevelConfigData GetLevelConfigData(int LevelId)
    {
        for (int i = 0; i < LevelConfigDatas.Count; i++)
        {
            if (LevelConfigDatas[i].LevelId == LevelId)
                return LevelConfigDatas[i];
        }
        return null;
    }
}
