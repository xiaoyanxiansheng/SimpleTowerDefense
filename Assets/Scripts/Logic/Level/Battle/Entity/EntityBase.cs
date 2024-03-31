using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBase
{
    protected int instanceId;        // ÏÔÊ¾ID
    protected int entityId;          // Âß¼­ID

    private List<EntityComponentBase> _entityComponentBases = new List<EntityComponentBase>();

    public EntityBase() { }

    public void SetInstanceId(int instanceId)
    {
        this.instanceId = instanceId;
    }

    public virtual void SetEntityId(int entityId)
    {
        this.entityId = entityId;
    }

    public EntityBase(int instanceId, int entityId)
    {
        this.instanceId = instanceId;
        this.entityId = entityId;
    }

    public void AddComponent(EntityComponentBase entityComponent)
    {
        _entityComponentBases.Add(entityComponent);

        entityComponent.Start();
    }

    public virtual void Update(float delta)
    {
        OnUpdate(delta);

        for (int i = 0; i < _entityComponentBases.Count; i++)
        {
            _entityComponentBases[i].Update();
        }
    }

    protected virtual void OnUpdate(float delta) { }

    public int GetEntityId()
    {
        return entityId;
    }

    public int GetEntityInstanceId()
    {
        return instanceId;
    }

    public GameObject GetGameObject()
    {
        return ResourceManager.GetGameObjectById(instanceId);
    }

    public RectTransform GetRectTransform()
    {
        return GetGameObject().GetComponent<RectTransform>();
    }

    public void ShowSelf(Vector2 pos)
    {
        GameObject obj = GetGameObject();
        obj.SetActive(true);
        obj.GetComponent<RectTransform>().localPosition = pos;
    }

    public Vector2 GetPos()
    {
        return GetRectTransform().localPosition;
    }

    public Vector2 SetPos(Vector2 pos)
    {
        return GetRectTransform().localPosition = pos;
    }

    public void SetRotation(Vector3 rot)
    {
        GetRectTransform().localEulerAngles = rot;
    }
    public void SetRotationZ(float rotz)
    {
        Vector3 rot = GetRectTransform().localEulerAngles;
        rot.z = rotz;
        GetRectTransform().localEulerAngles = rot;
    }
    public Vector3 GetRotation()
    {
        return GetRectTransform().localEulerAngles;
    }

    public void SetScale(float scale)
    {
        GetRectTransform().localScale = Vector3.one * scale;
    }
    public void SetScaleY(float scaleY)
    {
        Vector2 scale = GetRectTransform().localScale;
        scale.y = scaleY;
        GetRectTransform().localScale = scale;
    }

    public Vector2 GetScale()
    {
        return GetRectTransform().localScale;
    }

    public void SetAtive(bool active)
    {
        GetGameObject().SetActive(active);
    }

    public void SetParent(GameObject parent)
    {
        GetGameObject().transform.SetParent(parent.transform);
    }
}
