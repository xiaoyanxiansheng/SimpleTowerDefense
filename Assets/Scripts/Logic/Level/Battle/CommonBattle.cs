using System.Collections.Generic;
using UnityEngine;
public class CommonBattle : BattleBase
{
    public CommonBattle(GameObject battleRoot, InitData initData) : base(battleRoot, initData)
    {

    }

    public override bool Update(float delta)
    {
        return false;
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