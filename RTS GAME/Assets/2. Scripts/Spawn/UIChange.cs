using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChange : MonoBehaviour
{
    public GameObject UpgradeUI;
    public Transform ViewPort;
    Transform UnitUI;
    // Start is called before the first frame update
    void Awake()
    {

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
    }
    public void ArmorUpgrade()
    {
        //�Ƹ� ���׷��̵�
    }
    public void TowerUpgrade()
    {
        //Ÿ�� ���׷��̵�
    }
}
