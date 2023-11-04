using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirMoveComponent : MoveComponentBase
{
    private Action _moveDestinationCall;
    private Action _moveOutAreaFinish;

    public void Move(Vector2 dir, float speed, Action moveDestinationCall, Action moveOutAreaFinish = null)
    {
        _moveDestinationCall = moveDestinationCall;
        _moveOutAreaFinish = moveOutAreaFinish;
        base.DirMove(dir, speed);
    }

    protected override void OnMoveDestinationFinish()
    {
        if (_moveDestinationCall != null)
            _moveDestinationCall();
    }

    protected override void OnMoveOutAreaFinish()
    {
        if (_moveOutAreaFinish != null)
            _moveOutAreaFinish();
    }
}
