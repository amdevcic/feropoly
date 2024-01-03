using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using Photon.Pun;

public class RoomListItem : MonoBehaviour
{
    //sets the text to the name of the room and when clicked, player should join the room
    [SerializeField] TMP_Text _roomName;
    [SerializeField] TMP_Text _roomPlayerCount;
    public RoomInfo _roomInfo;

    public void SetUp(RoomInfo info)
    {
        _roomName.text = info.Name;
        _roomPlayerCount.text = info.PlayerCount + "/8 players";
        _roomInfo = info;
    }

    public void onClick()
    {
        ConnectToServices.Instance.JoinRoom(_roomInfo);
    }
}
