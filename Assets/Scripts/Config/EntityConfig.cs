using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityConfigData
{
    public string desc = "";
    public int EntityId = 0;
    public GameObject prefab;
    public string prefabPath;
    public int Health;
    public float MoveSpeed;
    public int SkillId;
    public int SkillLevel; 
    // Buff ап╠М
}

[CreateAssetMenu(fileName = "EntityConfig" , menuName = "Config/EntityConfig" , order = 1)]
public class EntityConfig : ScriptableObject
{
    [SerializeField]
    public List<EntityConfigData> EntityConfigDatas = new List<EntityConfigData>();  
    
    public EntityConfigData GetEntityConfigData(int EntityId)
    {
        for (int i = 0; i < EntityConfigDatas.Count; i++)
        {
            if (EntityConfigDatas[i].EntityId == EntityId)
                return EntityConfigDatas[i];
        }
        return null;
    }
}
