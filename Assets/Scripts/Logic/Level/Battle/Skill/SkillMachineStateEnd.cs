using UnityEngine;

public class SkillMachineStateEnd : MachineStateBase
{
    public SkillMachineStateEnd(string name, StateMachine stateMachine) : base(name, stateMachine)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("DoSkill SkillMachineStateEnd");
        stateMachine.Enter(SKILLSTATE.None.ToString());
    }
}
