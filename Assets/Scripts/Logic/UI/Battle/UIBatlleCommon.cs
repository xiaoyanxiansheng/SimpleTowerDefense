using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.UI;

public class UIBattleCommon : UIBaseView
{
    private int _chapterIndex;
    private int _levelIndex;

    private bool _isFixedPath = false;
    private List<int2> _canWalkPaths = new List<int2>();
    private List<int2> _canPlacePoints = new List<int2>();
    private List<LevelEnemyData> _levelEnemys = new List<LevelEnemyData>();

    public UIBattleCommon(string name) : base(name)
    {
    }

    public override void OnClose()
    {
        
    }

    public override void OnCreate()
    {
        // 加载关卡数据
        _chapterIndex = (int)GetParams()[0];
        _levelIndex = (int)GetParams()[1];
        string levelDataPath = string.Format("Prefab/UI/Level/Chapter{0:D3}/{1:D3}", _chapterIndex, _levelIndex);
        ResourceManager.CreateGameObjectAsync(LoadGameObjectType.GameObject, levelDataPath, (instanceId, requestId) => { InitBatleData(instanceId, requestId); });
    }

    public override void OnDestory()
    {
        
    }

    public override void OnRegisterMessage()
    {
        RegisterButtonClick("StartButton", ClickBattleStart);
        RegisterButtonClick("BattleEventRoot/EventScreen", ClickPlayDown);
    }

    public override void OnShow(bool isBack = false)
    {
        
    }

    private void InitBatleData(int instanceId, int requestId)
    {
        _canWalkPaths.Clear();
        _canPlacePoints.Clear();
        _levelEnemys.Clear();

        GameObject obj = ResourceManager.GetGameObjectById(instanceId);
        obj.transform.parent = GetGameObject("Back").transform;
        obj.gameObject.SetActive(true);
        CommonUtil.TrimGameObejct(obj);

        // 1 可行走区域
        Transform pathsNode = obj.transform.Find("paths0");
        if (pathsNode != null)
        {
            _isFixedPath = true;
        }
        else
        {
            _isFixedPath = false;
            pathsNode = obj.transform.Find("paths1");
        }
        for (int i = 0; i < pathsNode.childCount; i++)
        {
            _canWalkPaths.Add(CommonUtil.VecConvertCell(pathsNode.GetChild(i).localPosition));
        }
        // 2 可摆放区域
        Transform placeNode = obj.transform.Find("places");
        for (int i = 0; i < placeNode.childCount; i++)
        {
            _canPlacePoints.Add(CommonUtil.VecConvertCell(placeNode.GetChild(i).localPosition));
        }
        // 3 敌人数据
        Transform enemyNode = obj.transform.Find("enemys");
        for (int i = 0; i < enemyNode.childCount; i++)
        {
            _levelEnemys.Add(LevelEnemyData.GetLevelEnemyData(enemyNode.GetChild(i).GetComponent<LevelEnemyBehaviour>()));
        }
    }

    private void ClickBattleStart()
    {
        GetGameObject("StartButton").gameObject.SetActive(false);
        BattleBase.InitData initData = new BattleBase.InitData();
        initData.chapterIndex = _chapterIndex;
        initData.levelIndex = _levelIndex;
        initData.isFixPaths = _isFixedPath;
        initData.canWalkPoints = _canWalkPaths;
        initData.canPlacePoints = _canPlacePoints;
        initData.enemys = _levelEnemys;
        LevelManager.Instance.CreateBattle(GetGameObject("BattleRoot"), initData);
        LevelManager.Instance.StartBattle();
    }

    private void ClickPlayDown()
    {
        Vector2 pos = Input.mousePosition;
        Vector2 offset = GetGameObject("BattleRoot").GetComponent<RectTransform>().anchoredPosition;
        offset += UIManager.Instance.canvas.renderingDisplaySize * 0.5f;
        pos = pos - offset + Vector2.one * Define.CELL_SIZE * 0.5f;
        int2 cell = CommonUtil.VecConvertCell(pos);
        MessageManager.Instance.SendMessage(MessageConst.Battle_TowerPlayDownOrUp, cell.x, cell.y, true, 10001);
    }
}