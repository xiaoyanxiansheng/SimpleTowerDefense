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
        �����ƶ�·�� 
        1 ���ñ��п���������
        2 ��Ҷ�̬���õķ�����
        3 ��̬�¼��ı�����Ȩ��
     */
    protected void UpdateMoveWays()
    {

    }
}