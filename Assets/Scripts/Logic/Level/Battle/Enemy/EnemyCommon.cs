/*
    最简单敌人
        能力：路径移动
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommon : EnemyBase
{
    protected EntityPathMoveComponent movePathComponent;        // 格子到格子的移动方式
    protected List<Vector2> movePosPaths = new List<Vector2>();
    //public EnemyCommon() : base() { }

    protected override void OnInitEntity()
    {
        base.OnInitEntity();
        movePathComponent = new EntityPathMoveComponent(this, MoveFinishCall, MoveCellFinishCall);
        AddComponent(movePathComponent);
    }

    /// <summary>
    /// 进入战斗
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    public override void EnterBattle(Vector2 startPos , Vector2 endPos)
    {
        base.EnterBattle(startPos, endPos);
        PreEnterBattle(startPos);
        movePathComponent.SetSpeed(GetMoveSpeed());
        movePathComponent.Move(CalMovePosPaths(ref movePosPaths, startPos, endPos));
    }

    public void PreEnterBattle(Vector2 startPos , bool isShow = true)
    {
        GameObject obj = ResourceManager.GetGameObjectById(GetEntityInstanceId());
        obj.transform.localPosition = startPos;
        obj.gameObject.SetActive(isShow);
    }

    /// <summary>
    /// 计算战斗路径
    /// </summary>
    /// <param name="movePosPaths"></param>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    protected virtual List<Vector2> CalMovePosPaths(ref List<Vector2> movePosPaths, Vector2 startPos, Vector2 endPos)
    {
        movePosPaths.Clear();
        BigWorldManager.Instance.Battle.GetBattleCellManager().GetMovePosPaths(ref movePosPaths, startPos, endPos);
        return movePosPaths;
    }

    /// <summary>
    /// 更新战斗路径
    /// </summary>
    public virtual void UpdateCellPaths()
    {
        movePathComponent.Move(CalMovePosPaths(ref movePosPaths, movePathComponent.GetCurPos(), endPos));
    }

    protected virtual void MoveFinishCall(int type)
    {
        MessageManager.Instance.SendMessage(MessageConst.Battle_EnemyExit, GetEntityMonoId(), type);
    }

    protected virtual void MoveCellFinishCall()
    {
    }

    protected virtual void MoveUpdateCall(Vector2 pos, Vector3 posWS)
    {

    }

    public EntityPathMoveComponent GetMoveComponent()
    {
        return movePathComponent;
    }
}