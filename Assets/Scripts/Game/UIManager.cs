using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(PhotonView))]
public class UIManager : MonoBehaviourPunCallbacks
{
    public static UIManager Instance{ get; private set; }

    [Header("Dice rolling")]
    [SerializeField] private GameObject _rollPanel;
    [SerializeField] private GameObject _dicePanel;
    [SerializeField] private Image _dice1img;
    [SerializeField] private Image _dice2img;
    [SerializeField] private Button _rollButton;
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private Sprite[] _diceSprites;

    [Header("Player info panel")]
    [SerializeField] private GameObject _playerInfoPrefab;
    [SerializeField] private Transform _playerInfoContainer;

    [Header("Event log")]
    [SerializeField] private Transform _eventLogContainer;
    [SerializeField] private GameObject _eventLogPrefab;
    [SerializeField] private ScrollRect _eventLogScroll;

    [Header("Property purchase")]
    [SerializeField] private BuyConfirmation _buyConfirmation;
    [SerializeField] private GameObject _buyConfirmationPanel;
    [SerializeField] private PropertyCard _propertyCard;
    [SerializeField] private GameObject _propertyCardPanel;

    [Header("Jail")]
    [SerializeField] private GameObject _jailPanel;
    [SerializeField] private Button _getOutOfJailButton;

    [Header("Debug")]
    [SerializeField] private GameObject _debugPanel;

    public void HideAll()
    {
        _rollPanel.SetActive(false);
        _jailPanel.SetActive(false);
        HidePropertyBuyConfirmation();
    }

    public void BeginTurn(bool myTurn, Player player)
    {
        HideAll();

        if (myTurn)
        {
            if (GameManager.Instance.localPawn.InJail)
            {
                _endTurnButton.interactable = false;
                _jailPanel.SetActive(true);

                _getOutOfJailButton.interactable = GameManager.Instance.localPawn.GetOutOfJailCards > 0;
                _getOutOfJailButton.GetComponentInChildren<TMP_Text>().text = $"Iskoristi ispričnicu ({GameManager.Instance.localPawn.GetOutOfJailCards})";
            }
            else
            {
                SetUpRoll();
            }            
            Log($"Ti si na redu.", true);

            return;
        }
        
        Log($"<color=orange>{player.NickName}</color> je na redu.", true);
    }

    public void SetUpRoll()
    {
        HideAll();
        _rollPanel.SetActive(true);
            
        _rollButton.interactable = true;
        _endTurnButton.interactable = false;
    }

    public void RollDice(int value1, int value2)
    {
        _dicePanel.SetActive(true);
        _dice1img.sprite = _diceSprites[value1-1];
        _dice2img.sprite = _diceSprites[value2-1];

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

    public void SetupPropertyBuy(string text, Tile tile, Pawn player, int buyFunction) 
    {
        _endTurnButton.interactable = false;
        _buyConfirmationPanel.SetActive(true);
        _buyConfirmation.SetUp(text, tile, player, buyFunction);
    }

    public void SetupPropertyCard(string name, int[] rentPrices, int housePrice, PropertyColor color, int cardType) 
    {
        _propertyCard.SetUp(name, rentPrices, housePrice, color, cardType);
        _propertyCardPanel.SetActive(true);
    }

    public void HidePropertyBuyConfirmation()
    {
        _endTurnButton.interactable = true;
        _buyConfirmationPanel.SetActive(false);
        _propertyCardPanel.SetActive(false);
    }

    public void HideDebugMove()
    {
       _debugPanel.SetActive(false);
    }

    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            HideDebugMove();
            Destroy(this);
        } 
        else 
        {
            Instance = this; 
        }
        
    }
}
