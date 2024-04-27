using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum CellType
{
    None = -1,  // 不可移动、放置
    Normal,     // 可移动可放置
    Move,       // 可移动、不可放置
    Place,       // 不可移动、可放置
}

public class CellData
{
    public CellType defaultCellType;

    public CellType CellType;
    public int X, Y;
    public int MoveWeight;
    public CellData(int x, int y, CellType cellType, int weight)
    {
        defaultCellType = cellType;X = x; Y = y;CellType = cellType; MoveWeight = weight;
    }
}

public class BattleCellData
{
    private bool _isFixcanWalkPoints;
    private List<int2> _canWalkPoints;
    private List<int2> _canPlacePoints;
    private BattleBase _battle;
    private Dictionary<int, Dictionary<int, CellData>> _cellDataMap = new Dictionary<int, Dictionary<int, CellData>>();

    private EntityAStarPath _entityAStarPath;

    public BattleCellData(bool isFixcanWalkPoints, List<int2> canWalkPoints , List<int2> canPlacePoints) 
    {
        _isFixcanWalkPoints = isFixcanWalkPoints;
        _canWalkPoints = canWalkPoints;
        _canPlacePoints = canPlacePoints;
        _entityAStarPath = new EntityAStarPath();
        Init();
    }

    private void Init()
    {
        _cellDataMap.Clear();
        for(int i = 0; i< Define.CELL_COUNT_WIDTH; i++)
        {
            if (!_cellDataMap.ContainsKey(i))
            {
                _cellDataMap[i] = new Dictionary<int, CellData>();
            }
            for (int j = 0; j < Define.CELL_COUNT_HEIGHT; j++)
            {
                _cellDataMap[i][j] = new CellData(i, j, CellType.None, 0);
                _cellDataMap[i][j].CellType = CellType.None;
            }
        }

        for(int i = 0; i< _canWalkPoints.Count; i++)
        {
            int2 p = _canWalkPoints[i];
            _cellDataMap[p.x][p.y].CellType = CellType.Move;            // 可移动
            _cellDataMap[p.x][p.y].defaultCellType = CellType.Move;
            _cellDataMap[p.x][p.y].MoveWeight = 100;
        }

        for (int i = 0; i < _canPlacePoints.Count; i++)
        {
            int2 p = _canPlacePoints[i];
            if(_cellDataMap[p.x][p.y].CellType == CellType.Move)
            {
                _cellDataMap[p.x][p.y].CellType = CellType.Normal;      // 可移动 可放置
                _cellDataMap[p.x][p.y].defaultCellType = CellType.Normal;
            }
            else
            {
                _cellDataMap[p.x][p.y].CellType = CellType.Place;       // 可放置
                _cellDataMap[p.x][p.y].defaultCellType = CellType.Place;
            }
            
        }
    }

    public void UpdateWeight(int x, int y, bool isPlace)
    {
        CellType t = _cellDataMap[x][y].CellType;
        _cellDataMap[x][y].CellType = isPlace ? CellType.None : _cellDataMap[x][y].defaultCellType;
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

        if (_isFixcanWalkPoints)
            pathPoints = _canWalkPoints;
        else
            _entityAStarPath.GetPathPoints(ref pathPoints, cellDatas, start, end);
    }

    public void GetMovePosPaths(ref List<Vector2> movePosPaths, Vector2 startPos, Vector2 targetPos)
    {
        movePosPaths.Clear();

        List<int2> movePointPaths = new List<int2>();   // TOTO 可优化
        GetMovePointPaths(ref movePointPaths, CommonUtil.VecConvertCell(startPos), CommonUtil.VecConvertCell(targetPos));
        if (movePointPaths.Count == 0) return;

        Vector2 p0 = CommonUtil.CellConvertVec(movePointPaths[0]);
        Vector2 p1 = CommonUtil.CellConvertVec(movePointPaths[1]);
        Vector2 pp = (startPos - p0) * (startPos - p1);
        movePosPaths.Add(startPos);
        int index = 1;
        if (pp.x + pp.y > 0.001f)
        {
            index = 0;
        }
        for (int i = index; i < movePointPaths.Count; i++)
        {
            movePosPaths.Add(CommonUtil.CellConvertVec(movePointPaths[i]));
        }
    }
}