using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityPathMoveComponent : EntityMoveComponentBase
{
    private int _pathIndex;
    private Action _moveCellFinishCall;
    private Action<int> _moveFinishCall;
    private Action<int> _moveCusFinishCall;
    protected List<Vector2> movePosPaths = new List<Vector2>();

    public EntityPathMoveComponent(EnemyBase entity, Action<int> MoveFinishCall, Action MoveCellFinishCall) :base(entity)
    {
        _moveFinishCall = MoveFinishCall;
        _moveCellFinishCall = MoveCellFinishCall;
    }

    public void SetCusMoveFinishCall(Action<int> moveFinishCall)
    {
        _moveCusFinishCall = moveFinishCall;
    }

    public void Move(List<Vector2> path , int pathIndex = 0)
    {
        if (path.Count == 0) return;

        movePosPaths = path;
        Move(pathIndex);
    }

    private void Move(int pathIndex)
    {
        if(pathIndex >= movePosPaths.Count - 1) return;

        _pathIndex = pathIndex;
        SetStart(movePosPaths[_pathIndex]);
        SetEnd(movePosPaths[_pathIndex + 1]);
        base.Move();
    }

    protected override void OnMoveDestinationFinish()
    {
        _pathIndex++;
        _moveCellFinishCall();
        if (_pathIndex + 1 >= movePosPaths.Count)
        {
            _moveFinishCall(0);
            if (_moveCusFinishCall != null) _moveCusFinishCall(0);
            return;
        }
        Move(_pathIndex);
    }

    protected override void OnMoveOutAreaFinish()
    {
        _moveFinishCall(1);
        if (_moveCusFinishCall != null) _moveCusFinishCall(1);
    }

    public override void Start()
    {
        
    }

    public int GetPathIndex()
    {
        return _pathIndex;
    }

    public List<Vector2> GetMovePaths()
    {
        return movePosPaths;
    }
}