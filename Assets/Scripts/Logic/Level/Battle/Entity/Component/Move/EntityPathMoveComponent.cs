using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityPathMoveComponent : EntityMoveComponentBase
{
    private int _pathIndex;
    private Action _moveCellFinishCall;
    private Action<int> _moveFinishCall;
    protected List<Vector2> movePosPaths = new List<Vector2>();

    public EntityPathMoveComponent(EnemyBase entity, Action<int> MoveFinishCall, Action MoveCellFinishCall) :base(entity)
    {
        _moveFinishCall = MoveFinishCall;
        _moveCellFinishCall = MoveCellFinishCall;
    }

    protected void Move(List<Vector2> path)
    {
        _pathIndex = 0;
        movePosPaths = path;
        MoveNext();
    }

    private void MoveNext()
    {
        SetStart(movePosPaths[_pathIndex]);
        SetEnd(movePosPaths[_pathIndex + 1]);
        base.Move();
        _pathIndex++;
    }

    protected override void OnMoveDestinationFinish()
    {
        _moveCellFinishCall();
        if (_pathIndex + 1 >= movePosPaths.Count)
        {
            _moveFinishCall(0);
            return;
        }
        MoveNext();
    }

    protected override void OnMoveOutAreaFinish()
    {
        _moveFinishCall(1);
    }

    public override void Start()
    {
        
    }
}