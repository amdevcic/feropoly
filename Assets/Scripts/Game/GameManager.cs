using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


[RequireComponent(typeof(PhotonView))]
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance{ get; private set; }
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPoint;
    private byte playerTurn = 0;
    private PhotonView _photonView;
    public Pawn localPawn;

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

        UIManager.Instance.BeginTurn(myTurn, PhotonNetwork.PlayerList[playerTurn]);
    }

    public void EndTurn() 
    {
        BoardManager.Instance.getPlayerPawn(PhotonNetwork.PlayerList[playerTurn]).DoublesRolled = 0;
        if (++playerTurn >= PhotonNetwork.CurrentRoom.PlayerCount) 
        {
            playerTurn = 0;
        }
        _photonView.RPC(nameof(BeginTurn), RpcTarget.All, new object[] { playerTurn });
    }

    public void Roll()
    {
        Pawn playerPawn = BoardManager.Instance.getPlayerPawn(PhotonNetwork.PlayerList[playerTurn]);

        int value1 = Random.Range(1, 7);
        int value2 = Random.Range(1, 7);
        playerPawn.DiceRoll = value1 + value2;
        UIManager.Instance.RollDice(value1, value2);

        if (!playerPawn.InJail)
        {
            BoardManager.Instance.MovePlayerSpaces(PhotonNetwork.PlayerList[playerTurn], value1 + value2);

            if (value1 == value2)
            {
                playerPawn.DoublesRolled++;
                if (playerPawn.DoublesRolled >= 3)
                {
                    SendPlayerToJail();
                }

                _photonView.RPC(nameof(BeginTurn), RpcTarget.All, new object[] { playerTurn });
            }
        }
        else if (value1 == value2) 
        {
            GetPlayerOutOfJail();
            BoardManager.Instance.MovePlayerSpaces(PhotonNetwork.PlayerList[playerTurn], value1 + value2);
        }
        
    }

    public void SendPlayerToJail()
    {
        Player activePlayer = PhotonNetwork.PlayerList[playerTurn];
        BoardManager.Instance.MovePlayerToJail(activePlayer);
        BoardManager.Instance.getPlayerPawn(activePlayer).InJail = true;
    }

    public void GetPlayerOutOfJail()
    {
        BoardManager.Instance.getPlayerPawn(PhotonNetwork.PlayerList[playerTurn]).InJail = false;
    }

    public void MovePlayerTo(int index)
    {
        Player activePlayer = PhotonNetwork.PlayerList[playerTurn];
        BoardManager.Instance.MovePlayerTo(activePlayer, index);
    }

    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 

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
            localPawn = pawn.GetComponent<Pawn>();

            CardManager.Instance.Init();
        }

        BeginTurn(0);
    }

}
