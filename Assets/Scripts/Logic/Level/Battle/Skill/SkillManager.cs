using UnityEngine;

public enum SKILLSTATE
{
    None = -1,
    Ready,
    Track,
    End,
}

public class SkillManager
{
    public static SkillManager Instance;
    public SkillManager()
    {
        Instance = this;
    }

    public void DoSkill(int levelId)
    {
        // ¼¼ÄÜ×´Ì¬»ú
        StateMachine machine = new StateMachine("DoSkill" + levelId.ToString(), levelId, new Vector2(0, 0), new Vector2(1000, 1000));
        machine.RegisterState(new SkillMachineStateReady(SKILLSTATE.Ready.ToString(), machine));
        machine.RegisterState(new SkillMachineStateTrack(SKILLSTATE.Track.ToString(), machine));
        machine.RegisterState(new SkillMachineStateEnd(SKILLSTATE.End.ToString(), machine));
        StateMachineManager.Instace.RegisterMachine(machine);
        machine.Enter(SKILLSTATE.Ready.ToString());
    }
}