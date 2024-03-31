
using UnityEngine;

public class SkillTrackEntity : EntityBase
{
    private EntityBase _owerEntity;

    public SkillTrackEntity() : base() { }
    public SkillTrackEntity(int instanceId, int entityId) : base(instanceId, entityId)
    {
    }

    public void SetOwerEntity(EntityBase owerEntity)
    {
        this._owerEntity = owerEntity;
    }

    public EntityBase GetOwerEntity()
    {
        return _owerEntity;
    }

    public void EnterBattle(Vector2 pos)
    {
        ShowSelf(pos);
    }

    public void ExitBattle()
    {

    }
}