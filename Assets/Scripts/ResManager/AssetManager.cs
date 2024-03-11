using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class AssetManager
{
    public void Init()
    {

    }

    public void OnEnable()
    {

    }

    public GameObject LoadAsset(string assetPath)
    {
        return null;// AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
    }

    public void OnUpdate()
    {
        
    }

    public GameObject CreateEntity<T>(int entityId) where T : EntityBase
    {
        GameObject entityPrefab = GetEntityConfig(entityId).prefab;
        GameObject entity = CommonUtil.CreateGameObjectParent(entityPrefab, GameApp.Instance.LogicOriginMoveRoot.gameObject);
        return entity;
    }

    public EntityConfigData GetEntityConfig(int entityId)
    {
        return GameApp.Instance.GetEntityConfig(entityId);
    }

    public void DestoryEntity(GameObject entityObj)
    {
        GameObject.DestroyImmediate(entityObj.gameObject);
    }
}
