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
    public int alivePlayers;
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

    public void ExitGame()
    {
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

    [PunRPC]
    public void EndGame(string name)
    {
        UIManager.Instance.SetupWinPanel($"Igrač {name} je pobjedio!");
    }

    public void EndTurn() 
    {
        BoardManager.Instance.getPlayerPawn(PhotonNetwork.PlayerList[playerTurn]).DoublesRolled = 0;
        if(alivePlayers <= 1) {
            Debug.Log("player won");
            int i = 0;
            for(; i < PhotonNetwork.CurrentRoom.PlayerCount; i++) {
                if(!BoardManager.Instance.getPlayerPawn(PhotonNetwork.PlayerList[i]).isBankrupt) break;
            }
            _photonView.RPC(nameof(EndGame), RpcTarget.All, new object[] { BoardManager.Instance.getPlayerPawn(PhotonNetwork.PlayerList[i]).name });
            return;
        }
        while(true) {
            playerTurn = (byte)((playerTurn + 1) % PhotonNetwork.CurrentRoom.PlayerCount);
            if(!BoardManager.Instance.getPlayerPawn(PhotonNetwork.PlayerList[playerTurn]).isBankrupt) break;
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
                    SendActivePlayerToJail();
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

    public void SendActivePlayerToJail()
    {
        Player activePlayer = PhotonNetwork.PlayerList[playerTurn];
        UIManager.Instance.Log($"<color=orange>{activePlayer.NickName}</color> ide na stegovnu.");
        BoardManager.Instance.MovePlayerToJail(activePlayer);
        BoardManager.Instance.getPlayerPawn(activePlayer).InJail = true;
    }

    public void GetPlayerOutOfJail()
    {
        BoardManager.Instance.getPlayerPawn(PhotonNetwork.PlayerList[playerTurn]).InJail = false;
    }

    public void ReducePlayerGetOutOfJailCards()
    {
        BoardManager.Instance.getPlayerPawn(PhotonNetwork.PlayerList[playerTurn]).GetOutOfJailCards--;
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
        alivePlayers = PhotonNetwork.CurrentRoom.PlayerCount;
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
            pawn.GetComponent<Pawn>().SetModel();
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
            localPawn = pawn.GetComponent<Pawn>();

            CardManager.Instance.Init();
        }

        BeginTurn(0);
    }

    public Vector3 GetTilePosition(Tile tile)
    {
        const float offset = 0.05f;
        int xOff = playerTurn % 3 - 1;
        int zOff = playerTurn / 3 - 1;
        Debug.Log($"x={xOff}, z={zOff}");
        return tile.transform.position + new Vector3(xOff*offset, 0, zOff*offset);
        // return tile.transform.position;
    }

}
