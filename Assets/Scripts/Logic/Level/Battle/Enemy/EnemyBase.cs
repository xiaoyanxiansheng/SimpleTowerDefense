using Unity.Mathematics;
using UnityEngine;

public enum EntityType
{
    Enemy = 1,
    Tower = 2,
}

public abstract class EnemyBase : EntityBase
{
    private EntityConfigData _config;
    public EnemyBase() : base() { }
    public EnemyBase(int instanceId , int entityId):base(instanceId, entityId)
    {
        SetEntityId(entityId);
    }

    public override void SetEntityId(int entityId)
    {
        base.SetEntityId( entityId);
        _config = GameApp.Instance.EntityConfig.GetEntityConfigData(entityId);
    }

    public abstract void EnterBattle(Vector2 startPos, Vector2 endPos);

    public abstract void ExitBattle();
    #region 对外接口

    public float GetMoveSpeed()
    {
        return _config.MoveSpeed;
    }
    #endregion
}
