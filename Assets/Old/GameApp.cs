//using System;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class GameApp : MonoBehaviour
//{
//    public GameObject LogicOriginRoot;
//    public GameObject LogicOriginMoveRoot;
    
//    public UnityEngine.Object EntityConfigAsset;
//    public UnityEngine.Object SkillConfigAsset;
//    public UnityEngine.Object MatrixConfigAsset;

//    [NonSerialized]
//    public EntityConfig EntityConfig;
//    [NonSerialized]
//    public SkillConfig SkillConfig;
//    [NonSerialized]
//    public MatrixConfig MatrixConfig;

//    public static GameApp Instance;

//    public BattleEntityManager entityManager;
//    public AssetManager assetManager;
//    public AStarPath aStarPath;

//    public void Awake()
//    {
//        Instance = this;
//        //EntityConfig = AssetDatabase.LoadAssetAtPath<EntityConfig>(AssetDatabase.GetAssetPath(EntityConfigAsset));
//        //SkillConfig = AssetDatabase.LoadAssetAtPath<SkillConfig>(AssetDatabase.GetAssetPath(SkillConfigAsset));
//        //MatrixConfig = AssetDatabase.LoadAssetAtPath<MatrixConfig>(AssetDatabase.GetAssetPath(MatrixConfigAsset));

//        entityManager = new EntityManager();
//        assetManager = new AssetManager();
//        aStarPath = new AStarPath();

//        //entityManager.Init();
//        assetManager.Init();
//        aStarPath.Init();
//    }

//    public void OnEnable()
//    {
//        //entityManager.OnEnable();
//        assetManager.OnEnable();
//        aStarPath.OnEnable();
//    }

//    public EntityConfigData GetEntityConfig(int entityId)
//    {
//        return EntityConfig.GetEntityConfigData(entityId);
//    }

//    public SKillConfigDataCombo GetSkillConfig(int skillId , int skillLevel)
//    {
//        return SkillConfig.GetSKillConfigDataCombo(skillId, skillLevel);
//    }

//    public SKillConfigDataComboItem GetSkillConfigItem(int skillId, int skillLevel , int skillIndex)
//    {
//        return SkillConfig.GetSKillConfigDataComboItem(skillId, skillLevel , skillIndex);
//    }

//    public void Update()
//    {
//        //entityManager.OnUpdate();
//        assetManager.OnUpdate();
//        // aStarPath.OnUpdate();
//    }
//}
