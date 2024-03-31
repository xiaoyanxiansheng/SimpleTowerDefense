
public class BattleEntityDataBase
{
    public int Health;
    public float MoveSpeed;

    public bool IsDead()
    {
        return (Health <= 0);
    }

    public void AddHealth(int add , bool isDirect = false)
    {
        if(isDirect ) Health = add;
        else Health += add;
        if (Health < 0) Health = 0;
    }

    public void AddMoveSpeed(float add, bool isDirect = false)
    {
        if (isDirect) MoveSpeed = add;
        else MoveSpeed += add;
        if(MoveSpeed < 0) MoveSpeed = 0;
    }

    public float GetHealth()
    {
        return Health;
    }

    public float GetMoveSpeed()
    {
        return MoveSpeed;
    }
}
