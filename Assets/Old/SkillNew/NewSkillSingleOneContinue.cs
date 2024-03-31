//using Unity.VisualScripting;
//using UnityEngine;

//public class NewSkillSingleOneContinue : NewSkillBase
//{
//    private BattleEntityBase _bullet = null;

//    public override void InitSkill(int skillId, int skillLevel, int skillIndex)
//    {
//        base.InitSkill(skillId, skillLevel, skillIndex);
//        _bullet = GameApp.Instance.entityManager.CreateEntity<BattleEntityBase>(EntityType.SKill, GetSkillConfig().entityId);
//    }

//    private void OnDestroy()
//    {
//        if (_bullet)
//            GameApp.Instance.entityManager.RemoveEntity(_bullet.GetEntityInstanceId());
//    }

//    protected override void OnAttackEnd(SkillHit skillHit)
//    {
//        // Debug.Log("OnAttackEntityEnd"); 
//    }

//    protected override void OnAttackUpdate()
//    {
//        base.OnAttackUpdate();

//        if (!GetFollowEntity()) return;

//        Vector2 curPos = GetFollowEntity().GetPosition();
//        Vector2 startPos = GetPosition();

//        Vector2 dir = (curPos - startPos).normalized;
//        float angle = CommonUtil.GetAngle(Vector2.up, dir);
//        float length = Vector2.Distance(curPos, startPos);

//        Vector3 rotation = _bullet.transform.localRotation.eulerAngles;
//        rotation.z = angle;
//        _bullet.transform.localRotation = Quaternion.Euler(rotation);
//        Vector3 localScale = transform.localScale;
//        localScale.y = length / Define.CELL_SIZE;
//        _bullet.transform.localScale = localScale;
//        _bullet.SetPosition(startPos + dir * length * 0.5f);
//    }
//}
