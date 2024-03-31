using UnityEngine;

public class SkillMachineStateCounter : MachineStateBase
{
    public SkillMachineStateCounter(string name, StateMachine stateMachine) : base(name, stateMachine)
    {
    }
    public override void OnEnter()
    {
        Debug.Log("DoSkill SkillMachineStateCounter");
        stateMachine.Enter(SKILLSTATE.End.ToString());
    }
}
