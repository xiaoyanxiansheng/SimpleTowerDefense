/*
    所有敌人显示基类
    能力：
        1 配置
        2 战斗
 */

using UnityEngine;

public abstract class EnemyBase : EntityBase
{
    protected float health;
    protected float moveSpeed;

    protected Vector2 startPos;
    protected Vector2 endPos;

    protected override void OnInitEntity()
    {
        base.OnInitEntity();

        // 血量 速度
        health = dataConfig.ps[0];
        moveSpeed = dataConfig.ps[1];
    }

    public virtual void EnterBattle(Vector2 startPos, Vector2 endPos)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        base.EnterBattle();
    }

    public override float GetBuffMoveSpeed()
    {
        return moveSpeed;
    }

    public override void SetBuffMoveSpeed(float speed)
    {
        if(speed < 0) { speed = 0; }
        moveSpeed = speed;
    }

    public override float GetBuffHealth()
    {
        return health;
    }
    public override void SetBuffHealth(float health)
    {
        this.health = health;

        if (this.health <= 0)
        {
            MessageManager.Instance.SendMessage(MessageConst.Battle_EnemyDie, this.GetEntityMonoId());
        }
    }
}
