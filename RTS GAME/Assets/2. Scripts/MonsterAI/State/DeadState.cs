using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IState
{
    private AIUnit Units;
    float time = 0;

    public DeadState(AIUnit aIUnit)
    {
        Units = aIUnit;
    }

    public void Enter()
    {
        //���� ���� �� Death �ִϸ��̼� ����
        Units.PlayAnimation(AIUnit.State.Death);
        //Ȱ��ȭ false�� �ʱ�ȭ
        Units.isEnable = false;
    }

    public void Exit()
    {
        //idle ���·� ���԰� ���ÿ� unitspawn �Լ����� ������Ʈ Ǯ�� ó��
        UnitSpawn.instance.Die(Units.gameObject, Units.character_num);
    }

    public void Stay()
    {
        //death �ִϸ��̼� ���� �� idle�� ���� ��ȯ
        time += Time.deltaTime;
        if (time >= 1.6f)
        {
            Units.States = AIUnit.State.Idle;
        }

    }
}