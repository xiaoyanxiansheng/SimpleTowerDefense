using System;
using System.Collections.Generic;
using UnityEngine;
using static NewSkillBase;
using static UnityEngine.EventSystems.EventTrigger;

public class NewSkillBase : MonoBehaviour
{

    public class SkillHit
    {
        private bool _isFinish = false;

        private NewSkillBase _newskillBase;
        private EntityBase _followEntity;
        private Vector2 _startPos;
        private Vector2 _curPos;
        private SKillConfigDataComboItem _sKillConfigDataItem;
        private Vector2 _attackEndPos;

        private float _hitContinueTimeCount;
        private float _IntervalHitTimeCount;

        private Action<SkillHit> _outCombatAreaCall;
        private Action<SkillHit,EntityBase> _hitEntityCall;
        private Action<SkillHit> _attackEntityFinishCall;

        public SkillHit(NewSkillBase newSkillBase, Action<SkillHit, EntityBase> hitEntityCall, Action<SkillHit> attackEntityFinishCall, Action<SkillHit> outCombatAreaCall)
        {
            _newskillBase = newSkillBase;
            _followEntity = _newskillBase.GetFollowEntity();
            _startPos = _newskillBase.GetPosition();
            _curPos = _startPos;
            _sKillConfigDataItem = _newskillBase.GetSkillConfig();
            _attackEndPos = _followEntity.GetPosition();
            _outCombatAreaCall = outCombatAreaCall;
            _hitEntityCall = hitEntityCall;
            _attackEntityFinishCall = attackEntityFinishCall;
            _IntervalHitTimeCount = 0;
        }

        private void OutCombatAreaCall()
        {
            _isFinish = true;
            _outCombatAreaCall(this);
        }

        private void HitEntityCall(EntityBase entity)
        {
            _hitEntityCall(this, entity);
            AttackEntityFinishCall();
        }

        private void AttackEntityFinishCall()
        {
            _isFinish = true;
            _attackEntityFinishCall(this);
        }

        public void UpdateHit()
        {
            if (_isFinish) { return; }

            if (CommonUtil.OutCombatArea(_curPos))
            {
                OutCombatAreaCall();
                return;
            }

            // ���嵥��
            if (_sKillConfigDataItem.attackNum == 1)
            {
                 if (_sKillConfigDataItem.skillReleaseType == SkillReleaseType.Dir)
                {
                    HitMoveToDir();
                }
                else if (_sKillConfigDataItem.skillReleaseType == SkillReleaseType.Follow)
                {
                    HitMoveToEntity();
                }
                else
                {
                    HitMoveToPoint();
                }
            }
            // ��Χ����
            else
            {
                if (_hitContinueTimeCount <= _newskillBase.GetContinueTime())
                {
                    if (_IntervalHitTimeCount <= 0)
                    {
                        _IntervalHitTimeCount = _newskillBase.GetIntervalHitTime();
                        if (_sKillConfigDataItem.skillRangeType == SkillRangeType.Rect)
                        {
                            HitRangeRect();
                        }
                        else
                        {
                            HitRangeCircle();
                        }
                    }
                    _IntervalHitTimeCount -= Time.deltaTime;
                    if(_newskillBase.GetIntervalHitTime() == 0)
                    {
                        _IntervalHitTimeCount = float.MaxValue;
                    }
                }
                else
                {
                    AttackEntityFinishCall();
                }
                _hitContinueTimeCount += Time.deltaTime;
            }
        }

        /// <summary>
        /// ���㼼�ܣ����������������
        /// </summary>
        /// <returns></returns>
        protected void HitMoveToPoint()
        {
            if (IsHited(GetEndPos(), _curPos))
            {
                HitEntityCall(null);    // һ�����ﵱǰ��
                return;
            }

            if (_newskillBase.GetSpeed() == 0)
            {
                _curPos = GetEndPos();
            }
            else
            {
                _curPos += GetAttackDir(false) * _newskillBase.GetSpeed();
            }

            return;
        }

        /// <summary>
        /// �����ͼ��ܣ��ܹ����赲
        /// </summary>
        /// <returns></returns>
        protected void HitMoveToDir()
        {
            EntityBase entity = GameApp.Instance.entityManager.GetEntityByDistance(EntityType.Enemy, _curPos, 50f);   // TODO BOX��С��Ҫͨ����������
            if (entity != null)
            {
                HitEntityCall(entity);  // ���ܴ򲻵���
                return;
            }

            _curPos += GetAttackDir(false) * _newskillBase.GetSpeed();
        }

