public class SpeedBuff : BuffBase
{
    private float _addSpeed;
    private float _continueTime;
    private int _level;

    private float _passTime;

    private float _levelRate = 1;

    private float _curAddSpeed;

    public SpeedBuff(int attackEntityId, int beAttackEntityId, BuffConfig.Buff buff , int level) : base(buff.buffId, attackEntityId, beAttackEntityId, buff.replaceType , BuffType.Speed)
    {
        // 速度 持续时间
        _addSpeed = buff.ps[0];
        _continueTime = buff.ps[1];
        _level = level;
    }

    public override void End()
    {
        
    }

    public override float GetEffectValue()
    {
        return _curAddSpeed;
    }

    public override void Start()
    {
        _passTime = 0;
        _curAddSpeed = _addSpeed * _level * _levelRate;
    }

    public override bool IsEnd()
    {
        return _passTime >= _continueTime;
    }

    public override void Update(float delta)
    {
        _passTime += delta;

        if (_passTime >= _continueTime) 
        {
            _curAddSpeed = 0;
        }
    }
}