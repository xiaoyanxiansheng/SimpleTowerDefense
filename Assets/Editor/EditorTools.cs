using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class EditorTools
{
    [MenuItem("GameObject/EditorTools/ReNameChild(00)", false, 0)]
    private static void ReNameChild(MenuCommand menuCommand)
    {
        GameObject go = menuCommand.context as GameObject;

        for (int i = 0;i<go.transform.childCount;i++)
        {
            go.transform.GetChild(i).name = string.Format("{0:D3}", i);
            if(go.transform.GetChild(i).GetComponent<Text>() != null)
                go.transform.GetChild(i).GetComponent<Text>().text = i.ToString();
        }
    }
}