        /// <summary>
        /// ���漼��
        /// </summary>
        /// <returns></returns>
        protected void HitMoveToEntity()
        {
            if (_followEntity != null)
            {
                if (IsHited(GetEndPos(), _curPos))
                {
                    HitEntityCall(_followEntity);
                    return;
                }
                if (_newskillBase.GetSpeed() == 0)
                {
                    _curPos = GetEndPos();
                }
                else
                {
                    _curPos += GetAttackDir(true) * _newskillBase.GetSpeed();
                }
                return;
            }
            else
            {
                HitEntityCall(null);
                return;
            }
        }


        /// <summary>
        /// Բ�η�Χ
        /// </summary>
        /// <returns></returns>
        protected void HitRangeCircle()
        {
            List<EntityBase> entityBases = new List<EntityBase>();
            GameApp.Instance.entityManager.GetEntityByDistances(EntityType.Enemy, ref entityBases, _curPos, _newskillBase.GetDistance() * 0.5f);
            foreach (EntityBase entityBase in entityBases)
            {
                _hitEntityCall(this, entityBase);
            }
        }

        protected void HitRangeRect()
        {
            // TODO ��ʱ����
        }

        private bool IsHited(Vector2 p1, Vector2 p2)
        {
            if (Vector2.Distance(p1, p2) <= 50f) // TODO
            {
                return true;
            }
            return false;
        }


        public Vector2 GetAttackDir(bool isSynamicDir = false)
        {
            return (GetEndPos() - (isSynamicDir ? GetCurPos() : GetStartPos())).normalized;
        }

        public virtual Vector2 GetEndPos()
        {
            return _sKillConfigDataItem.skillReleaseType == SkillReleaseType.Follow ? _followEntity != null ? _followEntity.GetPosition() : _attackEndPos : _attackEndPos;
        }

        public Vector2 GetCurPos()
        {
            return _curPos;
        }

        public Vector2 GetStartPos()
        {
            return _startPos;
        }

        public bool IsFinish()
        {
            return _isFinish;
        }
    }

    protected int skillId;
    protected int skillLevel;

    protected int skillIndex = 0;
     protected Vector2 position;

    private EntityBase _attackEntity;
    private EntityBase _followEntity;

    private float _intervalTimeCount;

    private SKillConfigDataComboItem _sKillConfigDataComboItem;

    private List<SkillHit> _skillHits = new List<SkillHit>();

    public virtual void InitSkill(int skillId , int skillLevel , int skillIndex)
    {
        this.skillId = skillId;
        this.skillLevel = skillLevel;
        this.skillIndex = skillIndex;
        _sKillConfigDataComboItem = GetSKillConfigh();
    }

    public void Play(Vector2 position)
    {
        this.position = position;
        _intervalTimeCount = 0;
    }

    public void Play(EntityBase attackEntity)
    {
        _attackEntity = attackEntity;
        _intervalTimeCount = 0;
    }

    public void SetSkillLevel(int skillLevel)
    {
        this.skillLevel = skillLevel;
        _sKillConfigDataComboItem = GetSKillConfigh();
    }

    public SKillConfigDataComboItem GetSKillConfigh()
    {
        return GameApp.Instance.GetSkillConfigItem(skillId, skillLevel, skillIndex);
    }

    protected void Update()
    {
        UpdateAttack();

        OnAttackUpdate();

        UpdateHit();
    }

    protected void UpdateAttack()
    {
        if (_intervalTimeCount <= 0)
        {
            if (CheckAttack())
            {
                _intervalTimeCount = GetIntervalTime();
                OnAttack();
            }
            
        }

        _intervalTimeCount -= Time.deltaTime;

        // ֻ�ܹ���һ��
        if (GetIntervalTime() == 0)
        {
            _intervalTimeCount = float.MaxValue;
        }
    }

    private void OnAttack()
    {
        OnAttackStart();
    }

    private void UpdateHit()
    {
        foreach (SkillHit skillHit in _skillHits)
        {
            skillHit.UpdateHit();
            OnHitUpdate(skillHit);

            // �ͷź�������
        }

        for (int i = _skillHits.Count - 1; i >= 0; i--)
        {
            if (_skillHits[i].IsFinish())
            {
                _skillHits.Remove(_skillHits[i]);
            }
        }
    }

