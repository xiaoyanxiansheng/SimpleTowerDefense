using System;
using UnityEditor;
using UnityEngine;

public class GameApp : MonoBehaviour
{
    public GameObject LogicOriginRoot;
    public GameObject LogicOriginMoveRoot;
    
    public UnityEngine.Object SkillConfigAsset;
    public UnityEngine.Object BulletConfigAsset;
    public UnityEngine.Object EntityConfigAsset;

    [NonSerialized]
    public EntityConfig EntityConfig;

    public static GameApp Instance;
    public void Awake()
    {
        Instance = this;
    }

    public void OnEnable()
    {
        EntityConfig = AssetDatabase.LoadAssetAtPath<EntityConfig>(AssetDatabase.GetAssetPath(EntityConfigAsset));
    }

    public EntityConfigData GetEntityConfig(int entityId)
    {
        return EntityConfig.GetEntityConfigData(entityId);
    }
}
