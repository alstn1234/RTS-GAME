using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UnitSpawn : MonoBehaviour
{
    public static UnitSpawn instance = null;
    public Hpbar_Control hpbar_Control;
    public int spawn_unit_num = -1;

    public Transform crys;
    public Transform DefaultTarget;
    public Tower MyTower;

    public Transform Canvas_Position;
    public Transform UI_Pos;
    private Transform tf;
    public GameObject SpawnPos;
    public string unit_name = "Human";
    private int[] Unit_Price = { 3, 5, 10, 10, 10, 20, 25, 30 };
    //Hpbar 프리펩
    public GameObject Player_HPbar;
    // 종족별 유닛들 프리팹
    public GameObject[] unitUI = new GameObject[4];
    public GameObject[] hunit = new GameObject[8];
    public GameObject[] sunit = new GameObject[8];
    public GameObject[] uunit = new GameObject[8];
    public GameObject[] wunit = new GameObject[8];

    // 나의 종족 프리팹
    private GameObject MyUnitUI;
    public GameObject[] unit = new GameObject[8];

    // 오브젝트 풀링
    List<Queue<GameObject>> unit_queue = new List<Queue<GameObject>>();

    //업그레이드 수치
    public int PlusAttack = 0, PlusArmor = 0;

    void Awake()
    {
        unit_name = PlayerPrefs.GetString("Player_Tribe");

        instance = this;
        MyTower = MyTower.GetComponent<Tower>();
        hpbar_Control = hpbar_Control.GetComponent<Hpbar_Control>();

        for (int i = 0; i < 8; i++)
        {
            Queue<GameObject> q = new Queue<GameObject>();
            unit_queue.Add(q);
        }

        if (unit_name == "Human")
        {
            unit = hunit;
            MyUnitUI = Instantiate(unitUI[0], Vector2.zero, Quaternion.identity, UI_Pos.Find("Viewport"));
        }
        else if (unit_name == "Sentinel")
        {
            unit = sunit;
            MyUnitUI = Instantiate(unitUI[1], Vector2.zero, Quaternion.identity, UI_Pos.Find("Viewport"));
        }
        else if (unit_name == "Undead")
        {
            unit = uunit;
            MyUnitUI = Instantiate(unitUI[2], Vector2.zero, Quaternion.identity, UI_Pos.Find("Viewport"));
        }
        else
        {
            unit = wunit;
            MyUnitUI = Instantiate(unitUI[3], Vector2.zero, Quaternion.identity, UI_Pos.Find("Viewport"));
        }
        RectTransform UIrt = MyUnitUI.GetComponent<RectTransform>();
        UI_Pos.GetComponent<ScrollRect>().content = UIrt;
        MyUnitUI.transform.position = new Vector3(MyUnitUI.transform.position.x, 100, MyUnitUI.transform.position.z);

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                CreateUnit(j);
            }
            CreateUnit(0);
            CreateUnit(0);
        }
    }
    void Update()
    {
        if (spawn_unit_num != -1)
            SpawnPos.SetActive(true);
        else
            SpawnPos.SetActive(false);
    }
    private GameObject Create_HPbar(GameObject gameObject, GameObject player)
    {
        GameObject Hp_bar = Instantiate(gameObject, Vector2.zero, Quaternion.identity, Canvas_Position);
        Hp_bar.SetActive(false);
        return Hp_bar;
    }
    private void CreateUnit(int n)
    {
        GameObject Unit = Instantiate(unit[n]);
        Unit.SetActive(false);

        if (n != 0)
        {
            AIUnit Units = Unit.GetComponent<AIUnit>();
            Units.Settarget(DefaultTarget);
            Units.MyTower = MyTower;
        }

        GameObject Hpbar = Create_HPbar(Player_HPbar, Unit);

        Hp_Bar hp_bar = Unit.GetComponent<Hp_Bar>();
        hp_bar.hpbar = Hpbar.GetComponent<Slider>();
        hp_bar.SetMaxHP();

        hpbar_Control.obj.Add(Unit.transform);
        hpbar_Control.hp_bar.Add(Hpbar);
        unit_queue[n].Enqueue(Unit);
    }


    public void spawn(int n)
    {
        SpawnClear(n);
        if (spawn_unit_num == n)
            spawn_unit_num = -1;
        else
            spawn_unit_num = n;


    }
    private void SpawnClear(int n)
    {
        tf = UI_Pos.Find("Viewport").Find(unit_name + "_Unit(Clone)");
        for (int i = 1; i < 9; i++)
        {
            if (i == n + 1)
                continue;
            Color color = tf.GetChild(i).gameObject.GetComponent<Image>().color;

            color.a = 1f;
            tf.GetChild(i).gameObject.GetComponent<Image>().color = color;

        }
    }

    public void Die(GameObject unit_object, int n)
    {
        // 몬스터 사망 시 큐에 추가
        unit_object.SetActive(false);
        unit_queue[n].Enqueue(unit_object);

    }

    public void SpawnUnit(int n)
    {
        if (Unit_Price[spawn_unit_num] > crystal.instance.now_crystal)
        {
            //구매 가격 부족

            PriceLack();

            return;
        }
        crystal.instance.now_crystal -= Unit_Price[spawn_unit_num];
        if (spawn_unit_num == -1)
            return;
        if (unit_queue[spawn_unit_num].Count == 0)
            CreateUnit(spawn_unit_num);
        Vector3 vec = new Vector3(-9 + n * 6.25f, 0, -23);
        GameObject unit = unit_queue[spawn_unit_num].Dequeue();
        if (spawn_unit_num != 0)
        {
            unit.GetComponent<AIUnit>().armor += PlusArmor;
            unit.GetComponent<AIUnit>().attack += PlusAttack;
        }
        if(unit_queue[spawn_unit_num].Count == 1)
        {
            if(spawn_unit_num == 0)
            {
                tf = UI_Pos.Find("Viewport").Find(unit_name + "_Unit(Clone)");
                tf.GetChild(1).gameObject.SetActive(false);
              
            }
        }
        Hp_Bar hp_bar = unit.GetComponent<Hp_Bar>();
        hp_bar.hpbar.gameObject.SetActive(true);
        unit.transform.position = vec;
        unit.SetActive(true);
        EnemySpawn.instance.spawn(n, spawn_unit_num);
    }
    //구매가격 부족
    public void PriceLack()
    {
        Color color = crystal.instance.tmp.color;
        crystal.instance.tmp.color = Color.red;
    }

    public void ChangeUI()
    {
        Transform tf = UI_Pos.Find("Viewport");
        tf.Find(unit_name + "_Unit(Clone)").gameObject.SetActive(false);
        tf.Find("Upgrade").gameObject.SetActive(true);
        spawn_unit_num = -1;
        SpawnPos.SetActive(false);
        SpawnClear(-1);
    }
}