using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class Matching : MonoBehaviourPunCallbacks
{
    public bool me_ready = false, opponent_ready = false;

    public TMP_Text me, opponent;
    public Button READY_btn;

    private Color color;

    public TMP_Dropdown dropdown;

    private int Timer = 50;
    public TMP_Text time;
    // Start is called before the first frame update
    void Start()
    {
        color = me.color;
        StartCoroutine("Count");
    }

    // Update is called once per frame
    void Update()
    {
        // ī��Ʈ �ٿ� ǥ��
        time.text = Timer.ToString();
        
        // �غ� �Ϸ� �Ǵ� �غ� �� ǥ��
        if (me_ready)
        {
            color.a = 1f;
            me.color = color;
        }
        else
        {
            color.a = 0f;
            me.color = color;
        }
        if (opponent_ready)
        {
            color.a = 1f;
            opponent.color = color;
        }
        else
        {
            color.a = 0f;
            opponent.color = color;
        }
        
        // ��� �غ� �Ϸ� �� 5�� ī��Ʈ �ٿ�
        if(me_ready && opponent_ready)
        {
            READY_btn.interactable = false;
            if (Timer > 5)
                Timer = 5;
        }
        
        // ���� ����
        if(Timer == 0)
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.LoadLevel("BattleScene");
        }
    }
    IEnumerator Count()
    {
        while(Timer > 0)
        {
            Debug.Log(Timer);
            yield return new WaitForSeconds(1.0f);
            Timer--;
        }

    }

    public void Ready()
    {
        if (me_ready)
        {
            me_ready = false;
            dropdown.interactable = true;
            
        }
        else
        {
            me_ready = true;
            dropdown.interactable = false;
        }
    }
}
