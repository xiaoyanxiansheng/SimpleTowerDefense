using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EntityPathMoveCommonComponent : EntityPathMoveComponent
{
    private List<int2> _movePointPaths = new List<int2>();

    public EntityPathMoveCommonComponent(EnemyBase entity , Action<int> MoveFinishCall , Action MoveCellFinishCall) : base(entity, MoveFinishCall, MoveCellFinishCall)
    {
    }

    public bool Move(Vector2 startPos, Vector2 targetPos)
    {
        bool isPaths = CalculateMovePosPaths(startPos, targetPos);
        if(isPaths) base.Move(movePosPaths);
        return isPaths;
    }

    public bool CalculateMovePosPaths(Vector2 startPos, Vector2 targetPos)
    {
        _movePointPaths.Clear();
        movePosPaths.Clear();

        LevelManager.Instance.battle.GetBattleCellManager().GetMovePointPaths(ref _movePointPaths, CommonUtil.VecConvertCell(startPos), CommonUtil.VecConvertCell(targetPos));
        if (_movePointPaths.Count == 0) return false;

        Vector2 p0 = CommonUtil.CellConvertVec(_movePointPaths[0]);
        Vector2 p1 = CommonUtil.CellConvertVec(_movePointPaths[1]);
        Vector2 pp = (startPos - p0) * (startPos - p1);
        if (pp.x + pp.y > 0.0f)
        {
            movePosPaths.Add(startPos);
        }
        movePosPaths.Add(startPos);
        int index = 0;
        if (pp.x + pp.y < 0.0f)
        {
            index = 1;
        }
        for (int i = index; i < _movePointPaths.Count; i++)
        {
            movePosPaths.Add(CommonUtil.CellConvertVec(_movePointPaths[i]));
        }
        return true;
    }
}