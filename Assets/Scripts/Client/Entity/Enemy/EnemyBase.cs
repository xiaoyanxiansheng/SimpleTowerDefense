using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : EntityBase
{
    // Start is called before the first frame update
    
    protected PathMoveComponent _pathMoveComponent;
    protected DirMoveComponent _dirMoveComponent;

    protected void Awake()
    {
        _pathMoveComponent = GetComponent<PathMoveComponent>();
        _dirMoveComponent = GetComponent<DirMoveComponent>();
    }

    public void PathMove(List<Vector2> _path , float speed , Action moveFinishCall)
    {
        _pathMoveComponent.Move(_path, speed, moveFinishCall);
    }

    public void DirMove(Vector2 dir , float speed , Action moveFinishCall)
    {
        _dirMoveComponent.Move(dir, speed, moveFinishCall);
    }
}
