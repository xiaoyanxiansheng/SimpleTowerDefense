using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BigWorldLoadManager
{
    public static BigWorldLoadManager Instance;

    public static void Create()
    {
        if (Instance != null) return;
        new BigWorldLoadManager();

        BigWorldBoxColliderManager.Create();
    }

    public BigWorldLoadManager() 
    {
        Instance = this;
    }

    public class BigWorldLoadItem
    {
        public int2 MapIndex;
        public int objInstance;
    }

    public int2 MapCellSize = new int2(2000, 1100);
    public int2 CellSize = new int2(10,10);
    [SerializeField]public List<GameObject> Maps = new List<GameObject>();

    private int2 _curMapIndex = new int2(4, 4);
    
    private List<BigWorldLoadItem> _loadedMaps = new List<BigWorldLoadItem>();
    private int2 _leftOffset        = new int2(-1,0);
    private int2 _leftTopOffset     = new int2(-1, 1);
    private int2 _topOffset         = new int2(0, 1);
    private int2 _rightTopOffset    = new int2(1, 1);
    private int2 _rightOffset       = new int2(1, 0);
    private int2 _rightBottomOffset = new int2(1, -1);
    private int2 _bottomffset       = new int2(0, -1);
    private int2 _leftBottomOffset  = new int2(-1, -1);

    public void Update()
    {
        Vector3 localPosition = BigWorldManager.Instance.GetLocalPosition();
        int2 curMapIndex = new int2(Mathf.RoundToInt(localPosition.x / MapCellSize.x), Mathf.RoundToInt(localPosition.y / MapCellSize.y));
        if (curMapIndex.x == _curMapIndex.x && curMapIndex.y == _curMapIndex.y) return;

        CaculateLoad(curMapIndex);
    }

    void CaculateLoad(int2 mapIndex)
    {
        _curMapIndex = mapIndex;

        // и╬ЁЩ
        RemoveMapItem();

        // ╪сть
        LoadMapItem(_curMapIndex);
        LoadMapItem(_curMapIndex + _leftOffset);
        LoadMapItem(_curMapIndex + _leftTopOffset);
        LoadMapItem(_curMapIndex + _topOffset);
        LoadMapItem(_curMapIndex + _rightTopOffset);
        LoadMapItem(_curMapIndex + _rightOffset);
        LoadMapItem(_curMapIndex + _rightBottomOffset);
        LoadMapItem(_curMapIndex + _bottomffset);
        LoadMapItem(_curMapIndex + _leftBottomOffset);
    }

    void LoadMapItem(int2 index)
    {
        foreach(BigWorldLoadItem i in _loadedMaps)
        {
            if(i.MapIndex.x == index.x && i.MapIndex.y == index.y)
            {
                return;
            }
        }
        BigWorldLoadItem item = new BigWorldLoadItem();
        item.MapIndex = index;
        _loadedMaps.Add(item);

        ResourceManager.CreateGameObjectAsync(LoadGameObjectType.UI, GetMapAssetPath(index), (instanceId, requestId) =>
        {
            GameObject newObj = ResourceManager.GetGameObjectById(instanceId);
            if(newObj == null)
            {
                Debug.Log(string.Format("[BigWorldLoadManager] MapIndex is Error {0} _ {1}", index.x, index.y));
            }
            else
            {
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(BigWorldManager.Instance.LocalBackGroundLayer.transform);
                CommonUtil.TrimGameObejct(newObj);
                newObj.transform.localPosition = new Vector3(index.x * MapCellSize.x, index.y * MapCellSize.y, 0);
                item.objInstance = instanceId;

                BigWorldBoxColliderManager.Instance.AddCollider(newObj);
            }
        });
    }

    void RemoveMapItem()
    {
        List<BigWorldLoadItem> waitDeleteList = new List<BigWorldLoadItem>();
        foreach (BigWorldLoadItem item in _loadedMaps)
        {
            if (Mathf.Abs(item.MapIndex.x - _curMapIndex.x) > 1 || Mathf.Abs(item.MapIndex.y - _curMapIndex.y) > 1)
                waitDeleteList.Add(item);
        }
        for(int i = 0; i < waitDeleteList.Count; i++)
        {
            ResourceManager.DestoryGameObject(waitDeleteList[i].objInstance);
            _loadedMaps.Remove(waitDeleteList[i]);
        }
    }

    public string GetMapAssetPath(int2 mapIndex)
    {
        return string.Format("Prefab/BigWorld/BackGround/{0}_{1}", mapIndex.x, mapIndex.y);
    }

    public int2 GetMapIndex(int index)
    {
        return new int2(index % CellSize.x , index / CellSize.x);
    }

    public int GetListIndex(int2 index)
    {
        return index.y * CellSize.x + index.x;
    }
}
