
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UIBigWorldInterAction : UIBaseView
{
    private GameObject _interActionPoint;

    public UIBigWorldInterAction(string name) : base(name)
    {
        
    }

    public override void OnClose()
    {
        // Debug.Log("UIMain OnClose " + name);
    }

    public override void OnCreate()
    {
        // Debug.Log("UIMain OnCreate " + name);
        // MessageManager.Instance.SendMessage(MessageConst.Battle_BigWorld_Create, GetInstanceId());
    }

    public override void OnDestory()
    {
        // Debug.Log("UIMain OnDestory " + name);
    }

    public override void OnRegisterMessage()
    {
        RegisterButtonClick("PlayBtn", ClickPlay);
    }

    public override void OnShow(bool isBack = false)
    {
        // Debug.Log("UIMain OnShow " + name);
        _interActionPoint = (GameObject)(GetParams()[0]);
    }

    private void ClickPlay()
    {
        List<int2> canWalkPaths = new List<int2>();
        Transform pathsTrans = _interActionPoint.transform.Find("paths0");
        for (int i = 0; i < pathsTrans.childCount; i++)
        {
            Vector2 p = pathsTrans.GetChild(i).position;
            p = BigWorldManager.Instance.WorldToLocal(p);
            // TODO 精度问题
            if (p.x > 0) p.x += 1; else p.x -= 1;
            if (p.y > 0) p.y += 1; else p.y -= 1;
            canWalkPaths.Add(CommonUtil.VecConvertCell(p));
        }

        List<LevelEnemyData> levelEnemys = new List<LevelEnemyData>();
        Transform enemyNode = _interActionPoint.transform.Find("enemys");
        for (int i = 0; i < enemyNode.childCount; i++)
        {
            levelEnemys.Add(LevelEnemyData.GetLevelEnemyData(enemyNode.GetChild(i).GetComponent<LevelEnemyBehaviour>()));
        }

        BattleBase.InitData initData = new BattleBase.InitData();
        initData.chapterIndex = 0;
        initData.levelIndex = 0;
        initData.isFixPaths = false;
        initData.canWalkPoints = canWalkPaths;
        initData.canPlacePoints = new List<int2>();
        initData.enemys = levelEnemys;
        BigWorldManager.Instance.CreateBattle(BigWorldManager.Instance.LocalBattleLayer, initData);

        OnBack();
        UIManager.Instance.Open(UIViewName.UIBattleMain, UIViewName.UIBattleInterAction);
    }
}
