using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMoveComponent : MoveComponentBase
{
    private bool _isDir;
    private Vector2 _dir;
    private Vector2 _dirUp = new Vector2(0, 1);
    private Vector2 _startPos;
    private Vector2 _endPos;
    private Action _moveDestinationCall;
    private Action _moveOutAreaFinish;

    private float _length = 0;

    public void Move(Vector2 startPos , Vector2 endPos, Action moveDestinationCall, Action moveOutAreaFinish = null)
    {
        _isDir = false;
        _startPos = startPos;
        _endPos = endPos;
        _dir = (_endPos - _startPos).normalized;
        _moveDestinationCall = moveDestinationCall;
        _moveOutAreaFinish = moveOutAreaFinish;
        _length = Vector2.Distance(_endPos, _startPos);
        base.PosMove(_startPos, _startPos, 0);
    }

    public void MoveDir(Vector2 startPos, Vector2 dir, Action moveDestinationCall, Action moveOutAreaFinish = null)
    {
        _isDir = true;
        _startPos = startPos;
        _dir = dir;
        _moveDestinationCall = moveDestinationCall;
        _moveOutAreaFinish = moveOutAreaFinish;
        _length = 2000; // TODO
        base.PosMove(_startPos, _startPos, 0);
    }

    protected override void OnMoveDestinationFinish()
    {
        float angle = CommonUtil.GetAngle(_dirUp, _dir);

        Vector3 rotation = transform.localRotation.eulerAngles;
        rotation.z = angle;
        transform.localRotation = Quaternion.Euler(rotation);
        Vector3 localScale = transform.localScale;
        localScale.y = _length / Define.CELL_SIZE;
        transform.localScale = localScale;
        SetPosition(_startPos + _dir * _length * 0.5f);

        Debug.Log(angle);
        // Debug.Log("Ò»Ö±´¥·¢");
        if (_moveDestinationCall != null)
            _moveDestinationCall();
    }

    protected override void OnMoveOutAreaFinish()
    {
        if (_moveOutAreaFinish != null)
            _moveOutAreaFinish();
    }
}
