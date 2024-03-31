using System.Collections.Generic;
using UnityEngine;
public class CommonBattle : BattleBase
{
    public CommonBattle(GameObject battleRoot, BattleConfig battleConfig) : base(battleRoot, battleConfig)
    {

    }

    public override bool Update(float delta)
    {
        return true;
    }

    /*
        更新移动路径 
        1 配置表中可行走区域
        2 玩家动态放置的防御塔
        3 动态事件改变区域权重
     */
    protected void UpdateMoveWays()
    {

    }
}