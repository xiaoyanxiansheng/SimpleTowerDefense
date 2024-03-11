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
        // ��Դ��������ʼ��
        ResourceManager.Init();

        // �ؿ���������ʼ��
        LevelManager.Init(LevelConfig);

        // UI��������ʼ��
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
