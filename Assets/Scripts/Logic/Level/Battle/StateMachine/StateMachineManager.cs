using System.Collections.Generic;

public class MachineStateBase
{
    public StateMachine stateMachine;
    private bool _isPause = false;
    public string name;
    public MachineStateBase(string name, StateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    public virtual void OnInit() { }
    public virtual void OnInitData() { }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void UnInit() { }
    public void Pause()
    {
        _isPause = true;
    }
    public void Continue()
    {
        _isPause = false;
    }
    public void Update(float delta)
    {
        if (_isPause) return;
        OnUpdate(delta);
    }
    public virtual void OnUpdate(float delta) { }
}

public class StateMachine
{
    private static int _stateMachineCount = 0;
    public string name;
    public string State = "None";
    public object[] ps;
    private Dictionary<string, MachineStateBase> _machineStates = new Dictionary<string, MachineStateBase>();
    private MachineStateBase _cur;

    public StateMachine(string name , params object[] ps)
    {
        this.name = name + (_stateMachineCount++);
        this.ps = ps;
    }

    public void SetData(params object[] ps)
    {
        this.ps = ps;
    }

    public void RegisterState(MachineStateBase stateBase)
    {
        _machineStates[stateBase.name] = stateBase;
    }

    public Dictionary<string, MachineStateBase> GetStates()
    {
        return _machineStates;
    }

    public void InitData(params object[] ps)
    {
        this.ps = ps;
        List<MachineStateBase> states = new List<MachineStateBase>(_machineStates.Values);
        for (int i = 0; i < states.Count; i++)
        {
            MachineStateBase state = states[i];
            state.OnInitData();
        }
    }

    public void Init()
    {
        List<MachineStateBase> states = new List<MachineStateBase>(_machineStates.Values);
        for (int i = 0; i < states.Count; i++)
        {
            MachineStateBase state = states[i];
            state.OnInit();
        }
    }

    public void UnInit()
    {
        List<MachineStateBase> states = new List<MachineStateBase>(_machineStates.Values);
        for (int i = 0; i < states.Count; i++)
        {
            MachineStateBase state = states[i];
            state.UnInit();
        }
    }

    public void Enter(string state)
    {
        State = state;

        if (_cur != null)
        {
            if (_cur.name == State) return;
            _cur.OnExit();
        }

        if (state == "None")
        {
            return;
        }

        _cur = _machineStates[state];

        _machineStates[state].OnEnter();
    }

    public void Update(float delta)
    {
        if (_cur == null) return;
        _cur.Update(delta);
    }

    public bool IsFinish()
    {
        return State == "None";
    }

}

public class StateMachineManager
{
    private Dictionary<string, StateMachine> _stateMachines = new Dictionary<string, StateMachine>();
    private List<StateMachine> _waitDeletes = new List<StateMachine>();

    public static StateMachineManager Instace;
    public StateMachineManager()
    {
        Instace = this;
    }

    public void AddWaitDeleteMachine(StateMachine machine)
    {
        _waitDeletes.Add(machine);
    }

    public void RegisterMachine(StateMachine machine)
    {
        _stateMachines[machine.name] = machine;
        machine.Init();
    }
    public void UnRegisterMachine(StateMachine machine)
    {
        _stateMachines.Remove(machine.name);
        machine.UnInit();
    }

    public void Update(float delta)
    {
        if (_stateMachines.Count == 0) return;

        foreach (StateMachine machine in _waitDeletes)
        {
            UnRegisterMachine(machine);
        }
        _waitDeletes.Clear();

        foreach (StateMachine machine in _stateMachines.Values)
        {
            machine.Update(delta);
        }
    }
}