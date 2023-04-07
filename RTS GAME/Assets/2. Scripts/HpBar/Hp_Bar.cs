using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp_Bar : MonoBehaviour
{
    public bool isdead = false; // ���� ���θ� �Ǵ��ϴ� bool ����
    public Slider hpbar; // ü�¹� UI
    public float maxHp; // �ִ� ü��
    public float currenthp; // ���� ü��

    void Update()
    {
        transform.position = this.transform.position + new Vector3(0, 0, 0); // ü�¹� ��ġ ����
        if (hpbar != null) // ü�¹� UI�� ������ ��
            hpbar.value = currenthp; // ü�¹� �� ����
        if (currenthp <= 0) // ���� ü���� 0 ������ ��
        {
            isdead = true; // ���� ���θ� false���� true�� ����
        }
    }

    public void GetAttack(float damage, float Armor)
    {
        float realdamage = damage - Armor; // ���������� ������ �� ���� ������ ���
        if (realdamage <= 0) // ���� �������� 0 ������ ��
        {
            realdamage = 1; // �ּ� �������� 1�� ����
        }
        currenthp -= realdamage; // ���� ü�¿��� ��������ŭ ����
    }

    public void SetMaxHP()
    {
        hpbar.maxValue = maxHp; // ü�¹��� �ִ밪 ����
    }
}