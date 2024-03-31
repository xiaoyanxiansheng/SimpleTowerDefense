//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class NewSkillBuff
//{
//    private BattleEntityDataBase _dataSystem;
//    private int _sourceHealth;
//    private float _sourceMoveSpeed;

//    protected List<SKillEffectConfigDataItem> sKillEffectConfigDataItems = new List<SKillEffectConfigDataItem>();

//    public NewSkillBuff(BattleEntityDataBase dataSystem)
//    {
//        _dataSystem = dataSystem;
//        _sourceHealth = dataSystem.Health;
//        _sourceMoveSpeed = dataSystem.MoveSpeed;
//    }

//    public void AddEffect(SKillEffectConfigDataItem item)
//    {
//        sKillEffectConfigDataItems.Add(item);
//    }

//    public void Update()
//    {
//        UpdateDataByEffect();
//    }

//    public void UpdateDataByEffect()
//    {
//        if (sKillEffectConfigDataItems.Count == 0) return;

//        int modifyHealth = 0;
//        float modifyMoveSpeed = 0;
//        foreach (var item in sKillEffectConfigDataItems)
//        {
//            float value = (item.addType == AddType.Add ? item.value : (-item.value));
//            if (item.skillEffectValueType == SkillEffectValueType.Value)
//            {
//                if(item.skillEffectType == SkillEffectType.Health) modifyHealth += (int)value;
//                else if (item.skillEffectType == SkillEffectType.MoveSpeed) modifyMoveSpeed += value;
//            }
//            else if(item.skillEffectValueType == SkillEffectValueType.Percent) {
//                if (item.skillEffectType == SkillEffectType.Health) modifyHealth += (int)(value * _sourceHealth);
//                else if (item.skillEffectType == SkillEffectType.MoveSpeed) modifyMoveSpeed += value * _sourceMoveSpeed;
//            }
//        }
//        _dataSystem.Health += modifyHealth;
//        _dataSystem.MoveSpeed += modifyMoveSpeed;

//        // 目前只有一次效果
//        sKillEffectConfigDataItems.Clear();
//    }
//}
