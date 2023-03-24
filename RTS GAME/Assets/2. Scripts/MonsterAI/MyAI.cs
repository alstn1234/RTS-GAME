using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAI : MonoBehaviour
{
    public Transform target;
    float attackDelay;

    private SphereCollider attackRange;
    private Unit unit;

    


    void Awake()
    {
        unit = GetComponent<Unit>();
        attackRange = GetComponent<SphereCollider>();    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            //������ ���� ������ ���� ����
            //unit.target = collision.collider.gameObject.transform;
            //ȸ��
            //FaceTarget(collsion.collider.gameObject);
            //A*�˰��� ����
            //unit.StopMethod();
            //���� �ִϸ��̼�
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            //Ÿ���� ���� ����
            //unit.target = ;
            //A* �˰��� �ٽ� ����
            //unit.StartMethod();
            //�ȴ� �ִϸ��̼�
        }
    }

    void FaceTarget(GameObject gameobject)
    {
            transform.localScale = new Vector3(gameobject.transform.position.x - transform.position.x, gameobject.transform.position.y - transform.position.y, 1);
    }
}
