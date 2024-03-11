using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;

public class EntityManager
{
    private AttackPlayer m_Player;
    private List<EntityBase> _entitys = new List<EntityBase>();

    public void Init()
    {
        
    }

    public void OnEnable()
    {
         //GameApp.Instance.aStarPath.SetStartTargetPoint(new int2(0, 0), new int2(9, 17));
        // GameApp.Instance.aStarPath.UpdatePath();
    }

    private int indexCount = 0;
    public  void OnUpdate()
    {
        indexCount++;
        if (indexCount % 1000 != 0)
            return;

        // ณ๖นึ
        var entity = CreateCommonEnemy();
        entity.Move();

        // 
        if (Player == null)
        {
            CreatePlayer();
        }
    }

    public void CreatePlayer()
    {
        m_Player = (AttackPlayer)CreateEntity<AttackPlayer>(EntityType.Player, 50001);
        m_Player.SetPositionCell(new int2(1, 1));
        m_Player.DoSkillFollow();
    }

    public CommonEnemy CreateCommonEnemy()
    {
        CommonEnemy commonEntity = (CommonEnemy)CreateEntity<CommonEnemy>(EntityType.Enemy,20001);

        return commonEntity;
    }

    public EntityBase CreateEntity<T>(EntityType entityType, int entityId) where T : EntityBase
    {
        GameObject entityObj = GameApp.Instance.assetManager.CreateEntity<T>(entityId);
        T entity = entityObj.GetComponent<T>();
        if(entity == null)
        {
            entity = entityObj.AddComponent<T>();
        }
        entity.Init(entityType, entityId);
        var config = GameApp.Instance.EntityConfig.GetEntityConfigData(entityId);
        entity.InitDataSystem(config.Health, config.MoveSpeed);
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
            GameApp.Instance.assetManager.DestoryEntity(_entitys[inIndex].gameObject);
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

    public void GetEntityByDistances(EntityType entityType, ref List<EntityBase> entityBases, Vector2 pos, float distance, float maxNum = -1)
    {
        foreach (EntityBase entity in _entitys)
        {
            if (entity.GetEntityType() != EntityType.SKill && entityType == entity.GetEntityType())
            {
                if (Vector2.Distance(entity.transform.localPosition, pos) < distance)
                {
                    entityBases.Add(entity);
                    if (maxNum != -1 && entityBases.Count >= maxNum)
                    {
                        break;
                    }
                }
            }
        }
    }

    public void GetTowerEntitys(ref List<EntityBase> towers)
    {
        towers.Clear();
        foreach (EntityBase entity in _entitys)
        {
            if (entity.GetEntityType() == EntityType.Tower)
            {
                towers.Add(entity);
            }
        }
    }

    public void GetEnemyEntitys(ref List<EntityBase> enemys)
    {
        enemys.Clear();
        foreach (EntityBase entity in _entitys)
        {
            if (entity.GetEntityType() == EntityType.Enemy)
            {
                enemys.Add(entity);
            }
        }
    }

    public AttackPlayer Player
    {
        get
        {
            return m_Player;
        }
    }
}
