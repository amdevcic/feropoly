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
    public GameObject playerPrefab;
    public Transform spawnPoint;
    private PhotonView _photonView;
    Pawn localPawn;

    [SerializeField]
    public byte playerTurn = 1;

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    public void BeginTurn(byte turn)
    {
        playerTurn = turn;
        Debug.Log($"Begin turn for player {playerTurn}");
        bool myTurn = PhotonNetwork.PlayerList[playerTurn].IsLocal;
        if (myTurn) {
            Debug.Log("Your turn");
        }
        UIManager.Instance.BeginTurn(myTurn);
    }

    [PunRPC]
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
        int value = Random.Range(1, 7);
        UIManager.Instance.RollDice(value);

        //move player
        BoardManager.Instance.MovePlayerSpaces(PhotonNetwork.PlayerList[playerTurn], value);
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
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
        }

        else
        {
            Debug.LogFormat("Instantiating LocalPlayer");
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            GameObject pawn = PhotonNetwork.Instantiate(this.playerPrefab.name, 
                                      spawnPoint.position + Vector3.back * (PhotonNetwork.LocalPlayer.ActorNumber - 1) * 0.1f, 
                                      Quaternion.identity, 0) as GameObject;
            pawn.name = PhotonNetwork.LocalPlayer.NickName;
        }

        BeginTurn(0);
    }

}
