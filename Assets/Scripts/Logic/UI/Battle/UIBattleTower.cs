using UnityEngine;
using UnityEngine.UI;

public class UIBattleTower : UIBaseView
{
    private int _selectTowerId = 0;
    private int _selectCusionId = 0;

    public UIBattleTower(string name) : base(name)
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
        GameObject towerObj = GetGameObject("GameObject/Tower/Content");
        for (int i = 0; i < towerObj.transform.childCount; i++)
        {
            Button btn = towerObj.transform.GetChild(i).GetComponent<Button>();
            RegisterButtonClick(btn, ClickTower, i);
        }
        GameObject cusionObj = GetGameObject("GameObject/Cusion/Content");
        for (int i = 0; i < cusionObj.transform.childCount; i++)
        {
            Button btn = cusionObj.transform.GetChild(i).GetComponent<Button>();
            RegisterButtonClick(btn, ClickCusion, i);
        }
    }

    public override void OnShow(bool isBack = false)
    {
        
    }

    void ClickTower(int index)
    {
        string name = GetGameObject("GameObject/Tower/Content").transform.GetChild(index).name;
        _selectTowerId = int.Parse(name);
        MessageManager.Instance.SendMessage(MessageConst.Battle_UI_SelectTower, _selectTowerId);
    }

    void ClickCusion(int index)
    {
        string name = GetGameObject("GameObject/Cusion/Content").transform.GetChild(index).name;
        _selectCusionId = int.Parse(name);
        MessageManager.Instance.SendMessage(MessageConst.Battle_UI_SelectCusion, _selectCusionId);
    }
}