using UnityEngine;

public class TowerBase : EntityBase
{
    public virtual void EnterBattle(Vector2 pos)
    {
        base.EnterBattle();
        ShowSelf(pos);
    }
}