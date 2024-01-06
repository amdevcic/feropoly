using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameSelect;
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _loadMenu;
    [SerializeField] private GameObject _createRoomMenu;
    [SerializeField] private GameObject _roomMenu;
    [SerializeField] private GameObject _errorMenu;
    [SerializeField] private TMP_Text _roomName;
    [SerializeField] private TMP_Text _errorText;
    [SerializeField] private GameObject _connectingText;

    private void Awake()
    {
        TurnOff();
        _connectingText.SetActive(true);
    }
    public void OpenGameMenu()
    {
        TurnOff();
        _gameMenu.SetActive(true);
    }
    public void OpenGameSelect()
    {
        TurnOff();
        _gameSelect.SetActive(true);
    }
    public void OpenGameLoad()
    {
        TurnOff();
        _loadMenu.SetActive(true);
    }
    public void CreateRoom()
    {
        TurnOff();
        _createRoomMenu.SetActive(true);
    }
    public void EnterRoom()
    {
        TurnOff();
        _roomName.text = PhotonNetwork.CurrentRoom.Name;
        _roomMenu.SetActive(true);
    }
    public void ShowError(string errmsg)
    {
        TurnOff();
        _errorText.text = "Creation of room failed, error message : " + errmsg;
        _errorMenu.SetActive(true);
    }
    private void TurnOff()
    {
        _gameSelect.SetActive(false);
        _gameMenu.SetActive(false);
        _createRoomMenu.SetActive(false);
        _loadMenu.SetActive(false);
        _roomMenu.SetActive(false);
        _errorMenu.SetActive(false);
        _connectingText.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
