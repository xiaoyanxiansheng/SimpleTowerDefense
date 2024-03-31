/*
    管理游戏中所以Entity
    进攻者
    防御者
    特效
    ......
 */
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityManager
{
    public static EntityManager Instance;
    private BattleEnemyConfig _battleEnemyConfig;

    private List<EntityBase> _entitys = new List<EntityBase>();        // 怪物

    private List<EnemyBase> _enemys = new List<EnemyBase>();        // 怪物
    private List<TowerBase> _towers = new List<TowerBase>();        // 防御塔

    private float _prebattleContinueTime = 0;

    public EntityManager(BattleEnemyConfig battleEnemyConfig) 
    {
        Instance = this;
        _battleEnemyConfig = battleEnemyConfig;
    }

    public void StartBattle()
    {

    }

    public void Update(float delta , float battleContinueTime)
    {
        //for (int i = 0; i < _enemys.Count; i++)
        //{
        //    _enemys[i].Update(delta);
        //}
        //for (int i = 0; i < _towers.Count; i++)
        //{
        //    _towers[i].Update(delta);
        //}
        //for (int i = 0; i < _skills.Count; i++)
        //{
        //    _skills[i].Update(delta);
        //}

        for (int i = 0; i < _entitys.Count; i++)
        {
            _entitys[i].Update(delta);
        }

        UpdateEnemyEnterBattle(battleContinueTime);
    }

    private void UpdateEnemyEnterBattle(float battleContinueTime)
    {
        for (int i = 0; i < _battleEnemyConfig.battleEnemyConfigDatas.Count; i++)
        {
            BattleEntityConfigData battleEnemyConfigData = _battleEnemyConfig.battleEnemyConfigDatas[i];
            if (battleEnemyConfigData.enterTime > _prebattleContinueTime && battleEnemyConfigData.enterTime <= battleContinueTime)
            {
                MessageManager.Instance.SendMessage(MessageConst.Battle_EnemyEnter, null, i);
                CreateEnemy(battleEnemyConfigData.entityId , (ennemy) =>
                {
                    EnemyCommon e = (EnemyCommon)ennemy;
                    e.SetEntityId(battleEnemyConfigData.entityId);
                    e.EnterBattle(CommonUtil.CellConvertVec(battleEnemyConfigData.startPoint), CommonUtil.CellConvertVec(battleEnemyConfigData.endPoint));
                });
            }
        }
        _prebattleContinueTime = battleContinueTime;
    }

    public void CreateEntity<T>(string prefabPath, Action<EntityBase> createFinishCall) where T : EntityBase , new()
    {
        ResourceManager.CreateGameObjectAsync(LoadGameObjectType.GameObject, prefabPath, (instanceId, requestId) =>
        {
            EntityBase entity = new T();
            entity.SetInstanceId(instanceId);
            if (entity.GetGameObject().GetComponent<RectTransform>() == null) entity.GetGameObject().AddComponent<RectTransform>();
            _entitys.Add(entity);

            createFinishCall(entity);
        });
    }

    public void CreateTower(int towerId , Action<EntityBase> createFinishCall)
    {
        string prefabPath = GameApp.Instance.TowerConfig.GetTowerConfigData(towerId).prefabPath;
        CreateEntity<TowerBase>(prefabPath, (entity) =>
        {
            entity.SetParent(LevelManager.Instance.battle.BattleRoot);
            createFinishCall(entity);
        });
    }

    public void CreateEnemy(int entityId, Action<EntityBase> createFinishCall)
    {
        string prefabPath = GameApp.Instance.EntityConfig.GetEntityConfigData(entityId).prefabPath;
        CreateEntity<EnemyCommon>(prefabPath, (entity) =>
        {
            EntityBehaviour entityBehaviour = entity.GetGameObject().GetComponent<EntityBehaviour>();
            if (entityBehaviour == null) entityBehaviour = entity.GetGameObject().AddComponent<EntityBehaviour>();
            entityBehaviour.entity = entity;

            entity.SetParent(LevelManager.Instance.battle.BattleRoot);
            _enemys.Add((EnemyBase)entity);
            createFinishCall(entity);
        });
    }

    public void CreateSkillTrack(int skillId , Action<EntityBase> createFinishCall)
    {
        var config = GameApp.Instance.SkillConfig.GetSkillConfigData(skillId).trackConfig;
        CreateEntity<SkillTrackEntity>(config.ShapeAssetPath, (entity) =>
        {
            EntityBehaviour entityBehaviour = entity.GetGameObject().GetComponent<EntityBehaviour>();
            if (entityBehaviour == null) entityBehaviour = entity.GetGameObject().AddComponent<EntityBehaviour>();
            entityBehaviour.entity = entity;

            entity.SetParent(LevelManager.Instance.battle.BattleRoot);
            createFinishCall(entity);
        });
    }

    public void UpdateAllEnemyCellPaths()
    {
        foreach (var entity in _enemys)
        {
            ((EnemyCommon)entity).UpdateCellPaths();
        }
    }

    public List<EnemyBase> GetEnemies()
    {
        return _enemys;
    }

    #region 距离相关

    /// <summary>
    /// 寻找最近的N个Entity TODO算法需要优化
    /// </summary>
    /// <param name="myPosition"></param>
    /// <param name="entitys"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public void SearchClosestNEntity(ref List<EnemyBase> searchEnemys ,Vector2 myPosition, List<EnemyBase> entitys , float distance, int n)
    {
        // 确保n不大于列表大小
        n = Mathf.Min(n, entitys.Count);

        // 使用SortedList模拟最小堆，键为距离，值为位置
        SortedList<float, EnemyBase> closestPositions = new SortedList<float, EnemyBase>(new DuplicateKeyComparer<float>());

        foreach (var e in entitys)
        {
            float distanceSqr = (e.GetPos() - myPosition).magnitude;

            if(distanceSqr <= distance)
            {
                // 添加当前点到"最小堆"，如果"最小堆"已满，且当前点距离小于最大距离点，则替换
                if (closestPositions.Count < n)
                {
                    closestPositions.Add(distanceSqr, e);
                }
                else if (distanceSqr < closestPositions.Keys[n - 1])
                {
                    closestPositions.RemoveAt(n - 1);
                    closestPositions.Add(distanceSqr, e);
                }
            }
        }

        searchEnemys.AddRange(closestPositions.Values);
    }

    // 允许SortedList有重复键
    private class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
    {
        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            // 如果项相同，则返回1，使其成为不同的键，避免SortedList的键冲突
            if (result == 0)
                return 1;   // Handle equality as being greater
            else
                return result;
        }
    }

    public EntityBase SearchOneEntity(Vector2 pos , float distance)
    {
        SortedList<float, EnemyBase> closestPositions = new SortedList<float, EnemyBase>(new DuplicateKeyComparer<float>());
        if(closestPositions.Count > 0)
            if (Vector2.Distance(pos, closestPositions[0].GetPos()) <= distance) return closestPositions[0];
        return null;
    }
    #endregion
}
