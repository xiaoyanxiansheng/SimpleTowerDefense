using UnityEngine;

public class Player
{
    public static Player Instance;

    private PlayerMovement _playerMovement;
    private PlayerAnimation _playerAnimation;
    
    private int _playerId;
    private int _objInstanceId;
    private bool _enterWorld;

    private BoxCollider2D _boxCollider;

    public static Player Create(int playerId)
    {
        if (Instance != null) return Instance;

        new Player(playerId);

        return Instance;
    }

    public Player(int playerId)
    {
        _playerId = playerId;
        Instance = this;
        _playerMovement = new PlayerMovement(this);
        _playerAnimation = new PlayerAnimation(this);
    }

    public virtual void EnterWorld(Vector2 p)
    {
        string assetPath = "Assets/BuildResource/Prefab/Player/Player1001/Player1001";  // TODO
        ResourceManager.CreateGameObjectAsync(LoadGameObjectType.GameObject, assetPath, (instanceId, requestId) =>
        {
            GameObject ins = ResourceManager.GetGameObjectById(instanceId);
            ins.SetActive(true);
            ins.transform.SetParent(BigWorldManager.Instance.WorldPlayerLayer.transform, true);
            CommonUtil.TrimGameObejct(ins);
            _objInstanceId = instanceId;

            _boxCollider = ins.transform.Find("Boxcollider").GetComponent<BoxCollider2D>();
            OnEnterWorld();
        });
    }

    public bool IsEnterWorld()
    {
        return _enterWorld;
    }

    protected virtual void OnEnterWorld()
    {
        _enterWorld = true;
        _playerAnimation.Start();
        _playerMovement.Start();
    }

    public GameObject GetPlayerObjInstance()
    {
        return ResourceManager.GetGameObjectById(_objInstanceId);
    }

    public void Update()
    {
        if (!_enterWorld) return;

        if (!_playerAnimation.IsAttack())
        {
            _playerMovement.FixedUpdate();
        }
    }

    public void SetPosition(Vector2 p)
    {
        BigWorldManager.Instance.SetLocalPosition(p);

    }

    public Vector2 GetPosition()
    {
        return BigWorldManager.Instance.GetLocalPosition();
    }

    public void SetRotation(Vector3 p)
    {
        GetPlayerObjInstance().transform.localEulerAngles = p;
    }

    public PlayerAnimation GetPlayerAnimation()
    {
        return _playerAnimation;
    }

    public BoxCollider2D GetBoxCollider2D()
    {
        return _boxCollider;
    }

    public Vector2 GetBoxCollider2DPosition()
    {
        return _boxCollider.transform.localPosition;
    }
}