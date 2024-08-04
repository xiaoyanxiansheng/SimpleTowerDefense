/*
    所有显示的基类
    能力：
        1 Instance
        2 ComponentBase
 */

using System.Collections.Generic;
using UnityEngine;
using static MessageManager;

public abstract class EntityBase : BuffInterce
{
    private static int _monoEnityOnlyId = 0;

    private bool _isEnterBattle;
    private int _entityMonoId;          // Entity唯一标识
    protected int entityId;             // entity配置Id
    protected int entityInstanceId;     // InstanceId
    protected DataConfig dataConfig;

    protected FrameAnimation animation; // 动画系统

    private List<EntityComponentBase> _entityComponentBases = new List<EntityComponentBase>();

    private List<MessageManager.Message> registerMessages = new List<MessageManager.Message>();	// 注册的消息

    public EntityBase() 
    {
        _entityMonoId = _monoEnityOnlyId++;
    }

    public void InitEntity(int entityId)
    {
        this.entityId = entityId;
        dataConfig = GameApp.Instance.EntityConfig.GetEntityConfigData(GetEntityId()).DataConfig;
        OnInitEntity();
    }

    public void InitEntityInstance(int instanceId)
    {
        this.entityInstanceId = instanceId;
        animation = GetGameObject().GetComponent<FrameAnimation>();
        if (animation) PlayAnimation(FrameAnimation.FrameAnimationType.Idle);
        OnInitEntityInstance();
    }

    public void Destory()
    {
        ExitBattle();
        OnDestory();
    }

    protected virtual void OnDestory()
    {

    }

    /// <summary>
    /// 脚本初始化完成
    /// </summary>
    protected virtual void OnInitEntity()
    {

    }

    /// <summary>
    /// 实体加载完成
    /// </summary>
    protected virtual void OnInitEntityInstance()
    {

    }

    public void AddComponent(EntityComponentBase entityComponent)
    {
        _entityComponentBases.Add(entityComponent);

        entityComponent.Start();
    }

    public void RemoveComponent(EntityComponentBase entityComponent)
    {
        _entityComponentBases.Remove(entityComponent);
    }

    public virtual void Update(float delta)
    {
        OnUpdate(delta);

        for (int i = 0; i < _entityComponentBases.Count; i++)
        {
            _entityComponentBases[i].Update(delta);
        }
    }

    protected virtual void OnUpdate(float delta) { }

    public virtual void Attack(int beAttackMonoId) 
    {
        //GameObject obj = GetGameObject();
        //FrameAnimation frameAnimation = obj.GetComponent<FrameAnimation>();
        //if (frameAnimation)
        //{
        //    frameAnimation.Play(FrameAnimation.FrameAnimationType.Attack);
        //}
    }

    public virtual void Attacked(int attackMonoId) 
    {
        PlayAnimation(FrameAnimation.FrameAnimationType.BeAttack);
    }

    public void PlayAnimation(FrameAnimation.FrameAnimationType it)
    {
        if (animation)
        {
            animation.Play(it, () =>
            {
                animation.Play(FrameAnimation.FrameAnimationType.Idle, null, true);
            });
        }
    }

    public FrameAnimation.FrameAnimationType GetAnimationState()
    {
        return animation.curAnimationState;
    }

    public int GetEntityInstanceId()
    {
        return entityInstanceId;
    }

    public int GetEntityMonoId()
    {
        return _entityMonoId;
    }

    public int GetEntityId()
    {
        return entityId;
    }

    public GameObject GetGameObject()
    {
        return ResourceManager.GetGameObjectById(entityInstanceId);
    }

    public GameObject GetGameObject(string path)
    {
        return GetGameObject().transform.Find(path).gameObject;
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

    public Vector3 GetPosWS()
    {
        return GetGameObject().transform.position;
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
        CommonUtil.TrimGameObejct(GetGameObject());
    }

    public Vector3 TransformPoint(Vector3 vec)
    {
        Vector3 vws = BigWorldManager.Instance.Battle.Battle3DRoot.transform.TransformPoint(vec);
        vws.z = 50;
        return vws;
    }

    public virtual void EnterBattle()
    {
        _isEnterBattle = true;
        var collision = GetGameObject().transform.GetComponent<BoxCollider2D>();
        if (collision != null) collision.enabled = true;

        OnEnterBattle();
    }

    public virtual void ExitBattle()
    {
        _isEnterBattle = false;
        RemoveRegisterMesssge();
        OnExitBattle();
    }

    protected virtual void OnEnterBattle() { }
    protected virtual void OnExitBattle() { }

    public bool IsEnterBattle()
    {
        return _isEnterBattle;
    }

    protected void RegisterMessage(string msgName, MessageDelegate messageCall)
    {
        for (int i = 0; i < registerMessages.Count; i++)
        {
            MessageManager.Message m = registerMessages[i];
            if (m.name == msgName && m.messageCall == messageCall)
            {
                return;
            }
        }

        MessageManager.Message nm = MessageManager.Instance.BeginMessage(msgName);
        nm.messageCall = messageCall;
        MessageManager.Instance.RegisterMessage(nm);
        registerMessages.Add(nm);
    }

    private void RemoveRegisterMesssge()
    {
        for (int i = 0; i < registerMessages.Count; i++)
        {
            MessageManager.Instance.RemoveMessage(registerMessages[i]);
        }
        registerMessages.Clear();
    }
}
