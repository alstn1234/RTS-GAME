using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIChange : MonoBehaviour
{
    public GameObject UpgradeUI;
    public Transform ViewPort;
    Transform UnitUI;

    //���׷��̵� ��� 
    private int AttackCost, ArmorCost, TowerCost;

    //Ÿ�� ���׷��̵� ���׸���
    public Material lv2, lv3, lv4;
    private int tower_lv = 1, attack_lv = 1, armor_lv = 1;

    // ���׷��̵� ���� �ؽ�Ʈ
    public TMP_Text tower, attack, armor; 

    // Start is called before the first frame update
    void Awake()
    {
        AttackCost = 30;
        ArmorCost = 30;
        TowerCost = 30;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnitClick()
    {
        UpgradeUI.SetActive(false);
        ViewPort.Find(UnitSpawn.instance.unit_name + "_Unit(Clone)").gameObject.SetActive(true);
    }
    public void AttackUpgrade()
    {
        //���� ���׷��̵�
        if(AttackCost > crystal.instance.now_crystal)
        {
            UnitSpawn.instance.PriceLack();
            return;
        }
        attack.text = (++attack_lv).ToString();
        crystal.instance.now_crystal -= AttackCost;
        UnitSpawn.instance.PlusAttack += 1;


    }
    public void ArmorUpgrade()
    {
        //�Ƹ� ���׷��̵�
        if (ArmorCost > crystal.instance.now_crystal)
        {
            UnitSpawn.instance.PriceLack();
            return;
        }
        armor.text = (++armor_lv).ToString();
        crystal.instance.now_crystal -= ArmorCost;
        UnitSpawn.instance.PlusArmor += 1;

    }
    public void TowerUpgrade()
    {
        //Ÿ�� ���׷��̵�
        if (tower_lv == 4)
            return;
        if (TowerCost > crystal.instance.now_crystal)
        {
            UnitSpawn.instance.PriceLack();
            return;
        }
        crystal.instance.now_crystal -= TowerCost;
        GameObject mytower = GameObject.Find("Mine WallTower");
        if (tower_lv == 1)
        {
            mytower.GetComponent<MeshRenderer>().material = lv2;
        }
        else if(tower_lv == 2)
        {
            mytower.GetComponent<MeshRenderer>().material = lv3;
        }
        else
        {
            mytower.GetComponent<MeshRenderer>().material = lv4;
            UpgradeUI.transform.GetChild(1).GetComponent<Button>().interactable = false;
        }
        mytower.GetComponent<MyTower>().upgrade_lv++;
        mytower.GetComponent<Hp_Bar>().maxHp += 50;
        mytower.GetComponent<Hp_Bar>().currenthp += 50;
        tower.text = (++tower_lv).ToString();
        
    }

}
