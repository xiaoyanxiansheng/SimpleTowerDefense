using UnityEngine;

public abstract class EntityComponentBase 
{
    protected EntityBase entity;
    public EntityComponentBase(EntityBase entity)
    {
        this.entity = entity;
    }

    public EntityBase GetEntity()
    {
        return entity;
    }

    public abstract void Start();
    public abstract void Update(float delta);
}