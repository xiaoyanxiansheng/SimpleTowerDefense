using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : EntityBase
{
    private bool _isEnable = false; // Ŀ����Ϊ����һ֡

    private void Update()
    {
        if(_isEnable == true) { return; }

        _isEnable = true;

        OnRealEnable();
    }

    protected virtual void OnRealEnable()
    {

    }
}
