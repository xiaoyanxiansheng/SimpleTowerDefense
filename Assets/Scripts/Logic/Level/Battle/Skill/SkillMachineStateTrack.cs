using UnityEngine;
using static SkillConfig;

public class SkillMachineStateTrack : MachineStateBase
{
    private int _attackerMonoId;
    private int _beAttackerMonoId;
    private TrackConfig _config;
    private SkillTrackEntity _skill;
    private bool isReady = false;
    private float passedTime = 0;
    public SkillMachineStateTrack(string name, StateMachine stateMachine) : base(name, stateMachine)
    {
    }

    public override void OnEnter()
    {
        isReady = false;
        passedTime = 0;
        int skillId = (int)stateMachine.ps[0];
        _attackerMonoId = (int)stateMachine.ps[1];
        _beAttackerMonoId = (int)stateMachine.ps[2];
        _config = GameApp.Instance.SkillConfig.GetSkillConfigData(skillId).trackConfig;
        EntityManager.Instance.CreateSkillTrack(skillId, (entity) =>
        {
            isReady = true;

            EntityBase attack = EntityManager.Instance.GetEntity(_attackerMonoId);
            EntityBase beAttacker = EntityManager.Instance.GetEntity(_beAttackerMonoId);

            if(attack != null && beAttacker != null)
            {
                Vector2 startPos = attack.GetPos();
                Vector2 endPos = beAttacker.GetPos();
                _skill = (SkillTrackEntity)entity;
                _skill.SetOwerEntity(_attackerMonoId);
                _skill.SetRotationZ(attack.GetRotation().z);
                _skill.EnterBattle(startPos);
            }
        });
    }

    public override void OnExit()
    {
        ResourceManager.DestoryGameObject(_skill.GetEntityInstanceId());
        isReady = false;
    }

    private void OnSkillFinishCall()
    {
        stateMachine.Enter(SKILLSTATE.End.ToString());
    }

    public override void OnUpdate(float delta)
    {
        if (!isReady) return;
        passedTime += delta;

        UpdateTranslation();
        UpdateFollow();
        UpdateRotation();
        UpdateScale(); 
    }

    private void UpdateTranslation()
    {
        EntityBase attacker = EntityManager.Instance.GetEntity(_attackerMonoId);
        EntityBase beAttacker = EntityManager.Instance.GetEntity(_beAttackerMonoId);
        if (attacker == null || beAttacker == null)
        {
            LoopOrFinish();
            return;
        }

        if (!_config.isUseTranslation) return;
        if(_config.translationConfig.type == TrackConfig.TranslationConfig.TranslationType.Point)
        {
            float distanceRate = _config.translationConfig.curve.Evaluate(passedTime * _config.translationConfig.mulSpeed);
            _skill.SetRotationZ(CommonUtil.CalAngle(attacker.GetPos(), beAttacker.GetPos()));
            _skill.SetPos(attacker.GetPos() * (1-distanceRate) + distanceRate * beAttacker.GetPos());
            if (distanceRate > 0.9999f) LoopOrFinish();
        }
        else
        {
            // ������ ����û���ٶ�
            if(_config.translationConfig.mulSpeed == 0)
            {
                if(Vector2.Distance(attacker.GetPos(),beAttacker.GetPos()) > ((TowerBase)attacker).GetBuffAttackDistance())
                {
                    LoopOrFinish();
                }
                else
                {
                    _skill.SetPos(beAttacker.GetPos());
                }
            }
            // ׷�� TODO
            else
            {
                float angle = _skill.GetRotation().z / 180 * Mathf.PI;
                Vector2 forward = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 dragForward = (beAttacker.GetPos() - _skill.GetPos()).normalized;
                float strength = _config.translationConfig.curve.Evaluate(passedTime);
                dragForward *= strength;
                Vector2 newForward = (forward + dragForward).normalized;
                _skill.SetRotationZ(CommonUtil.GetAngle(newForward,Vector2.up));
                _skill.SetPos(_skill.GetPos() + newForward * _config.translationConfig.mulSpeed);
                // ��ײ֮������
            }
        }
        
    }
    private void UpdateFollow()
    {

    }
    private void UpdateRotation()
    {
        if (!_config.isUseRotation) return;
        float rot = _config.rotationConfig.curve.Evaluate(passedTime * _config.rotationConfig.mulSpeed);
        _skill.SetRotation(new Vector3(0, 0, -rot * 360));
        if (rot > 0.9999f) LoopOrFinish();
    }
    private void UpdateScale()
    {
        if (!_config.isUseScale) return;
        float scale = _config.scaleConfig.curve.Evaluate(passedTime * _config.scaleConfig.mulSpeed);
        _skill.SetScale(scale);
        if (scale > 0.9999f) LoopOrFinish();
    }

    private void LoopOrFinish()
    {
        EntityBase attack = EntityManager.Instance.GetEntity(_attackerMonoId);
        EntityBase beAttack = EntityManager.Instance.GetEntity(_beAttackerMonoId);
        if (attack == null || beAttack == null) 
        {
            OnSkillFinishCall();
            return;
        }

        if (_config.IsLoop)
        {
            EntityBase entity = EntityManager.Instance.SearchOneEntity(attack.GetPos(), ((TowerBase)attack).GetBuffAttackDistance());
            if (entity == null)
            {
                OnSkillFinishCall();
            }
        }
        else
        {
            OnSkillFinishCall();
        }
    }
}