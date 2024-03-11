using System.Collections.Generic;
using UnityEngine;
public static class UIConfig
{
    public class UIPrefabScript
    {
        public UIBaseView uiView;
        public UIType uiType;
        public int topLayer;

        public UIPrefabScript(UIBaseView uiView, UIType uiType, int topLayer = 0)
        {
            this.uiView = uiView;
            this.uiType = uiType;
            this.topLayer = topLayer;
        }
    }

    public static UIBaseView GetUIView(string name)
    {
        foreach (var script in _uiPrefabScripts)
        {
            if (script.uiView.name == name)
            {
                return script.uiView;
            }
        }

        Debug.Log("UIConfig GetUIView Error " + name);
        return null;
    }

    public static UIType GetUIType(string name)
    {
        foreach(var script in _uiPrefabScripts)
        {
            if(script.uiView.name == name)
            {
                return script.uiType;
            }
        }

        Debug.LogError("UIConfig GetUIType Error " + name);
        return UIType.AutoMain;
    }

    private static UIPrefabScript[] _uiPrefabScripts = new UIPrefabScript[]
    {
        new UIPrefabScript(new UIMain(UIViewName.UIMain),UIType.AutoMain),
        new UIPrefabScript(new UIBattleMain(UIViewName.UIBattleMain),UIType.AutoMain)
    };

}

public static class UIViewName
{
    public static string UIMain = "Prefab/UI/Main/UIMain";
    public static string UIBattleMain = "Prefab/UI/Battle/UIBattleMain";
}

public enum UIType
{
    AutoMain = 1,
    Main     = 2,
    Son      = 3
}