using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class ConnectToServices : MonoBehaviourPunCallbacks
{
    public static ConnectToServices Instance;

    private CanvasManager _canvasManager;
    [SerializeField] TMP_InputField _roomInputField;
    [SerializeField] TMP_InputField _nameInputField;
    [SerializeField] Transform _roomListContent;
    [SerializeField] GameObject _roomListing;
    [SerializeField] Transform _playerListContent;
    [SerializeField] GameObject _playerListing;
    [SerializeField] private GameObject _startButton;


    private void Awake()
    {
        Instance = this;
        _canvasManager = this.GetComponent<CanvasManager>();
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to master...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master.");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

	public override void OnJoinedLobby()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString();
        // Debug.Log("I joined lobby " + PhotonNetwork.NickName);
        _canvasManager.OpenGameMenu();
    }
	
    public void CreateRoom()
    {
        Debug.Log("Creating new room...");
        if(_nameInputField.text != "") PhotonNetwork.NickName = _nameInputField.text;

        if(_roomInputField.text == "") PhotonNetwork.CreateRoom(PhotonNetwork.NickName + "'s room");
        else PhotonNetwork.CreateRoom(_roomInputField.text);
        _canvasManager.OpenGameLoad();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully joined the room.");
        //Metoda uspjesnog stvaranja sobe
        _canvasManager.EnterRoom();

        _playerListContent.DetachChildren();

        foreach(Player player in PhotonNetwork.PlayerList)
        {
            GameObject listing = Instantiate(_playerListing, _playerListContent);
            PlayerListItem playerChild = listing.AddComponent(typeof(PlayerListItem)) as PlayerListItem;
            listing.GetComponent<PlayerListItem>().SetUp(player);
        }

        if(PhotonNetwork.IsMasterClient)
        {
            _startButton.SetActive(true);
        }
        else
        {
            _startButton.SetActive(false);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Error on joining room.");
        _canvasManager.ShowError(message);
    }


    public void LeaveRoom()
    {
        Debug.Log("Leaving room...");
        PhotonNetwork.LeaveRoom();
        _canvasManager.OpenGameLoad();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("User left the room.");
        _canvasManager.OpenGameMenu();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Rooms updated.");

        _roomListContent.DetachChildren();

        foreach (RoomInfo info in roomList)
        {
            if (info.IsOpen)
            {
                GameObject listing = Instantiate(_roomListing, _roomListContent);
                RoomListItem roomChild = listing.AddComponent(typeof(RoomListItem)) as RoomListItem;
                listing.GetComponent<RoomListItem>().SetUp(info);
            }
        }
    }

    public void JoinRoom(RoomInfo info)
    {
        Debug.Log("Joining room...");
        if(_nameInputField.text != "") PhotonNetwork.NickName = _nameInputField.text;
        PhotonNetwork.JoinRoom(info.Name);
        _canvasManager.OpenGameLoad();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player entered room.");
        GameObject listing = Instantiate(_playerListing, _playerListContent);
        PlayerListItem playerChild = listing.AddComponent(typeof(PlayerListItem)) as PlayerListItem;
        listing.GetComponent<PlayerListItem>().SetUp(newPlayer);
        ;
    }

    public void StartGame()
    {
        Debug.Log("Game starting...");
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("Room master changed.");
        if (PhotonNetwork.IsMasterClient)
        {
            _startButton.SetActive(true);
        }
        else
        {
            _startButton.SetActive(false);
        }
    }
}
