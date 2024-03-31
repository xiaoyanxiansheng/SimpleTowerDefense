using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum MovePathType
{
    None = -1,  // 出生之后不移动
    Common = 0, // 沿路径正常移动
}

public class CellData
{
    public CellType CellType;
    public int X, Y;
    public int MoveWeight;
    public CellData(int x, int y, int weight)
    {
        X = x; Y = y; MoveWeight = weight;
    }
}

public class BattleCellData
{
    private BattleWaysConfig _battleWaysConfig;
    private BattleBase _battle;
    private Dictionary<int, Dictionary<int, CellData>> _cellDataMap = new Dictionary<int, Dictionary<int, CellData>>();

    private EntityAStarPath _entityAStarPath;

    public BattleCellData(BattleWaysConfig battleWaysConfig) 
    {
        _battleWaysConfig = battleWaysConfig;
        _entityAStarPath = new EntityAStarPath();
        Init();
    }

    private void Init()
    {
        _cellDataMap.Clear();
        // 配置表中获取可行走区域和可放置防御塔区域
        for (int i = 0; i < _battleWaysConfig.BattleWaysConfigDatas.Count; i++)
        {
            BattleWaysConfigData configData = _battleWaysConfig.BattleWaysConfigDatas[i];
            if (!_cellDataMap.ContainsKey(configData.Point.x))
            {
                _cellDataMap[configData.Point.x] = new Dictionary<int, CellData>();
            }
            CellData data = new CellData(configData.Point.x, configData.Point.y, configData.MoveWeight);
             data.CellType = configData.CellType;
            _cellDataMap[configData.Point.x][configData.Point.y] = data;
        }


    }

    public void UpdateWeight(int x, int y, bool isPlay)
    {
        CellType t = _cellDataMap[x][y].CellType;
        if (t == CellType.Normal || t == CellType.Move)
        {
            if (isPlay)
            {
                _cellDataMap[x][y].MoveWeight = Define.CELL_MAXWEIGHT;
            }
            else
            {
                _cellDataMap[x][y].MoveWeight = _battleWaysConfig.GetBattleWaysConfigData(x, y).MoveWeight;
            }
        }
    }

    public void GetMovePointPaths(ref List<int2> pathPoints ,int2 start , int2 end)
    {
        List<CellData> cellDatas = new List<CellData>();
        foreach (var values in _cellDataMap.Values)
        {
            foreach (var cellData in values)
            {
                cellDatas.Add(cellData.Value);
            }
        }
        _entityAStarPath.GetPathPoints(ref pathPoints, cellDatas, start, end);
    }

    public int2 GetPointByPointIndex(int wayPointIndex)
    {
        BattleWaysConfigData config = _battleWaysConfig.BattleWaysConfigDatas[wayPointIndex];
        return config.Point;
    }
}