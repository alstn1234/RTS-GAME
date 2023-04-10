using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class UIChange : MonoBehaviourPunCallbacks
{
    public GameObject UpgradeUI;
    public Transform ViewPort;
    Transform UnitUI;

    //업그레이드 비용 
    private int AttackCost, ArmorCost, TowerCost;

    //타워 업그레이드 메테리얼
    public Material lv2, lv3, lv4;
    private int tower_lv = 1, attack_lv = 1, armor_lv = 1;

    // 업그레이드 레벨 텍스트
    public TMP_Text tower, attack, armor;

    // 업그레이드 가격 텍스트
    public TMP_Text tower_price, attack_price, armor_price;

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
        //공격 업그레이드
        if (AttackCost > crystal.instance.now_crystal)
        {
            UnitSpawn.instance.PriceLack();
            return;
        }
        attack.text = (++attack_lv).ToString();
        crystal.instance.now_crystal -= AttackCost;
        UnitSpawn.instance.PlusAttack += 1;
        photonView.RPC("attack_up", RpcTarget.Others);
        // 가격 증가
        AttackCost += 10;
        attack_price.text = AttackCost.ToString();


    }
    public void ArmorUpgrade()
    {
        //아머 업그레이드
        if (ArmorCost > crystal.instance.now_crystal)
        {
            UnitSpawn.instance.PriceLack();
            return;
        }
        armor.text = (++armor_lv).ToString();
        crystal.instance.now_crystal -= ArmorCost;
        UnitSpawn.instance.PlusArmor += 1;
        photonView.RPC("armor_up", RpcTarget.Others);
        //가격 증가
        ArmorCost += 10;
        armor_price.text = ArmorCost.ToString();


    }
    public void TowerUpgrade()
    {
        //타워 업그레이드
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
        else if (tower_lv == 2)
        {
            mytower.GetComponent<MeshRenderer>().material = lv3;
        }
        else
        {
            mytower.GetComponent<MeshRenderer>().material = lv4;
            UpgradeUI.transform.GetChild(1).GetComponent<Button>().interactable = false;
        }
        mytower.GetComponent<Tower>().upgrade_lv++;
        mytower.GetComponent<Hp_Bar>().maxHp += 50;
        mytower.GetComponent<Hp_Bar>().currenthp += 50;
        tower.text = (++tower_lv).ToString();
        photonView.RPC("tower_up", RpcTarget.Others, tower_lv);
        //가격 증가
        TowerCost += 10;
        tower_price.text = TowerCost.ToString();

    }

    [PunRPC]
    public void tower_up(int n)
    {
        GameObject othertower = GameObject.Find("Other WallTower");
        if (n == 2)
            othertower.GetComponent<MeshRenderer>().material = lv2;
        else if (n == 3)
            othertower.GetComponent<MeshRenderer>().material = lv3;
        else
            othertower.GetComponent<MeshRenderer>().material = lv4;
        othertower.GetComponent<Tower>().upgrade_lv = n;
        othertower.GetComponent<Hp_Bar>().maxHp += 50;
        othertower.GetComponent<Hp_Bar>().currenthp += 50;
    }

    [PunRPC]
    public void armor_up()
    {
        EnemySpawn.instance.enemy_PlusArmor += 1;
    }

    [PunRPC]
    public void attack_up()
    {
        EnemySpawn.instance.enemy_PlusAttack += 1;
    }
}