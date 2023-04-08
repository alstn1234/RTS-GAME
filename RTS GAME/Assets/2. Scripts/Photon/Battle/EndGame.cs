using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviourPunCallbacks
{
    public Tower Player_Tower;
    public Tower Enemy_Tower;
    public End_Popup end_Popup;
    bool isend = false;

    private void Update()
    {
        if (!isend)
        {

            if (Player_Tower.IsDestroy)
            {
                Surrender(false);
                isend = true;
            }
            else if (Enemy_Tower.IsDestroy)
            {
                Surrender(true);
                isend = true;
            }
        }
    }

    public void Surrender(bool iswin)
    {
        photonView.RPC("LeaveOther", RpcTarget.Others, !iswin);
        PhotonNetwork.LeaveRoom();
        end_Popup.Active_End_Popup(iswin);
        end_Popup.gameObject.SetActive(iswin);
    }

    [PunRPC]
    public void LeaveOther(bool iswin)
    {
        end_Popup.Active_End_Popup(iswin);
        end_Popup.gameObject.SetActive(iswin);
        PhotonNetwork.LeaveRoom();
    }
}
