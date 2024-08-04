using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

public class TowerCusion : TowerBase
{
    public List<int> buffs = new List<int>();

    public List<int> GetBuffs()
    {
        return buffs;
    }
}