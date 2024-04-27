public class SpeedBuff : EntityComponentBase
{
    private float _addSpeed;
    private float _continueTime;
    private int _level;

    private float _passTime;

    private float _levelRate = 1;

    public SpeedBuff(EntityBase entity, BuffConfig.Buff buff , int level) : base(entity)
    {
        // 速度 持续时间
        _addSpeed = buff.ps[0];
        _continueTime = buff.ps[1];
        _level = level;
    }

    public override void Start()
    {
        _passTime = 0;

        entity.SetBuffMoveSpeed(entity.GetBuffMoveSpeed() + _addSpeed * _level * _levelRate);
    }

    public override void Update(float delta)
    {
        _passTime += delta;

        if (_passTime >= _continueTime) 
        {
            entity.SetBuffMoveSpeed(entity.GetBuffMoveSpeed() - _addSpeed);
            entity.RemoveComponent(this);
        }
    }
}