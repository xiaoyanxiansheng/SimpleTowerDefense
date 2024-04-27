public enum BuffType
{
    Health,
    Speed
}
public class BuffInterce
{
    public virtual float GetBuffHealth() { return 0;}
    public virtual void SetBuffHealth(float health) { }

    public virtual float GetBuffMoveSpeed() { return 0; }
    public virtual void SetBuffMoveSpeed(float speed) { }

    public virtual int GetBuffSkillId() { return 0; }
    public virtual void SetBuffSkillId(int skillId) { }

    public virtual int GetBuffAttackNum() { return 0; }
    public virtual void SetBuffAttackNum(int num) { }

    public virtual float GetBuffAttackInverval() { return 0; }
    public virtual void SetBuffAttackInverval(float interval) { }

    public virtual float GetBuffAttackDistance() { return 0; }
    public virtual void SetBuffAttackDistance(float distance) { }
}