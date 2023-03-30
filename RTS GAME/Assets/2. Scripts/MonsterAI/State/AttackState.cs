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
        //상태 진입시 상대 오브젝트를 향해 회전
        Units.transform.rotation = Quaternion.LookRotation(Units.target.position - Units.transform.position);
        //상대의 AIUnit 스크립트를 target_AiUnit에 받아옴
        target_AIUnit = Units.target.GetComponent<AIUnit>();
    }
    public void Stay()
    {
        //Attack 애니메이션을 실행
        Units.PlayAnimation(AIUnit.State.Attack);

        //1.3초마다 상대 피가 까이고 오브젝트가 날아가게 함
        time += Time.deltaTime;
        if (time >= 1.3f)
        {
            time = 0;
            target_AIUnit.hp_bar.GetAttack(Units.attack, target_AIUnit.armor);
            if (Units.is_range_long)
            {
                //투사체 오브젝트 풀링 받아와서 쏘기
            }
        }

        //적이 죽거나 적이 거리가 멀어지면 walkstate로 변환
        float distance = Vector3.Distance(Units.transform.position, Units.target.position);
        if (distance > Units.attackRange + 1 || target_AIUnit.isDead == true)
        {
            Units.States = AIUnit.State.Walk;
            Units.target = Units.DefaultTarget;
            Units.unit.target = Units.DefaultTarget;
        }
    }

    public void Exit()
    {
    }
}