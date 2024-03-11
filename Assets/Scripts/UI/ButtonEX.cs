using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEX : Button
{
    private Action<Vector2> _OnMoveCall;

    public void AddClickListener(UnityAction onClickCall)
    {
        onClick.AddListener(onClickCall);
    }

    public void OnMove(Action<Vector2> OnMoveCall)
    {
        _OnMoveCall = OnMoveCall;
    }

    private void Update()
    {
        if (_OnMoveCall == null) return;
        if (!IsPressed()) return;
        if (CommonUtil.OutCombatArea(Input.mousePosition)) return;

        _OnMoveCall(Input.mousePosition);
    }
}
