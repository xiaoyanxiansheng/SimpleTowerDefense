public class HealthBuff : EntityComponentBase
{
    private float _addHealth;
    private float _continueTime;
    private float _inverval;

    private float _passTime;
    private float _level;

    private float _levelRate = 1;

    public HealthBuff(EntityBase entity, BuffConfig.Buff buff , int level) : base(entity)
    {
        // 增加生命 持续时间 间隔时间
        _addHealth = buff.ps[0];
        _continueTime = buff.ps[1];
        _inverval = buff.ps[2];
        _level = level;
    }

    public override void Start()
    {
        _passTime = 0;
    }

    public override void Update(float delta)
    {
        if (_continueTime < 0) 
        {
            entity.RemoveComponent(this);
            return;
        }

        _continueTime -= delta;

        if (_inverval == 0)
        {
            entity.SetBuffHealth(entity.GetBuffHealth() + _addHealth * _level * _levelRate);
            entity.RemoveComponent(this);
            return;
        }

        _passTime += delta;
        if (_passTime >= _inverval)
        {
            entity.SetBuffHealth(entity.GetBuffHealth() + _addHealth * _level * _levelRate);
            _passTime = 0;
        }
    }
}