using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum EntityType
{
    Enemy = 1,
    User = 2,
    SKill = 3
}

public class EntityBase : MonoBehaviour
{
    private static int EntityInstanceID = 0;

    private int _entityInstanceId;
    private EntityType _entityType;
    private int _entityId;

    public void Init(EntityType entityType , int entityId)
    {        
        _entityInstanceId = ++EntityInstanceID;
        _entityType = entityType;
        _entityId = entityId;
    }

    public int GetEntityId()
    {
        return _entityId;
    }

    public int GetEntityInstanceId()
    {
        return _entityInstanceId;
    }

    public virtual Vector2 GetPosition()
    {
        return transform.localPosition;
    }

    public virtual void SetPosition(Vector2 position)
    {
        transform.localPosition = position;
    }

    public virtual void SetPositionCell(int2 position)
    {
        transform.localPosition = new Vector3(position.x * Define.CELL_SIZE , position.y * Define.CELL_SIZE , 0);
    }

    public EntityType GetEntityType()
    {
        return _entityType;
    }
}
