//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.Mathematics;
//using UnityEngine;
//using UnityEngine.Rendering;

//public class CommonEnemy : EnemyBase
//{
//    private List<int2> _paths = new List<int2>();

//    public void Move()
//    {
//        GameApp.Instance.aStarPath.SetStartTargetPoint(CommonUtil.VecConvertCell(GetPosition()));
//        GameApp.Instance.aStarPath.UpdatePath(ref _paths);
//        if(_paths.Count == 0)
//        {
//            Debug.Log("·ÉÐÐ");
//            return;
//        }
//        List<Vector2> realPaths = CommonUtil.RealPathConvert(_paths);
//        List<Vector2> firstPaths = new List<Vector2>();
//        firstPaths.Add(GetPosition());
//        firstPaths.Add(realPaths[1]);
//        PathMove(firstPaths, DataSystem.GetMoveSpeed(), () =>
//        {
//            realPaths.RemoveAt(0);
//            PathMove(realPaths, DataSystem.GetMoveSpeed(), () =>
//            {
//                GameApp.Instance.entityManager.RemoveEntity(GetEntityInstanceId());
//            });
//        });
//    }
//}
