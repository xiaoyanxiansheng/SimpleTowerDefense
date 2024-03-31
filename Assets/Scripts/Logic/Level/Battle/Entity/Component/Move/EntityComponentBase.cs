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
    public abstract void Update();

    public void SetRotation(float angleDeg)
    {
        GameObject obj = ResourceManager.GetGameObjectById(entity.GetEntityInstanceId());
        obj.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, angleDeg - 90));
    }
}