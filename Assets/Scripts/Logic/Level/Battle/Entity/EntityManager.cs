/*
    ������Ϸ������Entity
    ������
    ������
    ��Ч
    ......
 */
using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager
{
    public static EntityManager Instance;

    private List<LevelEnemyData> _levelEnemyDatas;

    //private List<EntityBase> _entitys = new List<EntityBase>();        // ����
    private Dictionary<int , EntityBase> _entityMaps = new Dictionary<int , EntityBase>();

    //private List<EnemyBase> _enemys = new List<EnemyBase>();        // ����
    private Dictionary<int , EnemyBase> _enemyMaps = new Dictionary<int , EnemyBase>();

    private List<TowerBase> _towers = new List<TowerBase>();        // ������

    private List<int> _waitDeleteMonoIds = new List<int>();

    private float _prebattleContinueTime = 0;

    public EntityManager(List<LevelEnemyData> enemyDatas) 
    {
        Instance = this;
        _levelEnemyDatas = enemyDatas;
    }

    public void StartBattle()
    {

    }

    public void Update(float delta , float battleContinueTime)
    {
        // ��һ֡ɾ��
        foreach(var entityMonoId in _waitDeleteMonoIds)
        {
            DestoryInstance(entityMonoId);
            _entityMaps.Remove(entityMonoId);
        }
        _waitDeleteMonoIds.Clear();

        foreach (var entity in _entityMaps.Values)
        {
            entity.Update(delta);
        }

        UpdateEnemyEnterBattle(battleContinueTime);
    }

    private void UpdateEnemyEnterBattle(float battleContinueTime)
    {
        for (int i = 0; i < _levelEnemyDatas.Count; i++)
        {
            LevelEnemyData enemyData = _levelEnemyDatas[i];
            if (enemyData.enterTime > _prebattleContinueTime && enemyData.enterTime <= battleContinueTime)
            {
                CreateEnemy<EnemyCommon>(enemyData.entityId , (ennemy) =>
                {
                    EnemyBase e = (EnemyBase)ennemy;
                    e.EnterBattle(enemyData.start, enemyData.end);
                });
            }
        }
        _prebattleContinueTime = battleContinueTime;
    }

    public void DestoryEntity(int entityMonoId)
    {
        _waitDeleteMonoIds.Add(entityMonoId);
    }

    public void DestoryEnemy(int entityMonoId)
    {
        EntityBase enemy = GetEntity(entityMonoId);
        if(enemy != null)
        {
            DestoryEntity(entityMonoId);
        }
    }

    public void DestoryInstance(int entityMonoId)
    {
        EntityBase entity = GetEntity(entityMonoId);
        if (entity != null)
        {
            entity.Destory();
            ResourceManager.DestoryGameObject(entity.GetEntityInstanceId());
        }
    }

    public void CreateEntity<T>(string prefabPath, Action<EntityBase> createFinishCall) where T : EntityBase , new()
    {
        ResourceManager.CreateGameObjectAsync(LoadGameObjectType.GameObject, prefabPath, (instanceId, requestId) =>
        {
            EntityBase entity = new T();
            entity.InitEntityInstance(instanceId);
            if (entity.GetGameObject().GetComponent<RectTransform>() == null) entity.GetGameObject().AddComponent<RectTransform>();
            _entityMaps[entity.GetEntityMonoId()] = entity;
            entity.GetGameObject().name += "_" + instanceId;
            createFinishCall(entity);
        });
    }

    public void CreateTower(int towerId , Action<EntityBase> createFinishCall)
    {
        string prefabPath = GameApp.Instance.EntityConfig.GetEntityConfigData(towerId).prefabPath;
        CreateEntity<TowerBase>(prefabPath, (entity) =>
        {
            entity.InitEntity(towerId);
            entity.SetParent(LevelManager.Instance.battle.BattleRoot);
            createFinishCall(entity);
        });
    }

    public void CreateEnemy<T>(int entityId, Action<EntityBase> createFinishCall) where T : EntityBase, new()
    {
        string prefabPath = GameApp.Instance.EntityConfig.GetEntityConfigData(entityId).prefabPath;
        CreateEntity<T>(prefabPath, (entity) =>
        {
            EntityBehaviour entityBehaviour = entity.GetGameObject().GetComponent<EntityBehaviour>();
            if (entityBehaviour == null) entityBehaviour = entity.GetGameObject().AddComponent<EntityBehaviour>();
            entityBehaviour.entityMonoId = entity.GetEntityMonoId();

            entity.InitEntity(entityId);
            entity.SetParent(LevelManager.Instance.battle.BattleRoot);
            _enemyMaps[entity.GetEntityMonoId()] = (EnemyBase)entity;
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
            entityBehaviour.entityMonoId = entity.GetEntityMonoId();

            entity.InitEntity(skillId);
            entity.SetParent(LevelManager.Instance.battle.BattleRoot);
            createFinishCall(entity);
        });
    }

    public void UpdateAllEnemyCellPaths()
    {
        foreach (var entity in _enemyMaps.Values)
        {
            ((EnemyCommon)entity).UpdateCellPaths();
        }
    }

    public EntityBase GetEntity(int entityMonoId)
    {
        EntityBase entity = null;
        _entityMaps.TryGetValue(entityMonoId, out entity);
        return entity;
    }

    public Dictionary<int ,EnemyBase> GetEnemies()
    {
        return _enemyMaps;
    }

    #region �������

    /// <summary>
    /// Ѱ�������N��Entity TODO�㷨��Ҫ�Ż�
    /// </summary>
    /// <param name="myPosition"></param>
    /// <param name="entitys"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public void SearchClosestNEntity(ref List<int> searchEnemys ,Vector2 myPosition, Dictionary<int ,EnemyBase> entitys , float distance, int n)
    {
        // ȷ��n�������б��С
        n = Mathf.Min(n, entitys.Count);

        // ʹ��SortedListģ����С�ѣ���Ϊ���룬ֵΪλ��
        SortedList<float, EnemyBase> closestPositions = new SortedList<float, EnemyBase>(new DuplicateKeyComparer<float>());

        foreach (var e in entitys.Values)
        {
            if (e.IsEnterBattle())
            {
                float distanceSqr = (e.GetPos() - myPosition).magnitude;

                if (distanceSqr <= distance)
                {
                    // ��ӵ�ǰ�㵽"��С��"�����"��С��"�������ҵ�ǰ�����С��������㣬���滻
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
        }

        foreach (var e in closestPositions.Values) 
        {
            searchEnemys.Add(e.GetEntityMonoId());
        }
    }

    // ����SortedList���ظ���
    private class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
    {
        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            // �������ͬ���򷵻�1��ʹ���Ϊ��ͬ�ļ�������SortedList�ļ���ͻ
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
