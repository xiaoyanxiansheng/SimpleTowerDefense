public class HealthBuff : BuffBase
{
    private float _addHealth;
    private float _continueTime;
    private float _inverval;

    private float _passTime;
    private float _level;

    private float _levelRate = 1;

    private int _curAddHealth = 0;

    public HealthBuff(int attackEntityId , int beAttackEntityId, BuffConfig.Buff buff , int level):base(buff.buffId, attackEntityId, beAttackEntityId, buff.replaceType, BuffType.Health)
    {
        // 增加生命 持续时间 间隔时间
        _addHealth = buff.ps[0];
        _continueTime = buff.ps[1];
        _inverval = buff.ps[2];
        _level = level;
    }

    public override void End()
    {
        
    }

    public override float GetEffectValue()
    {
        return _curAddHealth;
    }

    public override bool IsEnd()
    {
        return _continueTime < 0;
    }

    public override void Start()
    {
        _passTime = 0;
    }

    public override void Update(float delta)
    {
        if (_continueTime < 0) 
        {
            return;
        }

        _continueTime -= delta;

        if (_inverval == 0)
        {
            _curAddHealth = (int)(_addHealth * _level * _levelRate);

            MessageManager.Instance.SendMessage(MessageConst.Battle_EntityHurt, beAttackEntityId , _curAddHealth);
            return;
        }

        _passTime += delta;
        if (_passTime >= _inverval)
        {
            _curAddHealth = (int)(_addHealth * _level * _levelRate);
            _passTime = 0;

            MessageManager.Instance.SendMessage(MessageConst.Battle_EntityHurt, beAttackEntityId, _curAddHealth);
        }
    }
}