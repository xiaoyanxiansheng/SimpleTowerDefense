/*
UI����
    1 ��UI(�Զ�����)��ÿ��UI���ϱ�����һ������UI����ǰ��Ҫ�رյ�ǰUI���ϣ���ʱֻ��Ҫ������߼����У��رյ�ǰUI���Ϻ���Ҫ���ϴιرյ�UI���ϣ���ʱֻ��Ҫ����ر��߼����С�
    2 ��UI(�ֶ�����)��ÿ��UI���ϱ�����һ���������Զ��򿪺͹رյ�Ӱ�졣����һЩ������
    0 ��UI������UIһ�𹹳�UI����
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class UIBaseView
{
    public string name = "";
    private List<object> openParams = new List<object>();
    private int uiInitRequestId = 0;		// �첽����ID
    private Action uiInitFinishCall;	// ���������ɺ�Ļص�
    private int uiInstanceId = 0;			// UI��ʵ��ID
    private GameObject uiBindCore;			// ��Prefab�нڵ�ؼ�
    private List<MessageManager.Message> registerMessages = new List<MessageManager.Message>();	// ע�����Ϣ

	// �򿪹ر�����
    private bool isShow = false;			// ��ǰUI�Ƿ���ʾ

    protected UIBaseView(string name)
    {
        this.name = name;
        this.openParams.Clear();
    }

    public void SetOpenParams(List<object> openParams)
    {
        this .openParams = openParams;
    }

    public void Init(Action initFinishCall)
    {
        // ������
        if (IsIniting())
        {
            return;
        }

        // ������
        if (IsInit())
        {
            if (initFinishCall != null)
            {
                initFinishCall();
            }
            return;
        }

        // δ����
        uiInitFinishCall = initFinishCall;
        ShowFullScreenMask();
        uiInitRequestId = ResourceManager.CreateGameObjectAsync(LoadGameObjectType.UI , name, (instanceId , requestId) => { OnCreateInstance(instanceId , requestId); });
    }

    private void OnCreateInstance(int instanceId , int requestId)
    {
        if(uiInitRequestId != requestId)
        {
            Debug.LogError("UIBaseView ���ش���");
            return;
        }

        CloseFullScreenMask();

        if(instanceId == 0)
        {
            Debug.LogError("OnCreateInstance is error " + name);
            return;
        }

        uiInitRequestId = 0;
        uiInstanceId = instanceId;

        // �������
        OnCreate();

        // ��UICore Ӧ�ò���Ҫ��
        BindUICore();

        //  ע��UI�¼�
        OnRegisterMessage();

        if(uiInitFinishCall != null)
        {
            uiInitFinishCall();
            uiInitFinishCall = null;
        }
    }

    public void Show(bool isBack)
    {
        if (IsShow())
        {
            OnShow();
            return;
        }

        // ����㼶
        SetUILayer();
        // ����ͼ��
        LoadAtlas(() => { OnShowBefore(isBack); });
    }

    public void Close(bool isDestory, Action closeFinishCall)
    {
        if (!IsShow())
        {
            CloseAfter(isDestory,closeFinishCall);
            return;
        }

        CloseFullScreenMask();

        // 1 �ͷ�ͼ��
        ReleaseAtlas();

        // 2 �ر�GameObject
        GameObject obj = GetGameObjectById(uiInstanceId);
        if(obj == null)
        {
            Debug.LogError("Close is Error" + name);
            return;
        }

        isShow = false;
        obj.SetActive(false);

        // 3 ɾ��UI�㼶
        DelUILayer();

        // 4 �ر����
        OnClose();

        // 5 �ر�֮�������
        CloseAfter(isDestory, closeFinishCall);
    }

    private void CloseAfter(bool isDestory, Action closeFinishCall)
    {
        if(closeFinishCall != null)
        {
            closeFinishCall();
        }

        if(isDestory)
        {
            UnInit();
        }
    }

    private void UnInit()
    {
        if (IsShow())
        {
            Debug.LogError("Please Close it first " + name);
            return;
        }

        // ȡ������
        if (IsIniting())
        {
            ResourceManager.CancelCreateAssetAsync(uiInitRequestId);
            uiInitRequestId = 0;
            return;
        }

        // �Ѿ�ж��
        if (!IsInit())
        {
            return;
        }

        // 1 ������Ϣ
        RemoveRegisterMesssge();
        // 2 ���UICore
        UnBindUICore();
        // 3 ж��GameObject
        DestoryGameObject(uiInstanceId);
        // 4 ��������
        ClearParams();
        // 5 ж�����
        OnDestory();
    }

    private void BindUICore()
    {
        // TODO 3 ��Ҫ����
    }

    private void UnBindUICore()
    {
        // TODO 3 ��Ҫ����
    }

    protected List<object> GetParams()
    {
        return openParams;
    }

    private void ClearParams()
    {
        openParams.Clear();
    }

    private void OnShowBefore()
    {
        // ��ȡʵ��
        GameObject obj = GetGameObjectById(uiInstanceId);
        if(obj == null)
        {
            Debug.LogError("OnShowBefore is Error " + name);
            return;
        }

        // ��ʾ
        isShow = true;
        obj.gameObject.SetActive(isShow);

        // ��ʾ���
        OnShow();

        // ֪ͨUI�Ѿ���

    }

    public bool IsIniting()
    {
        return uiInitRequestId > 0;
    }

    public bool IsInit()
    {
        return uiInstanceId != 0;
    }

    // TODO ȫ�����ִ���
    public void ShowFullScreenMask() { }
    public void CloseFullScreenMask(){ }


    public bool IsShow()
    {
        return isShow;
    }

    protected void RegisterMessage(MessageManager.Message message)
    {
        if(!registerMessages.Contains(message))
        {
            registerMessages.Add(message);
        }

        MessageManager.Instance.RegisterMessage(message);
    }

    private void RemoveRegisterMesssge() 
    { 
        for(int i = 0; i < registerMessages.Count; i++)
        {
            MessageManager.Instance.RemoveMessage(registerMessages[i]);
        }
        registerMessages.Clear();
    }

    private void SetUILayer()
    {
        // TODO 0 ��Ҫ��̬����㼶
    }

    private void DelUILayer()
    {
        // TODO 0 ��Ҫ��̬����㼶
    }

    private void LoadAtlas(Action finishCall)
    {
        // TODO ��Ҫ����
        finishCall();
    }

    private void ReleaseAtlas()
    {
        // TODO ��Ҫ����
    }

    private void OnShowBefore(bool isBack)
    {
        GameObject obj = GetGameObjectById(uiInstanceId);
        if(obj == null )
        {
            Debug.LogError("OnShowBefore is Error " + name);
            return;
        }

        // ��ʾ
        isShow = true;
        obj.SetActive(isShow);

        // ��ʾ���
        OnShow();

        // ֪ͨUI�Ѿ���
        MessageManager.Instance.SendMessage(MessageManager.Instance.BeginMessage(MessageConst.UI_Open));
    }

    public bool IsAutoMainUI()
    {
        UIType uIType = UIConfig.GetUIType(name);
        return uIType == UIType.AutoMain;
    }

    public bool IsMainUI()
    {
        UIType uIType = UIConfig.GetUIType(name);
        return uIType == UIType.Main;
    }

    public bool IsSonUI()
    {
        UIType uIType = UIConfig.GetUIType(name);
        return uIType == UIType.Son;
    }

    private GameObject GetGameObjectById(int uiInstanceId)
    {
        return ResourceManager.GetGameObjectById(uiInstanceId);
    }

    private void DestoryGameObject(int uiInstanceId)
    {
        ResourceManager.DestoryGameObject(uiInstanceId);
    }

    protected void RegisterButtonClick(string path ,UnityAction clickCall)
    {
        GameObject o = GetGameObject(path);
        if(o == null)
        {
            Debug.LogError("RegisterButtonClick is Error " + path + " " + name);
            return;
        }
        o.GetComponent<Button>().onClick.AddListener(clickCall);
    }

    protected void RegisterButtonClick(string path, Action<int> clickCall , int p)
    {
        GameObject o = GetGameObject(path);
        if (o == null)
        {
            Debug.LogError("RegisterButtonClick is Error " + path + " " + name);
            return;
        }
        o.GetComponent<Button>().onClick.AddListener(() => {
            clickCall(p);
        });
    }

    protected void RegisterButtonClick(Button btn, Action<int> clickCall, int p)
    {
        btn.onClick.AddListener(() => {
            clickCall(p);
        });
    }

    protected void RegisterButtonClick(Button btn, UnityAction clickCall)
    {
        btn.onClick.AddListener(clickCall);
    }

    protected GameObject GetGameObject()
    {
        return GetGameObjectById(uiInstanceId).gameObject;
    }

    protected GameObject GetGameObject(string path)
    {
        return GetGameObjectById(uiInstanceId).transform.Find(path).gameObject;
    }

    #region ������д
    // �������
    public abstract void OnCreate();
    // ע���¼�
    public abstract void OnRegisterMessage();
    // �����
    public abstract void OnShow(bool isBack = false);
    // �ر����
    public abstract void OnClose();

    // ж�����
    public abstract void OnDestory();
    #endregion
}
