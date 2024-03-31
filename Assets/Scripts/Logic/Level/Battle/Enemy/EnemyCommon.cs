/*
    ×î¼òµ¥µÐÈË
 */

using System;
using UnityEngine;

public class EnemyCommon : EnemyBase
{
    private Vector2 _targetPos;
    private EntityPathMoveCommonComponent _movePathComponent;
    public EnemyCommon() : base() { }
    public EnemyCommon(int instanceId,int entityId) : base(instanceId, entityId)
    {
        SetInstanceId(instanceId);
        SetEntityId(entityId);
    }

    public override void SetEntityId(int entityId)
    {
        base.SetEntityId(entityId);
        _movePathComponent = new EntityPathMoveCommonComponent(this, MoveFinishCall, MoveCellFinishCall);
        AddComponent(_movePathComponent);
    }

    public override void EnterBattle(Vector2 startPos , Vector2 endPos)
    {
        _targetPos = endPos;
        GameObject obj = ResourceManager.GetGameObjectById(GetEntityInstanceId());
        obj.transform.localPosition = startPos;
        obj.gameObject.SetActive(true);
        _movePathComponent.Move(startPos, endPos);
        _movePathComponent.SetSpeed(GetMoveSpeed());
    }

    public void UpdateCellPaths()
    {
        _movePathComponent.Move(_movePathComponent.GetCurPos(), _targetPos);
    }

    private void MoveFinishCall(int type)
    {
        // Debug.Log("MoveFinishCall");
    }

    private void MoveCellFinishCall()
    {
        // Debug.Log("MoveCellFinishCall");
    }

    public override void ExitBattle()
    {
        
    }
}