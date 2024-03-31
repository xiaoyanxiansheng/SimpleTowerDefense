using UnityEngine;
using static SkillConfig;
using static TowerConfig;

public class SkillMachineStateTrack : MachineStateBase
{
    private EntityBase _attacker;
    private EntityBase _beAttacker;
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
        _attacker = (EntityBase)stateMachine.ps[1];
        _beAttacker = (EntityBase)stateMachine.ps[2];
        _config = GameApp.Instance.SkillConfig.GetSkillConfigData(skillId).trackConfig;
        EntityManager.Instance.CreateSkillTrack(skillId, (entity) =>
        {
            isReady = true;

            Vector2 startPos = _attacker.GetPos();
            Vector2 endPos = _beAttacker.GetPos();

            _skill = (SkillTrackEntity)entity;
            _skill.SetOwerEntity(_attacker);
            _skill.SetEntityId(skillId);
            _skill.SetRotationZ(_attacker.GetRotation().z);
            _skill.EnterBattle(startPos);
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
        if (!_config.isUseTranslation) return;
        if(_config.translationConfig.type == TrackConfig.TranslationConfig.TranslationType.Point)
        {
            float distanceRate = _config.translationConfig.curve.Evaluate(passedTime * _config.translationConfig.mulSpeed);
            _skill.SetRotationZ(CommonUtil.CalAngle(_attacker.GetPos(), _beAttacker.GetPos()));
            _skill.SetPos(_attacker.GetPos() * distanceRate + (1 - distanceRate) * _beAttacker.GetPos());
            if (distanceRate > 0.9999f) LoopOrFinish();
        }
        else
        {
            // 跟随性 并且没有速度
            if(_config.translationConfig.mulSpeed == 0)
            {
                if(Vector2.Distance(_attacker.GetPos(),_beAttacker.GetPos()) > ((TowerBase)_attacker).skillConfig.AttackDistance)
                {
                    LoopOrFinish();
                }
                else
                {
                    _skill.SetPos(_beAttacker.GetPos());
                }
            }
            // 追踪 TODO
            else
            {
                float angle = _skill.GetRotation().z / 180 * Mathf.PI;
                Vector2 forward = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 dragForward = (_beAttacker.GetPos() - _skill.GetPos()).normalized;
                float strength = _config.translationConfig.curve.Evaluate(passedTime);
                dragForward *= strength;
                Vector2 newForward = (forward + dragForward).normalized;
                _skill.SetRotationZ(CommonUtil.GetAngle(newForward,Vector2.up));
                _skill.SetPos(_skill.GetPos() + newForward * _config.translationConfig.mulSpeed);
                // 碰撞之后销毁
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
        _skill.SetRotation(new Vector3(0, 0, rot * 360));
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
        if (_config.IsLoop)
        {
            EntityBase entity = EntityManager.Instance.SearchOneEntity(_attacker.GetPos(), ((TowerBase)_attacker).skillConfig.AttackDistance);
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

    public EntityBase GetAttackerEntity()
    {
        return _attacker;
    }
}