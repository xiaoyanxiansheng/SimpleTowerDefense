using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleMain : UIBaseView
{
    public UIBattleMain(string name) : base(name)
    {
    }

    public override void OnClose()
    {
        Debug.Log("OnClose");
    }

    public override void OnCreate()
    {
        
    }

    public override void OnDestory()
    {
        Debug.Log("OnDestory");
    }

    public override void OnRegisterMessage()
    {
        RegisterButtonClick("button_startGame", ClickTest);
    }

    public override void OnShow(bool isBack = false)
    {
        Debug.Log("OnShow");
    }

    private void ClickTest()
    {
        LevelManager.Instance.CreateBattle(1001);
        LevelManager.Instance.StartBattle();
    }
}
