using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance;

    private List<EntityBase> _entitys = new List<EntityBase>();

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        // 创建自己
        EntityBase entity = CreateEntity<SingleAttckTower>(EntityType.User, 10001);
        entity.SetPositionCell(new int2(6, 4));
        entity = CreateEntity<SingleAttckTower>(EntityType.User, 10001);
        entity.SetPositionCell(new int2(6, 10));
        entity = CreateEntity<ContinueSingleAttackTower>(EntityType.User, 10002);
        entity.SetPositionCell(new int2(5, 3));
        entity = CreateEntity<ContinueSingleAttackTower>(EntityType.User, 10002);
        entity.SetPositionCell(new int2(5, 3));
        entity = CreateEntity<ContinueSingleAttackTower>(EntityType.User, 10002);
        entity.SetPositionCell(new int2(4, 5));
        entity = CreateEntity<ContinueSingleAttackTower>(EntityType.User, 10002);
        entity.SetPositionCell(new int2(6, 6));
        entity = CreateEntity<ContinueSingleAttackTower>(EntityType.User, 10002);
        entity.SetPositionCell(new int2(5, 7));
        entity = CreateEntity<ContinueSingleAttackTower>(EntityType.User, 10002);
        entity.SetPositionCell(new int2(4, 8));
        entity = CreateEntity<SingleAttckDirRangeTower>(EntityType.User, 10003);
        entity.SetPositionCell(new int2(5, 9));
        entity = CreateEntity<SingleAttckDirRangeTower>(EntityType.User, 10003);
        entity.SetPositionCell(new int2(2, 3));
        entity = CreateEntity<NearRangeAttckTower>(EntityType.User, 10004);
        entity.SetPositionCell(new int2(8, 10));
        entity = CreateEntity<LongRangeAttackTower>(EntityType.User, 10005);
        entity.SetPositionCell(new int2(6, 11));
        entity = CreateEntity<LongRangeAttackTower>(EntityType.User, 10005);
        entity.SetPositionCell(new int2(3, 7));
    }

    private int indexCount = 0;
    private void Update()
    {
        indexCount++;
        if (indexCount % 100 != 0)
            return;

        // 出怪
        CreateCommonEnemy();
    }

    public CommonEnemy CreateCommonEnemy()
    {
        CommonEnemy commonEntity = (CommonEnemy)CreateEntity<CommonEnemy>(EntityType.Enemy,20001);
        return commonEntity;
    }

    public EntityBase CreateEntity<T>(EntityType entityType, int entityId) where T : EntityBase
    {
        GameObject entityObj = AssetManager.Instance.CreateEntity<T>(entityId);
        T entity = entityObj.GetComponent<T>();
        entity.Init(entityType, entityId);
        _entitys.Add(entity);
        return entity;
    }

    public EntityBase GetEntity(int entityInstanceId) 
    {
        for(int i = 0 ; i < _entitys.Count; i++)
        {
            if (_entitys[i].GetEntityInstanceId() == entityInstanceId)
                return _entitys[i];
        }
        return null;
    }

    public void RemoveEntity(int entityInstanceId)
    {
        int inIndex = -1;
        for (int i = 0; i < _entitys.Count; i++)
        {
            if (_entitys[i].GetEntityInstanceId() == entityInstanceId)
            {
                inIndex = i;
                break;
            }
        }
        if(inIndex != -1)
        {
            AssetManager.Instance.DestoryEntity(_entitys[inIndex].gameObject);
            _entitys.RemoveAt(inIndex);
        }
    }

    //private void ClearSkillReference(EntityBase entity)
    //{
    //    foreach(EntityBase skillBase in _entitys)
    //    {
    //        if (skillBase.GetType().IsSubclassOf(typeof(SkillBase)))
    //        {
    //            ((SkillBase)skillBase).ClearHitReference(entity);
    //        }
    //    }
    //}

    public EntityBase GetEntityByDistance(EntityType entityType ,Vector2 pos, float distance)
    {
        EntityBase entityBase = null;
        float minDistance = 100000f;
        foreach(EntityBase entity in _entitys)
        {
            if(entity.GetEntityType() != EntityType.SKill && entityType == entity.GetEntityType())
            {
                float tempDistance = Vector2.Distance(entity.transform.localPosition, pos);
                if (Vector2.Distance(entity.transform.localPosition, pos) < distance)
                {
                    if(tempDistance < minDistance) 
                    {
                        minDistance = tempDistance;
                        entityBase = entity;
                    }
                }
            }
        }
        return entityBase;
    }

    public List<EntityBase> GetEntityByDistances(EntityType entityType, Vector2 pos, float distance)
    {
        List<EntityBase> entityBases = new List<EntityBase>();
        foreach (EntityBase entity in _entitys)
        {
            if (entity.GetEntityType() != EntityType.SKill && entityType == entity.GetEntityType())
            {
                if (Vector2.Distance(entity.transform.localPosition, pos) < distance)
                {
                    entityBases.Add(entity);
                }
            }
        }
        return entityBases;
    }
}
