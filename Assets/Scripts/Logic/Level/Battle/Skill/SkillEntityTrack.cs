
using UnityEngine;

public class SkillTrackEntity : EntityBase
{
    private int _owerEntityMonoId;

    //public SkillTrackEntity() : base() { }

    public void SetOwerEntity(int owerEntityMonoId)
    {
        this._owerEntityMonoId = owerEntityMonoId;
    }

    public int GetOwerEntityMonoId()
    {
        return _owerEntityMonoId;
    }

    public void EnterBattle(Vector2 pos)
    {
        ShowSelf(pos);
        base.EnterBattle();
    }
}