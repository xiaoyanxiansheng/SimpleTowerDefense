
using UnityEngine;

public class SkillTrackEntity : EntityBase
{
    private int _owerEntityMonoId;
    private int _beAttackEntityMonoId;

    public void SetOwerEntity(int owerEntityMonoId)
    {
        this._owerEntityMonoId = owerEntityMonoId;
    }

    public void SetBeAttackEntity(int beAttackMonoId)
    {
        this._beAttackEntityMonoId = beAttackMonoId;
        GetGameObject().GetComponent<EntityBehaviour>().entityBeAttackMonoId = beAttackMonoId;
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