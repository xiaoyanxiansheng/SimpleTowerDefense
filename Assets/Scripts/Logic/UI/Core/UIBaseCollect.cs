
using System;
using System.Collections.Generic;
using System.Linq;

public class UIBaseCollect
{

    private List<object> _params;                                       // ��ҳ�����
    private List<UIBaseView> initViews = new List<UIBaseView>();        // �Ѿ�����
    private List<UIBaseView> openingViews = new List<UIBaseView>();     // ���ص���δ��
    private List<UIBaseView> openedViews = new List<UIBaseView>();      // �Ѿ���

    public void Init(List<object> ps, Action initFinishCall , List<UIBaseView> uiViews)
    {

        if (!CheckViews(uiViews)) return;

        // UIҳ�����
        this._params = ps;
        // �����б�
        AddInitView(uiViews);

        // ����ǰ����ȫ������UI����ֹ���������ͷ�ֹ��
        ShowFullScreenMask();

        // ��������UI ȫ��������ɺ����������
        int curInitCount = 0;
        int totalInitCount = uiViews.Count;
        for(int i = 0; i < totalInitCount; i++)
        {
            uiViews[i].SetOpenParams(ps);
            uiViews[i].Init(() =>
            {
                curInitCount++;
                // �������
                if(curInitCount == totalInitCount)
                {
                    // �ر�ȫ��Mask
                    CloseFullScreenMask();
                    AddOpeningViews(uiViews);
                    if (initFinishCall != null)
                    {
                        initFinishCall();
                    }
                }
            });
        }
    }

    public void Show(bool isBack = false)
    {
        if (!CheckViews(openingViews)) return;

        // �����еȴ��򿪵�UI
        for(int i = 0;i < openingViews.Count; i++)
        {
            openingViews[i].Show(isBack);
        }

        // �ı�״̬
        AddOpenedViews(openingViews);
        openingViews.Clear();
    }

    public void CloseAll(Action closeFinishCall , bool isDestory = false)
    {
        // ж�����̴�������UI���ر�����ֻ��Ҫ�����Ѿ��򿪵�
        List<UIBaseView> uiViews = isDestory ? initViews : openedViews;
        if (initViews.Count == 0) return;

        // �Ӻ���ǰ�ر�UI
        int uiViewCout = uiViews.Count;
        for(int i = uiViewCout-1; i >= 0 ; i--)
        {
            UIBaseView uiView = uiViews[i];
            // ������Ҫע�� ֻ���Զ�������UI����Ȩ�����֮�󷵻�
            Close(uiView, uiView.IsAutoMainUI() ? closeFinishCall : null, isDestory);
        }
    }

    // �ر�UI���ϲ��ұ��浱ǰ��UI������״̬��Ŀ�ģ�ȥ��ĳ������֮��������´�֮ǰ��UI���ϣ�eg��ս����������֮ǰ�Ľ��棩
    public void CloseAllAndSave(Action closeFinishCall , bool isDestory = false)
    {
        // ����֮ǰ�Ĵ�״̬
        List<UIBaseView> uiViews = new List<UIBaseView> ();
        for(int i = 0; i < openedViews.Count; i++)
        {
            uiViews.Add(openedViews[i]);
        }

        // �رյ�ǰ�򿪵�UI����
        CloseAll(closeFinishCall , isDestory);

        // ���뵽�����б��� �ȴ��������̵�ʱ���
        AddOpeningViews(uiViews);
    }

    // �ر�UI
    public void Close(UIBaseView uiView , Action closeFinishCall = null , bool isDestory = false)
    {
        RemoveOpeningView(uiView);
        RemoveOpendedView(uiView);
        if(isDestory)
        {
            RemoveInitView(uiView);
        }
        uiView.Close(isDestory, closeFinishCall);
    }

    private void AddInitView(List<UIBaseView> uiViews , bool isReset = false)
    {
        if (isReset) initViews.Clear();

        initViews.AddRange(uiViews);
    }

    private void AddOpeningViews(List<UIBaseView> uiViews , bool isReset = false)
    {
        if (isReset)
        {
            openingViews.Clear();
        }

        openingViews.AddRange(uiViews);
    }

    private void AddOpenedViews(List<UIBaseView> uiViews , bool isReset = false)
    {
        if(isReset) 
        {
            openedViews.Clear();
        }

        openedViews.AddRange(uiViews);
    }

    private void RemoveInitView(UIBaseView uiView)
    {
        initViews.Remove(uiView);
    }
    private void RemoveOpeningView(UIBaseView uiView)
    {
        openingViews.Remove(uiView);
    }
    private void RemoveOpendedView(UIBaseView uiView)
    {
        openedViews.Remove(uiView);
    }

    public List<UIBaseView> GetInitViews()
    {
        return initViews;
    }

    // ��Ҫ�򿪵�UI�Ƿ������UI
    public bool IsContainAutoMainUI()
    {
        foreach (UIBaseView view in openingViews)
        {
            if (view.IsAutoMainUI())
            {
                return true;
            }
        }
        return false;
    }

    public bool IsAllSonUI()
    {
        foreach (UIBaseView view in openingViews)
        {
            if (view.IsSonUI())
            {
                return false;
            }
        }
        return true;
    }

    public bool IsAotuMainCollet()
    {
        return GetMain().IsAutoMainUI();
    }

    public bool IsMainCollet()
    {
        return GetMain().IsMainUI();
    }

    private bool CheckViews(List<UIBaseView> uiViews)
    {
        return uiViews != null && uiViews.Count > 0;
    }

    private void ShowFullScreenMask()
    {
        // TODO 5
    }

    private void CloseFullScreenMask()
    {
        // TODO 5
    }

    public UIBaseView GetMain()
    {
        return initViews[0];
    }

    public bool IsAutoMainCollect()
    {
        return GetMain().IsAutoMainUI();
    }
}
