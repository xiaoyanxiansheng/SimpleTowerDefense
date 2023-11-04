using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class CommonEnemy : EnemyBase
{
    List<int2> _paths = new List<int2>();

    private void OnEnable()
    {
        _paths.Clear();
        _paths.Add(new int2(0, 0));
        _paths.Add(new int2(0, 1));
        _paths.Add(new int2(0, 2));
        _paths.Add(new int2(1, 2));
        _paths.Add(new int2(2, 2));
        _paths.Add(new int2(2, 3));
        _paths.Add(new int2(2, 4));
        _paths.Add(new int2(3, 4));
        _paths.Add(new int2(4, 4));
        _paths.Add(new int2(4, 5));
        _paths.Add(new int2(5, 5));
        _paths.Add(new int2(5, 6));
        _paths.Add(new int2(6, 6));
        _paths.Add(new int2(7, 6));
        _paths.Add(new int2(8, 6));
        _paths.Add(new int2(8, 7));
        _paths.Add(new int2(8, 8));
        _paths.Add(new int2(8, 10));
        _paths.Add(new int2(9, 10));
        _paths.Add(new int2(9, 17));

        PathMove(CommonUtil.RealPathConvert(_paths), 1, () =>
        {
            EntityManager.Instance.RemoveEntity(GetEntityInstanceId());
        });
    }

    public void Update()
    {
        //Debug.Log(indexCount);
        //indexCount++;
        //if (indexCount % 100 == 0)
        //{
        //    GameObject bullet = AssetManager.Instance.CreateBullet(10001);
        //    bullet.GetComponent<SingleAttckSkill>().Play(0, 0, 5, transform.localPosition, _pathMoveComponent.GetDir());
        //}
    }
}
