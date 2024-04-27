using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffConfig", menuName = "Config/BuffConfig", order = 1)]
public class BuffConfig : ScriptableObject
{
    [Serializable]
    public class Buff
    {
        public int buffId;
        public BuffType type;
        public List<float> ps;
    }

    public Buff[] buffs;

    public Buff GetBuffConfig(int buffId)
    {
        for (int i = 0; i < buffs.Length; i++)
        {
            if (buffs[i].buffId == buffId)
                return buffs[i];
        }
        return null;
    }
}