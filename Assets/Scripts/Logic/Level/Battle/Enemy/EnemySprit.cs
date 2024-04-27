
using System.Collections.Generic;
using UnityEngine;

public class EnemySprit : EnemyCommon
{
    protected override void OnUpdate(float delta)
    {
        base.OnUpdate(delta);
    }

    protected override void MoveCellFinishCall()
    {
        int curIndex = movePathComponent.GetPathIndex();
        if (curIndex == 0) return;
        List<Vector2> movePaths = movePathComponent.GetMovePaths();
        if (curIndex >= movePaths.Count-1) return;
        
        Vector2 curDir = (movePaths[curIndex] - movePaths[curIndex-1]).normalized;
        Vector2 nextDir = (movePaths[curIndex+1] - movePaths[curIndex]).normalized;
        if(CommonUtil.GetAngle(curDir, nextDir) < 1)
        {
            movePathComponent.SetSpeed(movePathComponent.GetSpeed() * 1.1f);    // TODOÅäÖÃ¿ØÖÆ
        }
        else
        {
            movePathComponent.SetSpeed(GetBuffMoveSpeed());
        }
        
    }
}