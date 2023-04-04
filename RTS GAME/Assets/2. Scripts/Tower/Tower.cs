using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Hp_Bar hp_bar;

    public float armor = 2;

    public int Upgrade_Level;

    // Start is called before the first frame update
    void Start()
    {
        hp_bar = GetComponent<Hp_Bar>();
        hp_bar.SetMaxHP();
    }

    // Update is called once per frame
    void Update()
    {
    }
}