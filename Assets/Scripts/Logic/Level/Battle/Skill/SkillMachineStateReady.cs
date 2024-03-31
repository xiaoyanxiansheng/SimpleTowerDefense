using UnityEngine;
public class SkillMachineStateReady : MachineStateBase
{
    private float _passTime;
    private SkillConfig.ReadyConfig _config;

    public SkillMachineStateReady(string name, StateMachine stateMachine) : base(name, stateMachine)
    {
        
    }

    public override void OnInitData()
    {
        int skillId = (int)stateMachine.ps[0];
        _config = GameApp.Instance.SkillConfig.GetSkillConfigData(skillId).readyConfig;
    }

    public override void OnEnter()
    {
        // Debug.Log("DoSkill SkillMachineStateReady");
        _passTime = 0;
    }

    public override void OnUpdate(float delta)
    {
        if (_passTime >= _config.continueTime) 
        {
            stateMachine.Enter(SKILLSTATE.Track.ToString());
            return;
        };
        _passTime += delta;
    }
}
