//using System.Collections;
//using System.Collections.Generic;
//using Unity.Mathematics;
//using UnityEngine;

//public class MoveComponentBase
//{
//    private bool _isMoving = false;
//    private bool _isDir = false;
//    private Vector2 _startPos = Vector2.zero;
//    private Vector2 _endPso = Vector2.zero;
//    private float _speed = 0;
//    private Vector2 _dir = Vector2.zero;

//    public void SetPosition(Vector2 pos)
//    {
//        OnUpdatePosition(pos);
//    }

//    public void SetPositionCell(int2 pos)
//    {
//        SetPosition(new Vector2(pos.x * Define.CELL_SIZE, pos.y * Define.CELL_SIZE));
//    }

//    public void PosMove(Vector2 startPos ,Vector2 endPso, float speed)
//    {
//        _isMoving = true;
//        _isDir = false;
//        _startPos = startPos;
//        _endPso = endPso;
//        _speed = speed;
//        _dir = (_endPso - _startPos).normalized;
//    }

//    public void PosMove(Vector2 endPso , float speed)
//    {
//        _isMoving = true;
//        _isDir = false;
//        _startPos = transform.localPosition;
//        _endPso = endPso;
//        _speed = speed;
//        _dir = (_endPso - _startPos).normalized;
//    }

//    public void DirMove(Vector2 dir , float speed)
//    {
//        _isMoving = true;
//        _startPos = transform.localPosition;
//        _isDir = true;
//        _dir = dir;
//        _speed = speed;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (!_isMoving) return;

//        if (CommonUtil.OutCombatArea(_startPos))
//        {
//            _isMoving = false;
//            OnMoveOutAreaFinish();
//            return;
//        }

//        if(!_isDir) 
//        {
//            if (Vector2.Distance(_startPos, _endPso) < 0.01f || Vector2.Dot(_endPso - _startPos, _dir) < 0)
//            {
//                _isMoving = false;
//                OnMoveDestinationFinish();
//                return;
//            }
//        }

//        // 最好采用这种方式 ， 之前的判断能够节约性能
//        if (!OnMoveingCheck())
//        {
//            _isMoving = false;
//            return;
//        }

//        _startPos += _dir * _speed;
//        OnUpdatePosition(_startPos);
//    }

//    protected virtual bool OnMoveingCheck()
//    {
//        return true;
//    }

//    protected virtual void OnUpdatePosition(Vector2 position)
//    {
//        transform.localPosition = position;
//    }

//    /// <summary>
//    /// 正常结束
//    /// </summary>
//    protected virtual void OnMoveDestinationFinish()
//    {

//    }

//    /// <summary>
//    /// 超出结束
//    /// </summary>
//    protected virtual void OnMoveOutAreaFinish()
//    {

//    }

//    public Vector2 GetDir()
//    {
//        return _dir;
//    }

//    public Vector2 GetPosition()
//    {
//        return transform.localPosition;
//    }
//}
