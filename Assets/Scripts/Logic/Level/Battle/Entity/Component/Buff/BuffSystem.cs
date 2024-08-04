using System.Collections.Generic;

public enum BuffReplaceType
{
    Replace = 0,    // 替换
    Add,            // 增加
    OneReplace,     // 同一攻击者 替换
}

public enum BuffType
{
    Health = 0,
    Speed,
    ChangeSkill
}

public abstract class BuffBase
{
    public int Id;
    public BuffReplaceType buffReplaceType = BuffReplaceType.Replace;
    public int attackEntityId;
    public int beAttackEntityId;
    public BuffType type = BuffType.Health;

    public BuffBase(int id, int attackEntityId, int beAttackEntityId, BuffReplaceType buffReplaceType, BuffType type)
    {
        Id = id;
        this.buffReplaceType = buffReplaceType;
        this.attackEntityId = attackEntityId;
        this.beAttackEntityId = beAttackEntityId;
        this.type = type;
    }

    public abstract bool IsEnd();
    public abstract float GetEffectValue();
    public abstract void Update(float delta);
    public abstract void Start();
    public abstract void End();
}

public class BuffSystem
{
    public static global::BuffSystem Instance;

    // BuffId BuffBase
    private Dictionary<int, List<BuffBase>> _buffMap = new Dictionary<int, List<BuffBase>>();
    private List<BuffBase> _clearBuffs = new List<BuffBase>();

    public BuffSystem()
    {
        Instance = this;
    }

    public void AddBuff(int attackEntityId , int beAttackEntityId ,int buffId , int buffLevel)
    {
        BuffBase buff = null;
        BuffConfig.Buff buffConfig = GameApp.Instance.BuffConfig.GetBuffConfig(buffId);
        if(buffConfig.type == BuffType.Health)
        {
            buff = new HealthBuff(attackEntityId, beAttackEntityId, buffConfig, buffLevel);
        }
        else if(buffConfig.type == BuffType.Speed)
        {
            buff = new SpeedBuff(attackEntityId, beAttackEntityId, buffConfig, buffLevel);
        }
        if(buff != null)
        {
            AddBuff(buff);
        }
    }

    public void AddBuff(BuffBase buff)
    {
        // 1 替换
        int inEntityId = -1;
        int inIndex = -1;
        foreach (int entityId in _buffMap.Keys)
        {
            bool isBreak = false; ;
            List<BuffBase> bfs = _buffMap[entityId];
            for (int i = 0; i < bfs.Count; i++)
            {
                BuffBase bf = bfs[i];
                if (bf.Id == buff.Id)
                {
                    if (bf.buffReplaceType == BuffReplaceType.Replace)
                    {
                        inEntityId = entityId;
                        inIndex = i;
                        isBreak = true;
                        break;
                    }

                    if (bf.buffReplaceType == BuffReplaceType.OneReplace && bf.attackEntityId == buff.attackEntityId && bf.beAttackEntityId == buff.beAttackEntityId)
                    {
                        inEntityId = entityId;
                        inIndex = i;
                        isBreak = true;
                        break;
                    }
                }
            }
            if (isBreak) break;
        }
        if (inEntityId != -1)
        {
            _buffMap[inEntityId][inIndex] = buff;
            return;
        }

        // 2 增加
        if (!_buffMap.ContainsKey(buff.beAttackEntityId))
        {
            _buffMap[buff.beAttackEntityId] = new List<BuffBase>();
        }
        _buffMap[buff.beAttackEntityId].Add(buff);
        buff.Start();
    }

    public void Update(float delta)
    {
        float addHealth = 0;
        float addSpeed = 0;
        float skillId = 0;
        foreach (int beAttackEntityId in _buffMap.Keys)
        {
            _clearBuffs.Clear();
            List<BuffBase> bfs = _buffMap[beAttackEntityId];
            for(int i = 0; i < bfs.Count; i++)
            {
                BuffBase bf = bfs[i];

                if (!bf.IsEnd())
                {
                    bf.Update(delta);

                    if (bf.type == BuffType.Health) addHealth += bf.GetEffectValue();
                    if (bf.type == BuffType.Speed) addSpeed += bf.GetEffectValue();
                    if (bf.type == BuffType.ChangeSkill) skillId = bf.GetEffectValue();
                }
                else
                {
                    _clearBuffs.Add(bf);
                }
            }
            EntityBase entity = EntityManager.Instance.GetEntity(beAttackEntityId);
            if (entity != null)
            {
                entity.AddBuffValue(BuffType.Health, addHealth);
                entity.AddBuffValue(BuffType.Speed, addSpeed);
                entity.AddBuffValue(BuffType.ChangeSkill, skillId);
            }

            for(int i = bfs.Count - 1; i >= 0; i--)
            {
                for(int j = 0; j < _clearBuffs.Count; j++)
                {
                    if (bfs.Contains(_clearBuffs[j]))
                    {
                        bfs.Remove(_clearBuffs[j]);
                    }
                }
            }
        }
    }
}