    public static NewSkillBase CreateSKill(GameObject gameObject, int skillId,int skillLevel,int skillIndex = 0)
    {
        NewSkillBase skill = null;
        var config = GameApp.Instance.GetSkillConfigItem(skillId, skillLevel, skillIndex);
        if (config.skillType == SkillType.SkillSingleOne) skill = CommonUtil.AddMissingComponent<NewSkillSingleOne>(gameObject);
        else if (config.skillType == SkillType.SkillSingleOneContinue) skill = CommonUtil.AddMissingComponent<NewSkillSingleOneContinue>(gameObject);
        else if (config.skillType == SkillType.SkillRangeCircle) skill = CommonUtil.AddMissingComponent<NewSkillRangeCircle>(gameObject);
        if (skill != null) skill.InitSkill(skillId, skillLevel, skillIndex);
        return skill;
    }

    protected EntityBase GetFollowEntity()
    {
        return _followEntity;
    }

    protected void SetFollowEntity(EntityBase followEntity)
    {
        _followEntity = followEntity;
    }

    public float GetDistance()
    {
        return _sKillConfigDataComboItem.distanceX;
    }

    public float GetSpeed()
    {
        return _sKillConfigDataComboItem.speed;
    }

    public int GetAttackNum()
    {
        return _sKillConfigDataComboItem.attackNum;
    }

    public float GetIntervalTime()
    {
        return _sKillConfigDataComboItem.intervalTime;
    }

    public float GetContinueTime()
    {
        return _sKillConfigDataComboItem.continueTime;
    }

    public float GetIntervalHitTime()
    {
        return _sKillConfigDataComboItem.intervalHitTime;
    }

    public List<SKillEffectConfigDataItem> GetEffect()
    {
        return _sKillConfigDataComboItem.effects;
    }

    public virtual Vector2 GetPosition()
    {
        return _attackEntity == null ?  position : _attackEntity.GetPosition();
    }

    public SKillConfigDataComboItem GetSkillConfig()
    {
        return _sKillConfigDataComboItem;
    }

    public int GetSkillId()
    {
        return skillId;
    }

    public int GetSkillLevel()
    {
        return skillLevel;
    }

    #region ����̳�
    protected virtual bool CheckAttack()
    {
        if (_followEntity)
        {
            if (Vector2.Distance(_followEntity.GetPosition(), GetPosition()) <= GetDistance())
                return true;
        }
        _followEntity = GameApp.Instance.entityManager.GetEntityByDistance(EntityType.Enemy, GetPosition(), GetDistance());
        return _followEntity != null;
    }

    /// <summary>
    /// ÿ֡����
    /// </summary>
    protected virtual void OnAttackUpdate()
    {

    }

    /// <summary>
    /// ÿ֡����
    /// </summary>
    /// <param name="skillHit"></param>
    protected virtual void OnHitUpdate(SkillHit skillHit)
    {

    }

    /// <summary>
    /// �ͷż���
    /// </summary>

    protected virtual void OnOutCombatArea(SkillHit skillHit)
    {
        OnHit(skillHit,null);
    }

    /// <summary>
    /// ��ʼ�ͷż���
    /// </summary>
    /// <param name="skillHit"></param>
    protected virtual void OnAttackStart()
    {
        SkillHit skillHit = new SkillHit(this, OnHit, OnAttackEnd, OnOutCombatArea);
        _skillHits.Add(skillHit);
        OnHitStart(skillHit);
    }

    protected virtual void OnHitStart(SkillHit skillHit)
    {

    }

    /// <summary>
    /// ���μ���
    /// </summary>
    /// <param name="entity"></param>
    protected virtual void OnHit(SkillHit skillHit, EntityBase entity)
    {
        if(entity != null)
        {
            foreach(var effect in GetEffect())
            {
                entity.SkillEffect.AddEffect(effect);
            }

            if (skillIndex + 1 < GameApp.Instance.SkillConfig.GetSkillIndexCount(skillId, skillLevel))
            {
                // ��λ�����һ�μ��ܣ���ɾ����ǰ����
                NewSkillBase skill = CreateSKill(gameObject, skillId, skillLevel, skillIndex + 1);
                skill.Play(skillHit.GetCurPos());
            }
        }
    }

    /// <summary>
    /// �����ͷŽ���
    /// </summary>
    /// <param name="skillHit"></param>
    protected virtual void OnAttackEnd(SkillHit skillHit)
    {

    }

    #endregion
}
