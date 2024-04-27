using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public enum EntityMoveFinishType
{
    OutArea = 0,
    MoveFinish
}

public class EntityMoveComponentBase : EntityComponentBase
{
    private bool _isMoving = false;
    private Vector2 _curPos;
    private Vector2 _startPos;
    private Vector2 _endPos;
    private float _speed;
    private Action _moveFinishCall;

    public EntityMoveComponentBase(EntityBase entity) :base(entity)
    {
    }

    public EntityMoveComponentBase SetMoveFinishCall(Action MoveFinishCall)
    {
        _moveFinishCall = MoveFinishCall;
        return this;
    }

    public EntityMoveComponentBase SetStart(Vector2 start)
    {
        _startPos = start;
        _curPos = _startPos;
        return this;
    }
    public EntityMoveComponentBase SetEnd(Vector2 end)
    {
        _endPos = end;
        return this;
    }
    public EntityMoveComponentBase SetSpeed(float speed)
    {
        _speed = speed;
        return this;
    }
    public float GetSpeed()
    {
        return _speed;
    }

    public EntityMoveComponentBase SetDir(Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;
        float angleRad = Mathf.Atan2(direction.y, direction.x);
        float angleDeg = (180 / Mathf.PI) * angleRad;
        SetRotation(angleDeg);
        return this;
    }

    public void Move()
    {
        _isMoving = true;
    }

    public override void Update(float delta)
    {
        if (!_isMoving) return;

        if (CommonUtil.OutCombatArea(_startPos))
        {
            _isMoving = false;
            MoveOutAreaFinish();
            return;
        }

        if (Vector2.Distance(_startPos, _curPos) - Vector2.Distance(_startPos, _endPos) >= 0)
        {
            _isMoving = false;
            MoveDestinationFinish();
            return;
        }

        _curPos += (_endPos - _startPos).normalized * _speed * delta;

        OnUpdatePosition();
    }
    private void MoveOutAreaFinish()
    {
        OnMoveOutAreaFinish();
    }
    private void MoveDestinationFinish()
    {
        if(_moveFinishCall != null)
        {
            _moveFinishCall();
        }
        OnMoveDestinationFinish();
    }
    /// <summary>
    /// 默认的移动实现方式
    /// </summary>
    protected virtual void OnUpdatePosition()
    {
        GameObject obj = ResourceManager.GetGameObjectById(entity.GetEntityInstanceId());
        if (obj) 
        {
            obj.GetComponent<RectTransform>().localPosition = _curPos;
        }
        
    }
    protected virtual void OnMoveOutAreaFinish() 
    {
        
    }
    protected virtual void OnMoveDestinationFinish() 
    {
    
    }

    public Vector2 GetCurPos()
    {
        // 未更新前的位置 位置下一
        return _curPos;
    }

    public int2 GetCurPoint()
    {
        return CommonUtil.VecConvertCell(GetCurPos());
    }

    public override void Start()
    {
        
    }

    public void SetRotation(float angleDeg)
    {
        GameObject obj = ResourceManager.GetGameObjectById(entity.GetEntityInstanceId());
        obj.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, angleDeg - 90));
    }
}