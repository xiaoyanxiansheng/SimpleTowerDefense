using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkillBaseData : SkillBaseData
{

}

public class AttackSkillBase : SkillBase
{
    // �������е��˺������ӵ�Ч������
    //private List<Dictionary<string,int>> _effects = new List<Dictionary<string, int>>();

    public void Play(AttackSkillBaseData attackSkillBaseData)
    {
        base.Play(attackSkillBaseData);
    }

    private void OnEnable()
    {
    }

    ///// <summary>
    ///// �����ͷŵĹ�����/�ͷ���ɺ��Ч��
    ///// </summary>
    ///// <param name="effectSkillBases"></param>
    //public void DoEffects(List<EffectSkillBase> effectSkillBases)
    //{
    //    for (int i = 0; i < effectSkillBases.Count; i++) 
    //    {
    //        for (int j = 0; j < _effects.Count; j++)
    //        {
    //            effectSkillBases[i].Play(_effects[i]["skillid"] , _effects[i]["skillLevel"]);
    //        } 
    //    }
    //}

    protected bool CheckAttackOne()
    {
        EntityBase hitEntity = GetHitEntityOne();
        
        if(hitEntity != null)
        {
            if (Vector2.Distance(hitEntity.GetPosition(), GetPosition()) > GetAttackDistance())
            {
                hitEntity = null;
            }
        }

        if (hitEntity == null)
        {
            hitEntity = EntityManager.Instance.GetEntityByDistance(GetAttackEntity().GetEntityType(), GetPosition(), attackDistance);
            if (hitEntity != null) 
            {
                hitEntitys.Clear();
                hitEntitys.Add(hitEntity);
            }
        }

        return hitEntity != null;
    }

    protected EntityBase GetHitEntityOne()
    {
        ClearNullReference();
        if (hitEntitys.Count == 0) return null;
        return hitEntitys[0];
    }
}
