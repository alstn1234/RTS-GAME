using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviourPunCallbacks
{
    public End_Popup end_popup;
    public GameObject Setting_Popup;

    // Start is called before the first frame update
    void Start()
    {
        Setting_Popup.SetActive(false);
        this.gameObject.SetActive(false);   
    }

    public void SetActive_Pause_popup()
    {
        this.gameObject.SetActive(true);
    }

    public void SetUnActive_Pause_popup()
    {
        this.gameObject.SetActive(false);
    }

    public void Surrender()
    {
        photonView.RPC("LeaveOther", RpcTarget.Others);
        PhotonNetwork.LeaveRoom();
        end_popup.Active_End_Popup(false);
        end_popup.gameObject.SetActive(true);
    }

    [PunRPC]
    public void LeaveOther()
    {
        end_popup.Active_End_Popup(true);
        end_popup.gameObject.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public void SetActive_Setting_popup()
    {
        Setting_Popup.SetActive(true);
    }

    public void SetUnActive_Setting_popup()
    {
        Setting_Popup.SetActive(false);
    }

    public override void OnLeftRoom()
    {
        this.gameObject.SetActive(false);
    }
}