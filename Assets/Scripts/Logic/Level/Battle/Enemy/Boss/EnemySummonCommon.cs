/*
    召唤生物
        能力：召唤

    在一定的范围内随机召唤
 */

using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemySummonCommon : EnemyCommon
{
    private float _passTime = 0;
    private float _intervalTime = 2;

    private int _summonMaxNum = 5;
    private List<int2> _summonPoints = new List<int2>();


    /// <summary>
    /// 召唤生物
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="pos"></param>
    protected void SummonEnemy()
    {
        int entityId = 20002;   // TODO 
        for(int i = 0; i < _summonPoints.Count; i++)
        {
            EntityManager.Instance.CreateEnemy<EnemyCommon>(entityId, (ennemy) =>
            {
                int x = UnityEngine.Random.Range(0, Define.CELL_COUNT_WIDTH);
                int y = UnityEngine.Random.Range(0, Define.CELL_COUNT_HEIGHT);
                int2 ss = new int2(x, y);
                ((EnemyCommon)ennemy).EnterBattle(CommonUtil.CellConvertVec(ss), endPos);
            });
        }
    }

    protected virtual void CalSummonPoints()
    {
        _summonPoints.Clear();

        for(int i = 0; i < _summonMaxNum; i++)
        {
            int x = UnityEngine.Random.Range(0,Define.CELL_COUNT_WIDTH);
            int y = UnityEngine.Random.Range(0, Define.CELL_COUNT_HEIGHT);
            _summonPoints.Add(new int2(x,y));
        }
    }

    protected override void OnUpdate(float delta)
    {
        base.OnUpdate(delta);

        _passTime += delta;
        if(_passTime < _intervalTime) { return; }
        _passTime = 0;
        CalSummonPoints();
        SummonEnemy();
    }
}
