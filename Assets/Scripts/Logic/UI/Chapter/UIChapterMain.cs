using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChapterMain : UIBaseView
{
    private int _curIndex = 0;

    public UIChapterMain(string name) : base(name)
    {
    }

    public override void OnClose()
    {
        
    }

    public override void OnCreate()
    {
        
    }

    public override void OnDestory()
    {
        
    }

    public override void OnRegisterMessage()
    {
        RegisterLevelEvent();
    }

    public override void OnShow(bool isBack = false)
    {
        RefreshLevelItemsState();
    }

    void RefreshLevelItemsState()
    {
        GameObject level = GetGameObject("level");
        for(int i = 0;i < level.transform.childCount;i++)
        {
            for(int j = 0; j < 3; j++)
            {
                level.transform.GetChild(i).Find("State").transform.GetChild(j).gameObject.SetActive(j == 2);
            }

            for(int j = 0;j < 4; j++)
            {
                level.transform.GetChild(i).Find("Star").transform.GetChild(j).gameObject.SetActive(j == 0);
            }
        }
    }

    void RegisterLevelEvent()
    {
        for (int i = 0; i < GetGameObject("level").transform.childCount; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Button btn = GetGameObject("level").transform.GetChild(i).Find("State").transform.GetChild(j).transform.Find("Button").gameObject.GetComponent<Button>();
                RegisterButtonClick(btn, ClickLevel , i);
            }
        }
    }

    void ClickLevel(int index)
    {
        List<object> ps = new List<object>();
        ps.Add(1);
        ps.Add(index+1);
        UIManager.Instance.Open(ps, UIViewName.UIBattleCommon, UIViewName.UIBattleInterAction, UIViewName.UIBattleTower);
    }
}