using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MatrixBase : TowerBase
{
    public List<MatrixConfigDataItem> matrixConfigDataItems;
    public List<NewSkillEffect> newSkillEffects;

    public override void SetPositionCell(int2 position)
    {
        base.SetPositionCell(position);

        // 位置设置成功之后 ， 开始设置新的Tower

        MatrixConfigData matrixConfigData = GameApp.Instance.MatrixConfig.GetMatrixConfigData(GetEntityId());
        for (int i = 0; i < matrixConfigData.matrixConfigDataItems.Count; i++) 
        {
            var data = matrixConfigData.matrixConfigDataItems[i];
            FixedTower tower = (FixedTower)GameApp.Instance.entityManager.CreateEntity<FixedTower>(EntityType.Tower, data.TowerId);
            tower.SetPosition(GetPosition() + data.Position * Define.CELL_SIZE);
            tower.DoSkill();
        }
    }
}
