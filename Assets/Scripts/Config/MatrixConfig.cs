using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class MatrixConfigDataItem
{
    public int TowerId;
    public Vector2 Position;
}

[Serializable]
public class MatrixConfigData
{
    public string desc = "";
    public int MatrixId = 0;
    [SerializeField] public List<MatrixConfigDataItem> matrixConfigDataItems;
    [SerializeField] public List<NewSkillEffect> newSkillEffects;
}

[CreateAssetMenu(fileName = "MatrixConfig", menuName = "Config/MatrixConfig", order = 1)]
public class MatrixConfig : ScriptableObject
{
    [SerializeField]
    public List<MatrixConfigData> MatrixConfigDatas = new List<MatrixConfigData>();

    public MatrixConfigData GetMatrixConfigData(int MatrixId)
    {
        for (int i = 0; i < MatrixConfigDatas.Count; i++)
        {
            if (MatrixConfigDatas[i].MatrixId == MatrixId)
                return MatrixConfigDatas[i]; 
        }
        return null;
    }
}
