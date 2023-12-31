using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirMoveComponent : MoveComponentBase
{
    private Action _moveOutAreaFinish;
    private Action _moveDestinationCall;
    private Action<EntityBase> _moveHitCall;

    public void Move(Vector2 dir, float speed, Action moveDestinationCall, Action moveOutAreaFinish = null , Action<EntityBase> moveHitCall = null)
    {
        _moveDestinationCall = moveDestinationCall;
        _moveOutAreaFinish = moveOutAreaFinish;
        _moveHitCall = moveHitCall;
        base.DirMove(dir, speed);
    }

    protected override bool OnMoveingCheck()
    {
        EntityBase entity = EntityManager.Instance.GetEntityByDistance(EntityType.Enemy, GetPosition(), 50f);// todo
        if (entity != null && _moveHitCall != null) _moveHitCall(entity);
        return entity == null;
    }

    protected override void OnMoveOutAreaFinish()
    {
        if (_moveOutAreaFinish != null)
            _moveOutAreaFinish();
    }
}
