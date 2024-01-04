using Photon.Pun;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviourPunCallbacks
{
    public GameObject _dicePanel;
    public TMP_Text _playerName;
    public TMP_Text _playerMoney;

    public void RollDice() 
    {
        HideAll();
        _dicePanel.SetActive(true);
    }

    public void HideAll()
    {
        _dicePanel.SetActive(false);
    }

    private void Start() 
    {
        _playerName.text = PhotonNetwork.LocalPlayer.NickName;
        HideAll();
    }
}
