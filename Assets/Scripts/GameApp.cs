using UnityEngine;

public class GameApp : MonoBehaviour
{
    [SerializeField]
    public EntityConfig EntityConfig;
    [SerializeField]
    public SkillConfig SkillConfig;
    [SerializeField]
    public BuffConfig BuffConfig;

    private static GameApp instance;
    public static GameApp Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;

        ResourceManager.Create();
        Timer.Create();
        //new LevelManager();
        UIManager.Create();
    }

    private void OnEnable()
    {
        // 资源加载器初始化
        // ResourceManager.Create();

        // 关卡管理器初始化
        // LevelManager.Instance.Init();

        // UI管理器初始化
        // UIManager.Instance.Open(UIViewName.UIBigWorld);
    }

    private void FixedUpdate()
    {
        //Timer.Instance.FixedUpdate();
    }

    private void Update()
    {
        ResourceManager.Instance.Update();
        Timer.Instance.FixedUpdate();
    }
}
