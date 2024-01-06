using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviourPunCallbacks
{
    public static UIManager Instance{ get; private set; }

    [Header("Dice rolling")]
    public GameObject _rollPanel;
    public GameObject _dicePanel;
    public TextMeshProUGUI _diceText1;
    public TextMeshProUGUI _diceText2;
    public Button _rollButton;
    public Button _endTurnButton;

    [Header("Player info panel")]
    public GameObject _playerInfoPrefab;
    public Transform _playerInfoContainer;

    [Header("Event log")]
    public Transform _eventLogContainer;
    public GameObject _eventLogPrefab;
    public ScrollRect _eventLogScroll;

    [Header("Property purchase")]
    public BuyConfirmation _buyConfirmation;
    public GameObject _buyConfirmationPanel;

    public void HideAll()
    {
        _rollPanel.SetActive(false);
        _dicePanel.SetActive(false);
        hidePropertyBuyConfirmation();
    }

    public void BeginTurn(bool myTurn, string playerName)
    {
        HideAll();

        if (myTurn)
        {
            _rollPanel.SetActive(true);
            _dicePanel.SetActive(false);
            
            _rollButton.interactable = true;
            _endTurnButton.interactable = false;
            
            Log($"Ti si na redu.", true);

            return;
        }
        
        Log($"<color=orange>{playerName}</color> je na redu.", true);
    }

    public void RollDice(int value1, int value2)
    {
        _dicePanel.SetActive(true);
        _diceText1.text = value1.ToString();
        _diceText2.text = value2.ToString();

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

    public void SetupPropertyBuy(string text, Property property, Pawn player) 
    {
        _buyConfirmationPanel.SetActive(true);
        _buyConfirmation.SetUp(text, property, player);
    }

    public void hidePropertyBuyConfirmation()
    {
        _buyConfirmationPanel.SetActive(false);
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
