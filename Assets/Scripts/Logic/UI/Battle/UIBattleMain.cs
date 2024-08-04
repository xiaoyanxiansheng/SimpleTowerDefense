
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class UIBattleMain : UIBaseView
{

    public UIBattleMain(string name) : base(name)
    {
    }

    public override void OnClose()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnCreate()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnDestory()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnShow(bool isBack = false)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnRegisterMessage()
    {
        RegisterButtonClick("ExitBtn", ClickExitBattle);
        RegisterButtonClick("PlayTownBtn", ClickPlayTownBtn);
        //RegisterButtonClick("EventScreen", ClickPlayDown);
    }

    private void ClickExitBattle()
    {
        BigWorldManager.Instance.ExitBattle();
        OnBack();
    }

    private void ClickPlayTownBtn()
    {
        Vector3 position = -Player.Instance.GetPosition();
        int2 cell = CommonUtil.VecConvertCell(position);
        MessageManager.Instance.SendMessage(MessageConst.Battle_UI_TowerPlayDownOrUp, cell.x, cell.y, true, 10001);
    }

    private void ClickPlayDown()
    {
        Vector2 pos = Input.mousePosition;
        Vector2 offset = GetGameObject().GetComponent<RectTransform>().anchoredPosition;
        offset += UIManager.Instance.canvas.renderingDisplaySize * 0.5f;
        pos = pos - offset + Vector2.one * Define.CELL_SIZE * 0.5f;

        Vector3 worldPos = BigWorldManager.Instance.WorldToLocal(GetGameObject().transform.TransformPoint(pos));
        int2 cell = CommonUtil.VecConvertCell(worldPos);

        MessageManager.Instance.SendMessage(MessageConst.Battle_UI_TowerPlayDownOrUp, cell.x, cell.y, true, 10001);
    }
}
