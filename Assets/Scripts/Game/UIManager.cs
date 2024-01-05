using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviourPunCallbacks
{
    public static UIManager Instance{ get; private set; }
    public GameObject _dicePanel;
    public TextMeshProUGUI _diceText;
    public Button _rollButton;
    public Button _endTurnButton;
    public GameObject _playerInfoPrefab;
    public Transform _playerInfoContainer;
    public Transform _eventLogContainer;
    public GameObject _eventLogPrefab;
    public ScrollRect _eventLogScroll;

    public void HideAll()
    {
        _dicePanel.SetActive(false);
    }

    public void BeginTurn(bool myTurn, string playerName)
    {
        HideAll();

        if (myTurn)
        {
            _dicePanel.SetActive(true);
            
            _rollButton.interactable = true;
            _endTurnButton.interactable = false;
            
            Log($"Ti si na redu.", true);

            return;
        }
        
        Log($"<color=orange>{playerName}</color> je na redu.", true);
    }

    public void RollDice(int value)
    {
        _diceText.text = value.ToString();
        _rollButton.interactable = false;
        _endTurnButton.interactable = true;
    }

    public void DisplayPlayer(Player player) 
    {
        GameObject info = PhotonNetwork.Instantiate(_playerInfoPrefab.name, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public void Log(string text, bool local = false)
    {
        if (local) {
            LogRPC(text);
        }
        else {
            photonView.RPC(nameof(LogRPC), RpcTarget.All, new object[] { text });
        }
    }

    [PunRPC]
    private void LogRPC(string text) 
    {
        GameObject log = Instantiate(_eventLogPrefab, _eventLogContainer) as GameObject;
        log.GetComponentInChildren<TMP_Text>().text = text;
        _eventLogScroll.velocity = new Vector2(0, 1000.0f);
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
    }

    private void Start() 
    {
        // _playerName.text = PhotonNetwork.LocalPlayer.NickName;
    }
}
