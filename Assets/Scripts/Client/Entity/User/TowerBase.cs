using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : EntityBase
{
    private bool _isEnable = false; // 目的是为了晚一帧

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
