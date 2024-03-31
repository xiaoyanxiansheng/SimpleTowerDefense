using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleConfig
{
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
    public BattleWaysConfig battleWaysConfig;
    public BattleEnemyConfig battleEnemyConfig;
}