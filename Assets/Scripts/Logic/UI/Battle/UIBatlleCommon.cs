using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class UIBattleCommon : UIBaseView
{
    private int _curSelectTowerId;
    private int _curSelectCusionId;

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
        RegisterMessage(MessageConst.Battle_UI_SelectTower, MessageSelectTower);
        RegisterMessage(MessageConst.Battle_UI_SelectCusion, MessageSelectCusion);

        RegisterButtonClick("StartButton", ClickBattleStart);
        RegisterButtonClick("BattleEventRoot/EventScreen", ClickPlayDown);
        RegisterButtonClick("BackButton", ClickBack);
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
        obj.transform.SetParent(GetGameObject("Back").transform);
        obj.gameObject.SetActive(true);
        CommonUtil.TrimGameObejct(obj);
        var trams = obj.GetComponent<RectTransform>();
        trams.sizeDelta = Vector2.zero;

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
            _canWalkPaths.Add(CommonUtil.VecConvertCell(pathsNode.GetChild(i).localPosition + Vector3.one * 0.5f));
        }
        // 2 可摆放区域
        Transform placeNode = obj.transform.Find("places");
        for (int i = 0; i < placeNode.childCount; i++)
        {
            _canPlacePoints.Add(CommonUtil.VecConvertCell(placeNode.GetChild(i).localPosition + Vector3.one * 0.5f));
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
        
        if (_curSelectTowerId != 0)
        {
            if (!BigWorldManager.Instance.Battle.battleCellManager.CanPlace(cell)) return;
            MessageManager.Instance.SendMessage(MessageConst.Battle_UI_TowerPlayDownOrUp, cell.x, cell.y, true, _curSelectTowerId);
        }
        else
            MessageManager.Instance.SendMessage(MessageConst.Battle_UI_CusionPlayDownOrUp, cell.x, cell.y, true, _curSelectCusionId);
    }

    private void ClickBack()
    {
        LevelManager.Instance.ExitBattle();
        OnBack();
    }

    private void MessageSelectTower(MessageManager.Message m)
    {
        _curSelectTowerId = (int)m.ps[0];
        _curSelectCusionId = 0;
    }
    private void MessageSelectCusion(MessageManager.Message m)
    {
        _curSelectCusionId = (int)m.ps[0];
        _curSelectTowerId = 0;
    }
}