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
        // ��Դ��������ʼ��
        // ResourceManager.Create();

        // �ؿ���������ʼ��
        // LevelManager.Instance.Init();

        // UI��������ʼ��
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
