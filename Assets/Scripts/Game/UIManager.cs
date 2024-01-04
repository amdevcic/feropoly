using Photon.Pun;
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
    public TMP_Text _playerName;
    public TMP_Text _playerMoney;

    public void HideAll()
    {
        _dicePanel.SetActive(false);
    }

    public void BeginTurn(bool myTurn)
    {
        HideAll();

        if (myTurn)
        {
            _dicePanel.SetActive(true);
            
            _rollButton.interactable = true;
            _endTurnButton.interactable = false;
        }
    }

    public void RollDice(int value)
    {
        _diceText.text = value.ToString();
        _rollButton.interactable = false;
        _endTurnButton.interactable = true;
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
        _playerName.text = PhotonNetwork.LocalPlayer.NickName;
    }
}
