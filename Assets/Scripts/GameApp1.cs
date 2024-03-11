using UnityEngine;

public class GameApp1 : MonoBehaviour
{
    [SerializeField]
    public LevelConfig LevelConfig;

    private static GameApp1 instance;
    public static GameApp1 Instance
    {
        get { return instance; }
    }

    public LevelManager LevelManager;
    public UIManager UIManager;
    public ResourceManager ResourceManager;
    public Timer Timer;


    private void Awake()
    {
        instance = this;

        Timer = new Timer();
        LevelManager = new LevelManager();
        UIManager = new UIManager();
        ResourceManager = new ResourceManager();
    }

    private void OnEnable()
    {
        // 资源加载器初始化
        ResourceManager.Init();

        // 关卡管理器初始化
        LevelManager.Init(LevelConfig);

        // UI管理器初始化
        UIManager.Open(UIViewName.UIMain);
    }

    private void FixedUpdate()
    {
        Timer.FixedUpdate();
    }

    private void Update()
    {
        ResourceManager.Update();
    }
}
