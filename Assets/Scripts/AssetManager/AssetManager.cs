using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject LoadAsset(string assetPath)
    {
        return AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
    }

    public void Update()
    {
        
    }

    public GameObject CreateEntity<T>(int entityId) where T : EntityBase
    {
        GameObject entityPrefab = GameApp.Instance.GetEntityConfig(entityId).prefab;
        GameObject entity = CommonUtil.CreateGameObjectParent(entityPrefab, GameApp.Instance.LogicOriginMoveRoot.gameObject);
        return entity;
    }

    public void DestoryEntity(GameObject entityObj)
    {
        GameObject.DestroyImmediate(entityObj.gameObject);
    }
}
