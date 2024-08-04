using UnityEngine;

public class UIBigWorld : UIBaseView
{
    public UIBigWorld(string name) : base(name)
    {

    }

    public override void OnClose()
    {
        // Debug.Log("UIMain OnClose " + name);
    }

    public override void OnCreate()
    {

    }

    public override void OnDestory()
    {
        // Debug.Log("UIMain OnDestory " + name);
    }

    public override void OnRegisterMessage()
    {
        RegisterButtonClick("Attack", ClickAttack);
    }

    public override void OnShow(bool isBack = false)
    {
        // Debug.Log("UIMain OnShow " + name);
    }

    private void ClickAttack()
    {
        Player.Instance.GetPlayerAnimation().PlayAttack(0.5f);
    }
}
