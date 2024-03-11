using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum EntityType
{
    Enemy = 1,
    Tower = 2,
    SKill = 3,
    Maxtrix = 4,
    Player = 5
}

public class EntityBase : MonoBehaviour
{

    private NewSkillEffect _newSkillEffect;

    private bool _isEnable = false; // 目的是为了晚一帧

    private static int EntityInstanceID = 0;

    private int _entityInstanceId;
    private EntityType _entityType;
    private int _entityId;

    private DataSystem _dataSystem;

    public virtual void Init(EntityType entityType , int entityId)
    {        
        _entityInstanceId = ++EntityInstanceID;
        _entityType = entityType;
        _entityId = entityId;
    }

    public void InitDataSystem(int Health, float MoveSpeed)
    {
        _dataSystem = new DataSystem();
        _dataSystem.Health = Health;
        _dataSystem.MoveSpeed = MoveSpeed;
        _newSkillEffect = new NewSkillEffect(_dataSystem);
    }

    public DataSystem DataSystem { get { return _dataSystem; } }

    public NewSkillEffect SkillEffect { get { return _newSkillEffect; } }

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

    private void Update()
    {
        if(_dataSystem.IsDead()) return;
        SkillEffect.Update();
        CheckDead();

        OnUpdate();

        if (_isEnable == true) { return; }
        _isEnable = true;
        OnRealEnable();
    }

    protected virtual void OnUpdate()
    {

    }

    private void CheckDead()
    {
        if (_dataSystem.IsDead())
            GameApp.Instance.entityManager.RemoveEntity(GetEntityInstanceId());
    }

    protected virtual void OnRealEnable()
    {

    }
}
