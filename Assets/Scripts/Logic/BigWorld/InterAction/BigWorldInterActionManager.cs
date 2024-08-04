using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWorldInterActionObjectManager
{
    public static BigWorldInterActionObjectManager Instance;

    public static void Create()
    {
        if (Instance != null) return;

        new BigWorldInterActionObjectManager();
    }

    public BigWorldInterActionObjectManager()
    {
        Instance = this;
    }

    private List<BigWorldInterActionItem> _configs;


    public void AddConfigs(List<BigWorldInterActionItem> configs)
    {
        foreach (var config in configs)
        {
            if(!_configs.Contains(config)) _configs.Add(config);
        }
    }
}
