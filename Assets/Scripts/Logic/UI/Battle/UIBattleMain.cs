using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleMain : UIBaseView
{
    private Button _skillBtn;

    public UIBattleMain(string name) : base(name)
    {
    }

    public override void OnClose()
    {
        Debug.Log("OnClose");
    }

    public override void OnCreate()
    {
        _skillBtn = GetGameObject("SkillButton").GetComponent<Button>();
        _skillBtn.gameObject.SetActive(false);
    }

    public override void OnDestory()
    {
        Debug.Log("OnDestory");
    }

    public override void OnRegisterMessage()
    {
        RegisterButtonClick("StartButton", ClickBattleStart);
        RegisterButtonClick(_skillBtn, ClickSkill);
        RegisterButtonClick("BattleEventRoot/EventScreen", ClickPlayDown);
    }

    public override void OnShow(bool isBack = false)
    {
        Debug.Log("OnShow");
    }

    private void ClickPlayDown()
    {
        Vector2 pos = Input.mousePosition;
        Vector2 offset = GetGameObject("BattleRoot").GetComponent<RectTransform>().anchoredPosition;
        pos = pos - offset + Vector2.one * Define.CELL_SIZE * 0.5f;
        int2 cell = CommonUtil.VecConvertCell(pos);
        MessageManager.Instance.SendMessage(MessageConst.Battle_TowerPlayDownOrUp, cell.x, cell.y, true , 10001);
    }

    private void ClickSkill()
    {
        // Test Do SKill
        LevelManager.Instance.battle.skillManager.DoSkill(10001);
    }

    private void ClickBattleStart()
    {
        GetGameObject("StartButton").gameObject.SetActive(false);
        LevelManager.Instance.CreateBattle(GetGameObject("BattleRoot"), 1001);
        LevelManager.Instance.StartBattle();

        _skillBtn.gameObject.SetActive(true);
    }
}
