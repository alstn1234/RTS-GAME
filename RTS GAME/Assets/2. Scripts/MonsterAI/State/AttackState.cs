using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AttackState : IState
{
    private AIUnit Units;
    AIUnit target_AIUnit;
    float time = 0;

    public AttackState(AIUnit aIUnit)
    {
        Units = aIUnit;
    }
    public void Enter()
    {
        Units.transform.rotation = Quaternion.LookRotation(Units.target.position - Units.transform.position);
        target_AIUnit = Units.target.GetComponent<AIUnit>();
    }
    public void Stay()
    {
        Units.PlayAnimation(AIUnit.State.Attack);
        time += Time.deltaTime;
        if (time >= 1.3f)
        {
            time = 0;
            target_AIUnit.hp_bar.GetAttack(Units.attack, target_AIUnit.armor);
            if (Units.is_range_long)
            {
                //����ü ������Ʈ Ǯ�� �޾ƿͼ� ���
            }
        }
        float distance = Vector3.Distance(Units.transform.position, Units.target.position);
        if (distance > Units.attackRange + 1 || target_AIUnit.isDead == true)
        {
            Units.States = AIUnit.State.Walk;
            Units.target = Units.DefaultTarget;
            Units.unit.target = Units.DefaultTarget;
        }
        //���� �װų� ���� �Ÿ��� �־����� walkstate�� ��ȯ
    }

    public void Exit()
    {
    }
}