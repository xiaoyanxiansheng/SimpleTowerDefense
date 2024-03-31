//using JetBrains.Annotations;
//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public class PathMoveComponent : MoveComponentBase
//{
//    private int _pathIndex = 0;
//    private float _pathSpeed = 0;
//    private Action _moveFinishCall;
//    private List<Vector2> _movePath = new List<Vector2>();


//    public void Move(List<Vector2> movePath , float speed , Action moveFinishCall = null)
//    {
//        _movePath = movePath;
//        _moveFinishCall = moveFinishCall;
//        SetPosition(_movePath[0]);
//        DoMove(1, speed);
//    }
//    private void DoMove(int pathIndex, float speed)
//    {
//        _pathIndex = pathIndex;
//        _pathSpeed = speed;
//        if (_pathIndex >= _movePath.Count)
//        {
//            OnPathMoveFinish();
//            return;
//        }
//        else
//        {
//            base.PosMove(_movePath[_pathIndex], _pathSpeed);
//        }
//    }

//    protected override void OnMoveDestinationFinish()
//    {
//        DoMove(_pathIndex + 1 , _pathSpeed);
//    }

//    protected void OnPathMoveFinish()
//    {
//        if (_moveFinishCall != null)
//            _moveFinishCall();
//    }

//    public void SetSpeed(float speed)
//    {
//        _pathSpeed = speed;
//    }
//}
