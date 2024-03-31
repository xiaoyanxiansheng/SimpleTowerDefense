//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.Mathematics;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class MainUI : MonoBehaviour
//{
//    public Transform StartOffset;
//    public ButtonEX GridButton;

//    private void Click(Vector3 pos)
//    {

//    }

//    public void OnEnable()
//    {
//        GridButton.onClick.AddListener(() => {
//            Vector2 pos = Input.mousePosition;

//            Vector2 offset = StartOffset.localPosition;
//            pos = pos - offset + Vector2.one * Define.CELL_SIZE * 0.5f;

//            if (true)
//            {
//                System.Random rIndex = new System.Random();
//                int index = rIndex.Next(10001, 10005); 
//                FixedTower tower = (FixedTower)GameApp.Instance.entityManager.CreateEntity<FixedTower>(EntityType.Tower, index);
//                tower.SetPositionCell(CommonUtil.VecConvertCell(pos));
//                tower.DoSkill();
//                //MatrixBase tower = (MatrixBase)GameApp.Instance.entityManager.CreateEntity<MatrixBase>(EntityType.Maxtrix, index);
//                //tower.SetPositionCell(CommonUtil.VecConvertCell(pos));
//                //tower.DoSkill();

//                GameApp.Instance.aStarPath.UpdateWeights();

//                List<BattleEntityBase> enemys = new List<BattleEntityBase>();
//                GameApp.Instance.entityManager.GetEnemyEntitys(ref enemys);
//                foreach (CommonEnemy enemy in enemys)
//                {
//                    enemy.Move();
//                }
//            }
//        });

//        //GridButton.OnMove((Vector2 pos) =>
//        //{
//        //    Vector2 offset = StartOffset.localPosition;
//        //    pos = pos - offset + Vector2.one * Define.CELL_SIZE * 0.5f;

//        //    if (true)
//        //    {
//        //        System.Random rIndex = new System.Random();
//        //        int index = 10001;// rIndex.Next(10003, 10006); 
//        //        FixedTower tower = (FixedTower)GameApp.Instance.entityManager.CreateEntity<FixedTower>(EntityType.Tower, index);
//        //        tower.SetPositionCell(CommonUtil.VecConvertCell(pos));
//        //        tower.DoSkill();
//        //        //MatrixBase tower = (MatrixBase)GameApp.Instance.entityManager.CreateEntity<MatrixBase>(EntityType.Maxtrix, index);
//        //        //tower.SetPositionCell(CommonUtil.VecConvertCell(pos));
//        //        //tower.DoSkill();

//        //        GameApp.Instance.aStarPath.UpdateWeights();

//        //        List<EntityBase> enemys = new List<EntityBase>();
//        //        GameApp.Instance.entityManager.GetEnemyEntitys(ref enemys);
//        //        foreach (CommonEnemy enemy in enemys)
//        //        {
//        //            enemy.Move();
//        //        }
//        //    }
//            //if (!CommonUtil.OutCombatArea(pos))
//            //{
//            //    pos -= Vector2.one * Define.CELL_SIZE;
//            //    GameApp.Instance.entityManager.Player.Move(pos, () =>
//            //    {
//            //        Debug.Log("move finish");
//            //    });
//            //}
//        //});
//        //GridButton.OnPointerDown
//        //GridButton.onClick.AddListener(OnClickBackGround);
//    }

//    //public void OnMove(AxisEventData eventData)
//    //{
//    //    //switch (eventData.moveDir)
//    //    //{
//    //    //    case MoveDirection.Down:
                
//    //    //}
//    //    //if(eventData.moveDir == MoveDirection.do)
//    //}

//    //public override OnClickBackGround(AxisEventData eventData)
//    //{
//    //    bool down = Input.GetMouseButtonDown(0);
//    //    if (down)
//    //    {
//    //        Vector2 pos = (Input.mousePosition - StartOffset.localPosition + Vector3.one * Define.CELL_SIZE * 0.5f) / Define.CELL_SIZE;
//    //        int2 cellPos = new int2(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));

//    //        if (false)
//    //        {


//    //            System.Random rIndex = new System.Random();
//    //            int index = 40001;// rIndex.Next(10003, 10006); 
//    //                              //FixedTower tower = (FixedTower)GameApp.Instance.entityManager.CreateEntity<FixedTower>(EntityType.User, index);
//    //                              //tower.SetPositionCell(cellPos);
//    //                              //tower.DoSkill();
//    //            MatrixBase tower = (MatrixBase)GameApp.Instance.entityManager.CreateEntity<MatrixBase>(EntityType.User, index);
//    //            tower.SetPositionCell(cellPos);
//    //            tower.DoSkill();

//    //            GameApp.Instance.aStarPath.UpdateWeight(cellPos, AStarPath.MaxWeight);
//    //        }

//    //        GameApp.Instance.entityManager.Player.Move(CommonUtil.IntConvertVec(cellPos) * Define.CELL_SIZE, () =>
//    //        {
//    //            Debug.Log("move finish");
//    //        });
//    //    }
//    //}
//}
