using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

[RequireComponent(typeof(PhotonView))]
public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _playerPrefab;
    
    [SerializeField]
    private Transform _spawnPoint;
    private PhotonView _photonView;

    [SerializeField]
    private byte playerTurn = 0;

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.PlayerList[playerTurn].IsLocal)
        {
            EndTurn();
        }
        UIManager.Instance.Log($"<color=orange>{PhotonNetwork.LocalPlayer.NickName}</color> je izašao iz igre.");
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    public void BeginTurn(byte turn)
    {
        playerTurn = turn;
        Debug.Log($"Begin turn for player {playerTurn}");
        bool myTurn = PhotonNetwork.PlayerList[playerTurn].IsLocal;

        UIManager.Instance.BeginTurn(myTurn, PhotonNetwork.PlayerList[playerTurn].NickName);
    }

    public void EndTurn() 
    {
        if (++playerTurn >= PhotonNetwork.CurrentRoom.PlayerCount) 
        {
            playerTurn = 0;
        }
        _photonView.RPC(nameof(BeginTurn), RpcTarget.All, new object[] { playerTurn });
    }

    public void Roll()
    {
        int value1 = Random.Range(1, 7);
        int value2 = Random.Range(1, 7);
        UIManager.Instance.RollDice(value1, value2);

        //move player
        BoardManager.Instance.MovePlayerSpaces(PhotonNetwork.PlayerList[playerTurn], value1 + value2);
    }

    private void Awake() 
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene(0);
            return;
        }
        _photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (_playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
        }

        else
        {
            Debug.LogFormat("Instantiating LocalPlayer");
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            GameObject pawn = PhotonNetwork.Instantiate(this._playerPrefab.name, 
                                      _spawnPoint.position + Vector3.back * (PhotonNetwork.LocalPlayer.ActorNumber - 1) * 0.1f, 
                                      Quaternion.identity, 0) as GameObject;

            UIManager.Instance.DisplayPlayer(PhotonNetwork.LocalPlayer);
            UIManager.Instance.Log($"<color=orange>{PhotonNetwork.LocalPlayer.NickName}</color> se pridružio igri.");
            pawn.name = PhotonNetwork.LocalPlayer.NickName;

            CardManager.Instance.Init();
        }

        BeginTurn(0);
    }

}
