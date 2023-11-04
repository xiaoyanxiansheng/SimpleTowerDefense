using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class CommonUtil
{
    public static void SetTransformLocalPosition(Transform transform, UnityEngine.Vector2 position)
    {
        transform.localPosition = new UnityEngine.Vector3(position.x, position.y,0);
    }

    public static List<int> GetSkillEffects(int skillId)
    {
        return null; // TODO
    }

    public static bool OutCombatArea(Vector2 pos)
    {
        if (pos.x > Define.CELL_COUNT_WIDTH * Define.CELL_SIZE) return true;  
        if (pos.y > Define.CELL_COUNT_HEIGHT * Define.CELL_SIZE) return true;
        if(pos.x < 0 || pos.y < 0) return true;
        return false;
    }

    public static List<Vector2> RealPathConvert(List<int2> cellPaths)
    {
        List<Vector2> realPaths = new List<Vector2>();
        for (int i = 0; i < cellPaths.Count;i++)
        {
            realPaths.Add(new Vector2(cellPaths[i].x* Define.CELL_SIZE, cellPaths[i].y * Define.CELL_SIZE));
        }
        return realPaths;
    }

    public static GameObject CreateGameObjectParent(GameObject prefab, GameObject parent)
    {
        GameObject obj = GameObject.Instantiate(prefab);
        obj.transform.SetParent(parent.transform);
        CommonUtil.SetGameObjectDefault(obj);
        return obj;
    }

    public static void SetGameObjectDefault(GameObject obj)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;
    }

    public static float GetAngle(Vector2 dir1 , Vector2 dir2)
    {
        float angle = Vector2.SignedAngle(dir1, dir2); ;
        return angle;
    }
}
