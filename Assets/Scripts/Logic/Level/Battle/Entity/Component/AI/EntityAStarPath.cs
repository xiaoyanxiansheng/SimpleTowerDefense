/*
    只计算路径
 */
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EntityAStarPath
{
    public class Item
    {
        public int2 point;
        public Item father;
        public int G;
        public int H;
        public int weight;
    }

    private int2 _startPoint;
    private int2 _targetPoint;

    private List<Item> _openList = new List<Item>();
    private List<Item> _closeList = new List<Item>();
    private Dictionary<int, Dictionary<int, int>> _weightList = new Dictionary<int, Dictionary<int, int>>();

    private List<int2> _dirs = new List<int2>() { new int2(0, 1), new int2(1, 0), new int2(0, -1), new int2(-1, 0) };

    public EntityAStarPath()
    {
        
    }

    public void ClearWeight()
    {
        _weightList.Clear();
        for (int i = 0; i < Define.CELL_COUNT_WIDTH; i++)
        {
            _weightList[i] = new Dictionary<int, int>();
            for (int j = 0; j < Define.CELL_COUNT_HEIGHT; j++)
            {
                _weightList[i][j] = Define.CELL_MAXWEIGHT;
            }
        }
    }

    public void GetPathPoints(ref List<int2> pathPoints , List<CellData> cellDatas, int2 start , int2 end)
    {
        ClearWeight();
        foreach (CellData cellData in cellDatas)
        {
            int moveWieght = cellData.MoveWeight;
            if (cellData.CellType != CellType.Move) moveWieght = Define.CELL_MAXWEIGHT;
            _weightList[cellData.X][cellData.Y] = moveWieght;
        }

        GetOptimalRoute(start, end, ref pathPoints);
    }

    public bool GetOptimalRoute(int2 startPoint, int2 targetPoint, ref List<int2> route)
    {
        if (startPoint.x == targetPoint.x && startPoint.y == targetPoint.y) return false;

        _startPoint = startPoint;
        _targetPoint = targetPoint;
        _openList.Clear();
        _closeList.Clear();

        // 1 起始点放入开放列表
        Item startItem = GetItem(_startPoint, null);
        _openList.Add(startItem);
        while (true)
        {
            Item item = GetMinGH();
            if (item == null) break;

            _openList.Remove(item);
            _closeList.Add(item);

            AddRoundPoint(item);

            if (CalcuRoute(ref route))
            {
                break;
            }
        }

        return false;
    }

    private bool CalcuRoute(ref List<int2> route)
    {
        route.Clear();

        int inIndex = IsInOpenList(_targetPoint);

        if (inIndex != -1)
        {
            Item temp = _openList[inIndex];
            while (temp != null)
            {
                route.Add(temp.point);
                temp = temp.father;
            }

            route.Reverse();

            return true;
        }

        return false;
    }

    private void AddRoundPoint(Item item)
    {
        for (int i = 0; i < _dirs.Count; i++)
        {
            int2 dir = _dirs[i];
            int2 point = new int2(item.point.x + dir.x, item.point.y + dir.y);
            if (CheckPointValid(point))
            {
                Item sonItem = GetItem(point, item);
                CheckReplaceInOpenList(sonItem);
            }
        }
    }

    private void CheckReplaceInOpenList(Item item)
    {
        if (IsInCloseList(item.point) != -1)
        {
            return;
        }

        int inIndex = IsInOpenList(item.point);
        if (inIndex != -1)
        {
            Item inItem = _openList[inIndex];
            if (item.G < inItem.G)
            {
                _openList[inIndex] = item;
            }
        }
        else
        {
            _openList.Add(item);
        }
    }

    private Item GetItem(int2 point, Item father)
    {
        Item item = new Item();
        item.point = point;
        item.father = father;
        int weight = GetWeight(point);
        item.G = father != null ? (father.G + weight) : weight;
        int2 dir = _targetPoint - point;
        item.H = Mathf.RoundToInt(Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y));
        return item;
    }

    private int IsInCloseList(int2 inPoint)
    {
        for (int i = 0; i < _closeList.Count; i++)
        {
            int2 point = _closeList[i].point;
            if (point.x == inPoint.x && point.y == inPoint.y) { return i; }
        }
        return -1;
    }

    private int IsInOpenList(int2 inPoint)
    {
        for (int i = 0; i < _openList.Count; i++)
        {
            int2 point = _openList[i].point;
            if (point.x == inPoint.x && point.y == inPoint.y) { return i; }
        }
        return -1;
    }

    private Item GetMinGH()
    {
        int weight = Define.CELL_MAXWEIGHT;
        Item minItem = null;
        for (int i = 0; i < _openList.Count; i++)
        {
            Item item = _openList[i];
            int G = item.G;
            int H = item.H;
            if (G + H < weight)
            {
                weight = item.G + item.H;
                minItem = item;
            }
        }
        return minItem;
    }

    private bool CheckPointValid(int2 point)
    {
        if (point.x < 0 || point.y < 0) return false;
        if (point.x >= Define.CELL_COUNT_WIDTH || point.y >= Define.CELL_COUNT_HEIGHT) return false;
        return true;
    }

    private int GetWeight(int2 point)
    {
        if (_startPoint.x == point.x && _startPoint.y == point.y)
        {
            return 0;
        }
        return _weightList[point.x][point.y];
    }
}