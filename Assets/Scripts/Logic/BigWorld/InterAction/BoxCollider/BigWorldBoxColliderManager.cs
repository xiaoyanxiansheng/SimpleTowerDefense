using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BigWorldBoxColliderManager
{
    public static BigWorldBoxColliderManager Instance;

    public static BigWorldBoxColliderManager Create()
    {
        if (Instance == null) new BigWorldBoxColliderManager();

        return Instance;
    }

    public BigWorldBoxColliderManager()
    {
        Instance = this;
    }

    private Dictionary<int,Dictionary<int, bool>> _boxColliders = new Dictionary<int, Dictionary<int, bool>>();

    public void AddCollider(GameObject gameObject)
    {
        BigWorldBoxCollider[] colliders = gameObject.GetComponentsInChildren<BigWorldBoxCollider>();
        foreach (var collider in colliders) 
        {
            int2 point = CommonUtil.GetLocalFloorPoint(CommonUtil.GetLocalPosition(collider.transform.position));
            if(!_boxColliders.ContainsKey(point.x)) _boxColliders[point.x] = new Dictionary<int, bool>();
            _boxColliders[point.x][point.y] = true;
        }
    }

    public int CalCollision(Vector2 position , Vector2 boxSize)
    {
        Vector2 hBoxSize = boxSize * 0.5f;
        int2 p00 = new int2((int)(position.x / Define.CELL_SIZE), (int)(position.y / Define.CELL_SIZE));

        int2 p10 = p00 + new int2(-1, 0);
        if (_boxColliders.ContainsKey(p10.x) && _boxColliders[p10.x].ContainsKey(p10.y))
            if (position.x - hBoxSize.x < p10.x * Define.CELL_SIZE + Define.CELL_SIZE) 
            {
                return 1;
            }
        

        int2 p20 = p00 + new int2(1, 0);
        if (_boxColliders.ContainsKey(p20.x) && _boxColliders[p20.x].ContainsKey(p20.y))
            if (position.x + hBoxSize.x > p20.x * Define.CELL_SIZE)
            {
                return 2;
            }
            

        int2 p01 = p00 + new int2(0, -1);
        if (_boxColliders.ContainsKey(p01.x) && _boxColliders[p01.x].ContainsKey(p01.y))
            if (position.y - hBoxSize.y > p01.y * Define.CELL_SIZE + Define.CELL_SIZE) 
            {
                return 3;
                    
            }
            
        int2 p02 = p00 + new int2(0, 1);
        if (_boxColliders.ContainsKey(p02.x) && _boxColliders[p02.x].ContainsKey(p02.y))
            if (position.y + hBoxSize.y > p02.y * Define.CELL_SIZE) 
            { 
                return 4; 
            }

        return 0;
    }
}