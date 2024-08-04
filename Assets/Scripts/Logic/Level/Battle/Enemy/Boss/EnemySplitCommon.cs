/*
    分裂生物
        能力：分裂

    受到一定的攻击会分裂，并且免疫当前一定的攻击，分裂具有一定的限度
 */
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemySplitCommon : EnemyCommon
{

    public override void Attacked(int attackMonoId)
    {
        base.Attacked(attackMonoId);
        int entityId = 20002;
        EntityManager.Instance.CreateEnemy<EnemyCommon>(entityId, (ennemy) =>
        {
            int x = UnityEngine.Random.Range(0, Define.CELL_COUNT_WIDTH);
            int y = UnityEngine.Random.Range(0, Define.CELL_COUNT_HEIGHT);
            Vector2 ss = CommonUtil.CellConvertVec(new int2(x, y));
            EnemyCommon enemyCommon = (EnemyCommon)ennemy;
            var moveComponent = ((EnemyCommon)ennemy).GetMoveComponent();
            enemyCommon.PreEnterBattle(GetPos());
            List<Vector2> paths = new List<Vector2>();
            paths.Add(GetPos());
            paths.Add(ss);
            moveComponent.SetSpeed(2);
            moveComponent.SetCusMoveFinishCall((type) =>
            {
                enemyCommon.EnterBattle(enemyCommon.GetPos(), endPos);
            });
            moveComponent.Move(paths);
        });
    }
}
