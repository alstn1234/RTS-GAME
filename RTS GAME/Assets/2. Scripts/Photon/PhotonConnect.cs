using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonConnect : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1.0";

    public Button joingButton;
    public Button CancelButton;
    public GameObject matching_popup;
    // Start is called before the first frame update

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        CancelButton.interactable = false;
        joingButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    // 서버 연결 성공 후
    public override void OnConnectedToMaster()
    {
        joingButton.interactable = true;
    }

    // 서버와 연결이 끊겼을 때
    public override void OnDisconnected(DisconnectCause cause)
    {
        joingButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();

    }
    
    // 매칭 버튼 클릭 함수
    public void matching()
    {
        joingButton.interactable = false;
        //서버에 연결 중이면 룸에 접속 시도
        if (PhotonNetwork.IsConnected)
        {
            matching_popup.SetActive(true);
            PhotonNetwork.JoinRandomRoom(); 
        }
        // 서버에 연결되어 있지 않다면 서버 재연결
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 매칭 취소 버튼 클릭 함수
    public void matching_cancel()
    {
        PhotonNetwork.LeaveRoom();
        matching_popup.SetActive(false);
        CancelButton.interactable = false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("룸 접속 완료");
       CancelButton.interactable = true;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                PhotonNetwork.LoadLevel("MatchingMenu");
            }
        }
    }
}
