using UnityEngine;

public class GameApp : MonoBehaviour
{
    [SerializeField]
    public LevelConfig LevelConfig;
    [SerializeField]
    public EntityConfig EntityConfig;
    [SerializeField]
    public TowerConfig TowerConfig;
    [SerializeField]
    public SkillConfig SkillConfig;

    private static GameApp instance;
    public static GameApp Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;

        new ResourceManager();
        new Timer();
        new LevelManager();
        new UIManager();
    }

    private void OnEnable()
    {
        // 资源加载器初始化
        ResourceManager.Instance.Init();

        // 关卡管理器初始化
        LevelManager.Instance.Init();

        // UI管理器初始化
        UIManager.Instance.Open(UIViewName.UIMain);
    }

    private void FixedUpdate()
    {
        Timer.Instance.FixedUpdate();
    }

    private void Update()
    {
        ResourceManager.Instance.Update();
    }
}
