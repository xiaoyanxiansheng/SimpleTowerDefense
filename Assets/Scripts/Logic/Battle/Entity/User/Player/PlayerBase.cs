using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Input ÒÆ¶¯»ùÀà
/// </summary>

[RequireComponent(typeof(PathMoveComponent))]
public class PlayerBase : TowerBase
{
    private List<Vector2> movePath = new List<Vector2>();

    public void Move(Vector2 desPos , Action moveFinish)
    {
        movePath.Clear();
        movePath.Add(GetPosition());
        movePath.Add(desPos);
        GetComponent<PathMoveComponent>().Move(movePath, DataSystem.GetMoveSpeed(), moveFinish);
    }
}
