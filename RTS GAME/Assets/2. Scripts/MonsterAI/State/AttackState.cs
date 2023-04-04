using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackState : MonoBehaviour,IState
{
    Hp_Bar target_hp_bar;
    private AIUnit Units;
    Tower Target_Tower;
    AIUnit target_AIUnit;
    float time;
    bool istower = false;
    Projectile_Spawn projectile_Spawn;

    public AttackState(AIUnit aIUnit)
    {
        Units = aIUnit;
    }
    public void Enter()
    {
        //�ð�
        time = 0;
        //���� ���Խ� ��� ������Ʈ�� ���� ȸ��
        Units.transform.rotation = Quaternion.LookRotation(Units.target.position - Units.transform.position);
      

        if (Units.is_range_long)
        {
            projectile_Spawn = Units.GetComponent<Projectile_Spawn>();
        }

        if (Units.target.CompareTag(Units.DefaultTarget.tag))
        {
            Target_Tower = Units.target.GetComponent<Tower>();
            target_hp_bar = Target_Tower.GetComponent<Hp_Bar>();
            istower = true;
        }
        else
        {
            //����� AIUnit ��ũ��Ʈ�� target_AiUnit�� �޾ƿ�
            target_AIUnit = Units.target.GetComponent<AIUnit>();
            target_hp_bar = target_AIUnit.GetComponent<Hp_Bar>();
        }
    }
    public void Stay()
    {
        if (!Units.isDead || !target_AIUnit.isDead)
        {
            if (time == 0)
            {
                if(target_AIUnit.isInAir)
                {
                    //Air_Attack �ִϸ��̼� ����
                    Units.PlayAnimation(AIUnit.State.Air_Attack);
                }
                else
                {
                    //Attack �ִϸ��̼��� ����
                    Units.PlayAnimation(AIUnit.State.Attack);
                }
            }

            //1.2�ʸ��� ��� �ǰ� ���̰� ������Ʈ�� ���ư��� ��
            time += Time.deltaTime;

            if (time >= Units.attack_anim_speed)
            {
                if (Units.is_range_long)
                {
                    //����ü ������Ʈ Ǯ�� �޾ƿͼ� ���
                    GameObject projectile = projectile_Spawn.Spawn_Projectile(Units.transform, Units.target.transform);
                    float dist = Vector3.Distance(Units.transform.position, Units.target.transform.position);
                    Projectile projectile_com = projectile.GetComponent<Projectile>();
                    if(istower) projectile_com.Launch(Units.Weapon, Units.target, dist, 0.5f, target_hp_bar,Units.attack, Target_Tower.armor);
                    else projectile_com.Launch(Units.Weapon, Units.target, dist, 0.5f, target_hp_bar, Units.attack, target_AIUnit.armor);
                    projectile_com.projectile_Spawn = projectile_Spawn;
                }
                else
                {
                    if (istower) target_hp_bar.GetAttack(Units.attack, Target_Tower.armor);                
                    else target_hp_bar.GetAttack(Units.attack, target_AIUnit.armor);
                }
                time = 0;

            }        
        }
        //���� �װų� ���� �Ÿ��� �־����� walkstate�� ��ȯ
        if (!istower)
        {
            float distance = Vector3.Distance(Units.transform.position, Units.target.position);
            if (distance > Units.attackRange + 1 || target_AIUnit.isDead == true)
            {
                Units.States = AIUnit.State.Walk;
                Units.target = Units.DefaultTarget;
                Units.unit.target = Units.DefaultTarget;
            }
        }
    }

    public void Exit()
    {
    }
}