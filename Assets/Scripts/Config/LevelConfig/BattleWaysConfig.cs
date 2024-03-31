
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Mathematics;

public enum CellType
{
    None = -1,  // 不可移动、放置
    Normal,     // 可移动可放置
    Move,       // 可移动、不可放置
    Play,       // 不可移动、可放置
}

public enum CellStartEndType
{
    None = -1,
    Start,
    End
}

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Config/LevelConfig", order = 1)]
public class LevelConfig : ScriptableObject
{
    [SerializeField]
    public List<BattleConfig> LevelConfigDatas = new List<BattleConfig>();

    public BattleConfig GetBattleConfig(int LevelId)
    {
        for (int i = 0; i < LevelConfigDatas.Count; i++)
        {
            if (LevelConfigDatas[i].LevelId == LevelId)
                return LevelConfigDatas[i];
        }
        return null;
    }
}

[Serializable]
public class BattleWaysConfigData
{
    public CellStartEndType cellStartEndType = CellStartEndType.None;
    public CellType CellType = CellType.Normal;
    public int2 Point;
    public int MoveWeight = 100;
}

[CreateAssetMenu(fileName = "LevelWaysConfig", menuName = "Config/LevelWaysConfig", order = 1)]
public class BattleWaysConfig : ScriptableObject
{
    [SerializeField]
    public List<BattleWaysConfigData> BattleWaysConfigDatas = new List<BattleWaysConfigData>();

    public BattleWaysConfigData GetBattleWaysConfigData(int x , int y)
    {
        foreach(var item in BattleWaysConfigDatas)
        {
            if (item.Point.x == x && item.Point.y == y) return item;
        }
        return null;
    }
}