using UnityEngine;

public class UIMain : UIBaseView
{
    public UIMain(string name) : base(name)
    {
    }

    public override void OnClose()
    {
        // Debug.Log("UIMain OnClose " + name);
    }

    public override void OnCreate()
    {
        // Debug.Log("UIMain OnCreate " + name);
    }

    public override void OnDestory()
    {
        // Debug.Log("UIMain OnDestory " + name);
    }

    public override void OnRegisterMessage()
    {
        RegisterButtonClick("ClickTest", ClickTest);
    }

    public override void OnShow(bool isBack = false)
    {
        // Debug.Log("UIMain OnShow " + name);
    }

    private void ClickTest()
    {
         UIManager.Instance.Open(UIViewName.UIBattleMain);
    }
}
