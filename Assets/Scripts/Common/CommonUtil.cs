using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public static class CommonUtil
{
    public static void TrimGameObejct(GameObject go)
    {
        if (go == null) return;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = new Vector3(1, 1, 1);
        go.transform.localRotation = Quaternion.identity;
    }

    public static void SetTransformLocalPosition(Transform transform, UnityEngine.Vector2 position)
    {
        transform.localPosition = new UnityEngine.Vector3(position.x, position.y, 0);
    }

    public static List<int> GetSkillEffects(int skillId)
    {
        return null; // TODO
    }

    public static bool OutCombatArea(Vector2 pos)
    {
        float halfCell = Define.CELL_SIZE * 0.5f;
        if (pos.x - halfCell > Define.CELL_COUNT_WIDTH * Define.CELL_SIZE) return true;
        if (pos.y - halfCell > Define.CELL_COUNT_HEIGHT * Define.CELL_SIZE) return true;
        if (pos.x + halfCell < 0 || pos.y + halfCell < 0) return true;
        return false;
    }

    public static List<Vector2> RealPathConvert(List<int2> cellPaths)
    {
        List<Vector2> realPaths = new List<Vector2>();
        for (int i = 0; i < cellPaths.Count; i++)
        {
            realPaths.Add(new Vector2(cellPaths[i].x * Define.CELL_SIZE, cellPaths[i].y * Define.CELL_SIZE));
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

    public static float GetAngle(Vector2 dir1, Vector2 dir2)
    {
        float angle = Vector2.SignedAngle(dir1, dir2); ;
        return angle;
    }

    public static Vector2 IntConvertVec(int2 val)
    {
        return new Vector2(val.x, val.y);
    }

    public static int2 VecConvertInt(Vector2 val)
    {
        return new int2((int)val.x, (int)val.y);
    }

    public static int2 VecConvertCell(Vector2 val)
    {
        return new int2((int)(val.x / Define.CELL_SIZE), (int)(val.y/ Define.CELL_SIZE));
    }

    public static Vector2 CellConvertVec(int2 cell)
    {
        return new Vector2(cell.x * Define.CELL_SIZE, cell.y * Define.CELL_SIZE);
    }

    public static T AddMissingComponent<T>(GameObject obj) where T : Component
    {
        if(obj.GetComponent<T>() == null) 
        {
            obj.AddComponent<T>();
        }
        return obj.GetComponent<T>();
    }

    public static float CalAngle(Vector2 start , Vector2 end)
    {
        Vector2 direction = end - start;
        float angleRad = Mathf.Atan2(direction.y, direction.x);
        float angleDeg = (180 / Mathf.PI) * angleRad;
        return angleDeg;
    }
}
