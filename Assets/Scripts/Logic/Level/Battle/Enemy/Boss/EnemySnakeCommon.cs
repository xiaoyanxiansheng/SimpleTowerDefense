/*
    Boss：蛇
    能力：
        1 管理身体Instance
 */

using System.Collections.Generic;
using UnityEngine;

public class EnemySnakeCommon : EnemyCommon
{
    class EnemySnakeCommonBody : EnemyCommon
    {
        private EnemySnakeCommon _head;
        public void SetHead(EnemySnakeCommon head)
        {
            _head = head;
        }

        public override void UpdateCellPaths()
        {
            movePathComponent.Move(CalMovePosPaths(ref movePosPaths, GetPos(), endPos), movePathComponent.GetPathIndex());
        }

        protected override List<Vector2> CalMovePosPaths(ref List<Vector2> movePosPaths, Vector2 startPos, Vector2 endPos)
        {
            movePosPaths.Clear();
            movePosPaths.AddRange(_head.movePosPaths);
            movePosPaths.Insert(movePathComponent.GetPathIndex(), GetPos());
            return movePosPaths;
        }
    }

    private List<Vector3> _linePaths = new List<Vector3>();
    private LineRenderer _lineRender;
    private int length = 5;   // 蛇身长度(Cell单位)
    private List<EnemySnakeCommonBody> snakeCommonBodies = new List<EnemySnakeCommonBody>();
    private int _enterIndex = 0;

    protected override void OnInitEntityInstance()
    {
        base.OnInitEntityInstance();

        _lineRender = GetGameObject("root").GetComponent<LineRenderer>();
        Vector3 defaultPosition = BigWorldManager.Instance.Battle.GetBattleRoot3DPosition();

        // 初始化身体
        snakeCommonBodies.Clear();
        for (int i = 0; i< length; i++)
        {
            // TODO 这里可能存在异步加载 所以需要同步处理
            EntityManager.Instance.CreateEnemy<EnemySnakeCommonBody>(20002, (ennemy) =>
            {
                EnemySnakeCommonBody e = (EnemySnakeCommonBody)ennemy;
                e.SetHead(this);
                snakeCommonBodies.Add(e);
            });
        }
    }

    public override void EnterBattle(Vector2 startPos, Vector2 endPos)
    {
        _enterIndex = 0;
        base.EnterBattle(startPos, endPos);
    }

    public override void UpdateCellPaths()
    {
        movePathComponent.Move(CalMovePosPaths(ref movePosPaths, GetPos(), endPos), movePathComponent.GetPathIndex());
    }

    protected override List<Vector2> CalMovePosPaths(ref List<Vector2> movePosPaths, Vector2 startPos, Vector2 endPos)
    {
        // 之前的路径
        List<Vector3> oldMovePosPaths = new List<Vector3>();
        if(movePathComponent.GetMovePaths().Count > 0)
        {
            for (int i = 0; i < movePathComponent.GetPathIndex(); i++)
            {
                oldMovePosPaths.Add(movePathComponent.GetMovePaths()[i]);
            }
        }

        // 新路径
        movePosPaths = base.CalMovePosPaths(ref movePosPaths, startPos, endPos);
        for(int i = oldMovePosPaths.Count-1 ; i>=0 ; i--)
        {
            movePosPaths.Insert(0, oldMovePosPaths[i]);
        }

        return movePosPaths;
    }

    protected override void OnUpdate(float delta)
    {
        if (!IsInited()) return;

        base.OnUpdate(delta);

        if(_linePaths.Count > 1)
        {
            _lineRender.SetPosition(_linePaths.Count-1, GetPosWS());
        }
    }

    protected override void MoveCellFinishCall()
    {
        if(!IsInited()) return;

        if(_enterIndex < snakeCommonBodies.Count)
        {
            snakeCommonBodies[_enterIndex++].EnterBattle(startPos, endPos);
        }

        CalLinePaths(movePathComponent.GetPathIndex());

        _lineRender.positionCount = _linePaths.Count;
        _lineRender.SetPositions(_linePaths.ToArray());
    }

    private void CalLinePaths(int pathIndex)
    {
        _linePaths.Clear();
        //_linePaths.Add(); // TODO 差最后一个
        int start = Mathf.Max(0, pathIndex - length);
        for (int i = start; i <= pathIndex; i++)
        {
            _linePaths.Add(TransformPoint(movePosPaths[i]));
        }
        if(pathIndex < movePosPaths.Count - 1)
        {
            _linePaths.Add(TransformPoint(movePosPaths[pathIndex + 1]));
        }

        _linePaths.Add(GetPosWS());
    }

    private bool IsInited()
    {
        return snakeCommonBodies.Count == length;
    }
}