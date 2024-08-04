/*
    所有敌人显示基类
    能力：
        1 配置
        2 战斗
 */

using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class EnemyBase : EntityBase
{
    protected float defaltHealth;
    protected float defaultMoveSpeed;

    protected float health;
    protected float moveSpeed;

    protected Vector2 startPos;
    protected Vector2 endPos;

    protected override void OnInitEntity()
    {
        base.OnInitEntity();

        // 血量 速度
        defaltHealth = dataConfig.ps[0];
        defaultMoveSpeed = dataConfig.ps[1];

        health = defaltHealth;
        moveSpeed = defaultMoveSpeed;
    }

    public virtual void EnterBattle(Vector2 startPos, Vector2 endPos)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        base.EnterBattle();
    }

    public float GetDefaultHealth()
    {
        return defaltHealth;
    }

    public float GetDefaultMoveSpeed()
    {
        return defaultMoveSpeed;
    }
    public float GetHealth()
    {
        return health;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public override void AddBuffValue(BuffType buffType, float value)
    {
        base.AddBuffValue(buffType, value);

        if(buffType == BuffType.Health)
        {
            health += value;
            if(health < 0) { health = 0;}
            if(health == 0)
            {
                MessageManager.Instance.SendMessage(MessageConst.Battle_EnemyDie, this.GetEntityMonoId());
            }
        }
        else if(buffType == BuffType.Speed)
        {
            moveSpeed = defaultMoveSpeed + value;
        }
    }
}
