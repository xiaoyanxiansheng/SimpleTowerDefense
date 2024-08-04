/*
    0 UI����
    1 ��ɫ����
    2 ��ͼ����
    3 ս������
 */

using UnityEngine;

public class BigWorldManager : MonoBehaviour
{
    public static BigWorldManager Instance;

    public BattleBase Battle;

    public GameObject Local;
    public GameObject World;
    public GameObject LocalBackGroundLayer;
    public GameObject LocalBattleLayer;
    //public GameObject LocalBattleBottomLayer;
    public GameObject WorldPlayerLayer;

    public VariableJoystick VariableJoystick;

    private void Start()
    {
        Instance = this;

        // MessageManager.Instance.RegisterMessage(MessageConst.Battle_BigWorld_Create, CreateBattleBigWorld);

        // UIManager.Instance.Open(UIViewName.UIBigWorld);
        // LocalBattleBottomLayer = LocalBattleLayer.transform.Find("Bottom").gameObject;

        // BigWorldInterActionObjectManager.Create();

        // ������ͼ������
        BigWorldLoadManager.Create();

        // ���ؽ�ɫ
        Player.Create(0).EnterWorld(Vector2.zero);
        
        // UI TODO
    }

    private void OnDisable()
    {
        Instance = null;
    }

    private void Update()
    {
        Player.Instance.Update();
        BigWorldLoadManager.Instance.Update();
    }

    public Vector3 GetLocalPosition()
    {
        return -Local.transform.localPosition;
    }
    public void SetLocalPosition(Vector3 position)
    {
        Local.transform.localPosition = -position;
    }

    public Vector3 WorldToLocal(Vector3 position)
    {
        return Local.transform.InverseTransformPoint(position);
    }

    public Vector3 LocalToWorld(Vector3 localPosition)
    {
        return Local.transform.TransformPoint(localPosition);
    }

    public void CreateBattle(GameObject battleRoot, BattleBase.InitData initData)
    {
        if (Battle == null) 
        {
            Battle = new CommonBattle(battleRoot, initData);
            Battle.StartBattle();
        }
        else
        {
            Battle.AddData(initData);
        }
        
    }

    public void ExitBattle()
    {
        if (Battle != null) Battle.ExitBattle();
        Battle = null;
    }
}