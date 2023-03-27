using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WalkState : IState
{
    private AIUnit Units;
    bool ishavetarget = false;
    bool isGetAttacked = false;

    public WalkState(AIUnit aIUnit)
    {
        Units = aIUnit;
    }


    public void Enter()
    {
        Units.PlayAnimation(AIUnit.State.Walk);
    }

    public void Stay()
    {
        if (!ishavetarget) FindTarget();
        else if(ishavetarget)
        {
            Units.States = AIUnit.State.Attack;
        }
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }



    public void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(Units.transform.position, Units.seekRange, 1 << LayerMask.NameToLayer("Enemy"));
        if (colliders.Length <= 0) return;
        AIUnit neareastTarget = null; // ���� ����� ���� �����ϱ� ���� ����
        float minDist = Mathf.Infinity; // ���� ����� ������ �Ÿ��� �����ϱ� ���� ����
        for(int i = 0; i < colliders.Length; i++)
        {
            AIUnit temp = colliders[i].GetComponentInParent<AIUnit>();
            if(!temp.isDead && Units.isCreep != temp.isCreep)
            {
                float dist = Vector3.Distance(Units.transform.position, temp.transform.position);
                if(dist < minDist)
                {
                    minDist = dist;
                    neareastTarget = temp;
                }
            }
        }
        if(neareastTarget != null)
        {
            Units.target = neareastTarget.transform;
            ishavetarget = true;
            Units.States = AIUnit.State.Attack;
        }
    }
}
