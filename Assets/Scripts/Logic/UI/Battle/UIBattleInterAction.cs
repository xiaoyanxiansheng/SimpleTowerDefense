using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBattleInterAction : UIBaseView
{
    // 血量条
    private Dictionary<int, GameObject> _bloodBarMap = new Dictionary<int, GameObject>();
    private List<GameObject> _bloodBarMapPool = new List<GameObject>();

    // 伤害
    private Dictionary<int, GameObject> _hitMap = new Dictionary<int, GameObject>();
    private List<GameObject> _hitMapPool = new List<GameObject>();

    // 文字

    private GameObject _defaultBloodBarObj;
    private GameObject _defaultHitObj;

    private int _timerId = -1;

    public UIBattleInterAction(string name) : base(name)
    {
    }

    public override void OnClose()
    {
        Timer.Instance.RemoveTimer(_timerId);
        _bloodBarMap.Clear();
        _bloodBarMapPool.Clear();
        _hitMap.Clear();
        _hitMapPool.Clear();
    }

    public override void OnCreate()
    {
        _defaultBloodBarObj = GetGameObject("BloodBar/bar");
        _defaultHitObj = GetGameObject("HurtBlood/hb");
    }

    public override void OnDestory()
    {

    }

    public override void OnRegisterMessage()
    {
        RegisterMessage(MessageConst.Battle_BattleExit, MessageBattleExit);
        RegisterMessage(MessageConst.Battle_EnemyEnter, MessageEnemyEnter);
        RegisterMessage(MessageConst.Battle_EnemyDie, MessageEnemyDie);
        RegisterMessage(MessageConst.Battle_EnemyExit, MessageEnemyExit);
        RegisterMessage(MessageConst.Battle_EntityHurt, MessageEntityHurt);
    }

    public override void OnShow(bool isBack = false)
    {
        _timerId = Timer.Instance.AddTimer(0.001f, OnUpdate);
    }

    private void AddEntityBloodBar(int entityId)
    {
        EntityBase entity = EntityManager.Instance.GetEntity(entityId);
        if (entity == null) return;

        GameObject bloodObj = null;
        //if(_bloodBarMapPool.Count > 0) { bloodObj = _bloodBarMapPool[0]; } TODO
        if (bloodObj == null) 
        {
            bloodObj = GameObject.Instantiate(_defaultBloodBarObj);
            bloodObj.transform.SetParent(_defaultBloodBarObj.transform.parent);
            CommonUtil.TrimGameObejct(bloodObj);
        }
        _bloodBarMap[entityId] = bloodObj;
        bloodObj.gameObject.SetActive(true);
    }

    private void AddEntityHurtBlood(int entityId , int hurtValue)
    {
        // TODO 目前没有缓存
        EntityBase entity = EntityManager.Instance.GetEntity(entityId);
        if (entity == null) return;

        GameObject bloodObj = null;
        //if (_hitMapPool.Count > 0) { bloodObj = _hitMapPool[0]; }
        if (bloodObj == null)
        {
            bloodObj = GameObject.Instantiate(_defaultHitObj);
            bloodObj.transform.SetParent(_defaultHitObj.transform.parent.transform);
            CommonUtil.TrimGameObejct(bloodObj);
        }
        //_hitMap[entityId] = bloodObj;
        string v = hurtValue > 0 ? "+" + hurtValue.ToString() : "-" + hurtValue.ToString();
        bloodObj.transform.Find("hb").GetComponent<TextMeshProUGUI>().text = v;
        bloodObj.transform.position = entity.GetPosWS();
        bloodObj.gameObject.SetActive(true);
    }

    public void RemoveEntityBloodBar(int entityId)
    {
        if (!_bloodBarMap.ContainsKey(entityId)) return;

        GameObject bloodObj = _bloodBarMap[entityId];
        _bloodBarMapPool.Add(bloodObj);
        _bloodBarMap.Remove(entityId);
        bloodObj.gameObject.SetActive(false);
    }

    private bool OnUpdate(float delta)
    {
        foreach(int entityId in _bloodBarMap.Keys)
        {
            EnemyBase entity = (EnemyBase)EntityManager.Instance.GetEntity(entityId);
            _bloodBarMap[entityId].transform.position = entity.GetPosWS();
            RectTransform blood = _bloodBarMap[entityId].transform.Find("blood").GetComponent<RectTransform>();
            Vector2 size = blood.sizeDelta;
            size.x = Mathf.Lerp(0,60, entity.GetHealth()/entity.GetDefaultHealth());
            blood.sizeDelta = size;
        }

        return false;
    }

    private void MessageBattleExit(MessageManager.Message m)
    {

    }

    private void MessageEnemyEnter(MessageManager.Message m)
    {
        int entityId = (int)m.ps[0];
        AddEntityBloodBar(entityId);
    }
    private void MessageEnemyDie(MessageManager.Message m)
    {
        int entityId = (int)m.ps[0];
        RemoveEntityBloodBar(entityId);
    }
    private void MessageEnemyExit(MessageManager.Message m)
    {
        int entityId = (int)m.ps[0];
        RemoveEntityBloodBar(entityId);
    }
    private void MessageEntityHurt(MessageManager.Message m)
    {
        AddEntityHurtBlood((int)m.ps[0], (int)m.ps[1]);
    }
}
