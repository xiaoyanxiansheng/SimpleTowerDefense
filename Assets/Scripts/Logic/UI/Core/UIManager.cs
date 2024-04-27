/*
    UI���ⲿ�ṩ�Ĺ�����

    ���� �� �ر� ж��
--*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager{
    public static UIManager Instance;

    public Canvas canvas;

    private List<UIBaseView> _uiViews = new List<UIBaseView>();
    private List<UIBaseCollect> _initViewCollects = new List<UIBaseCollect>();
    private List<UIBaseCollect> _openViewCollects = new List<UIBaseCollect>();

    public UIManager()
    {
        Instance = this;
        canvas = GameObject.Find("UIRoot").GetComponent<Canvas>();
    }

    public void InitBase()
    {
        ResourceManager.CreateGameObjectAsync(LoadGameObjectType.UI, "ui/uiprefab/ui_base", (int instanceId, int requestId) =>
        {
            GameObject obj = ResourceManager.GetGameObjectById(instanceId);
            if(obj != null)
            {
                CommonUtil.TrimGameObejct(obj);
                obj.SetActive(true);
                // TODO 0 ���س���ʱ��Ҫɾ��
                OpenMainUI();
            }
            else
            {
                Debug.LogError("UIManager init is error ");
            }
        });
    }

    private void OpenMainUI()
    {
        Open(UIViewName.UIMain);
    }

    #region �����ṩ
    // ����UI
    public void Init(List<object> ps , Action<UIBaseCollect> initFinishCall , params string[] viewNames)
    {
        List<UIBaseView> views = new List<UIBaseView>();
        for(int i = 0; i < viewNames.Length; i++)
        {
            views.Add(UIConfig.GetUIView(viewNames[i]));
        }
        // ��ȡ����
        UIBaseCollect viewCollect = GetViewCollect(views[0]);
        if(viewCollect == null)
        {
            Debug.LogError("UIManager init is error");
            return;
        }

        _initViewCollects.Remove(viewCollect);
        _initViewCollects.Add(viewCollect);

        viewCollect.Init(ps, () =>
        {
            if (initFinishCall != null)
            {
                initFinishCall(viewCollect);
            }
        }, views);
    }
    
    // ��UI����UI���� 
    // 1 ��ǰUI����UI �ر���һ��UI�����ٴ򿪵�ǰ
    // 2 ��ǰUI������UI ֱ�Ӵ�UI
    public void Open(List<object> ps, params string[] uiNames)
    {
        Init(ps, (UIBaseCollect viewCollect) =>
        {
            UIBaseCollect topViewCollect = GetTopAutoShowMainViewCollect();
            if(topViewCollect != null && topViewCollect != viewCollect)
            {
                // ֻ����UI��������Ϊ1�ĲŻ�����Զ�����
                if (viewCollect.IsAotuMainCollet())
                {
                    // �ȹرյ�ǰUI���� Ȼ���ڴ���һ��UI����
                    CloseViewCollect(topViewCollect, () =>
                    {
                        OpenViewCollect(viewCollect);
                    }, false , true);
                }
                else
                {
                    OpenViewCollect(viewCollect);
                }
            }
            else
            {
                OpenViewCollect(viewCollect);
            }
        }, uiNames);
    }

    public void Open(params string[] uiNames)
    {
        Open(null, uiNames);
    }

    public void Close(string viewName , bool isDestory = false)
    {
        UIBaseView view = UIConfig.GetUIView(viewName);
        UIBaseCollect viewCollect = GetViewCollect(view);
        if(viewCollect == null) 
        {
            Debug.LogError("UIManager Close Error");
            return;
        }

        if(view.IsAutoMainUI())
        {
            // �ȹر�UI���� Ȼ���ڴ��ϴιرյ�UI����
            _openViewCollects.Remove(viewCollect);
            CloseViewCollect(viewCollect, () =>
            {
                UIBaseCollect topViewCollect = GetTopAutoMainViewCollect();
                if(topViewCollect != null)
                {
                    OpenViewCollect(topViewCollect,true);
                }
            }, isDestory);
        }
        else
        {
            viewCollect.Close(view);
        }
    }

    public void OpenViewCollect(UIBaseCollect viewCollect , bool isBack = false)
    {
        _openViewCollects.Remove(viewCollect);
        _openViewCollects.Add(viewCollect);
        viewCollect.Show(isBack);
    }

    public void CloseViewCollect(UIBaseCollect viewCollect , Action closeFinishCall , bool isDestory , bool isBack = false)
    {
        if(isDestory)
        {
            _initViewCollects.Remove(viewCollect);
        }

        if(isBack)
        {
            viewCollect.CloseAllAndSave(closeFinishCall, isDestory);
        }
        else
        {
            viewCollect.CloseAll(closeFinishCall, isDestory);
        }
    }

    #endregion

    #region �������ṩ
    // ��ȡ��ʾUI����(���ϲ��Զ����̼��ϣ�������UI����Ϊ1��UI����)
    // ��Ϊ��UI�����֣�����1 �ᱻ�Զ��򿪹ر�Ӱ�죬����2 ���᣻������Ҫ��������1����UI
    private UIBaseCollect GetTopAutoShowMainViewCollect()
    {
        for (int i = _openViewCollects.Count - 1; i >= 0; i--)
        {
            if (_openViewCollects[i].IsAutoMainCollect()) return _openViewCollects[i];
        }
        return null;
    }

    // ��ȡUI����
    private UIBaseCollect GetTopAutoMainViewCollect()
    {
        for (int i = _openViewCollects.Count - 1; i >= 0; i--)
        {
            if(_openViewCollects[i].IsAutoMainCollect())return _initViewCollects[i];
        }
        return null;
    }

    // ��ȡUI����
    private UIBaseCollect GetViewCollect(UIBaseView uiView)
    {
        for(int i = _initViewCollects.Count - 1; i >= 0; i--)
        {
            foreach(UIBaseView view in _initViewCollects[i].GetInitViews())
            {
                if (uiView == view) return _initViewCollects[i];
            }
        }
        return new UIBaseCollect();
    }

    #endregion
}
