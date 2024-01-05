using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Pawn localPawn;
    
    public static CardManager Instance { get; private set; }

    public void GetMoney(int money)
    {
        localPawn.GetMoney(money);
    }

    public void PayMoney(int money)
    {
        localPawn.PayMoney(money, null);
    }

    public void PayPerHouse(int house, int hotel)
    {

    }

    public void PayAll(int money)
    {

    }

    public void GoToJail()
    {

    }

    public void GetOutOfJailFree()
    {

    }

    public void GoBackSpaces(int spaces)
    {

    }

    public void GoTo(Tile tile)
    {

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

    public void Init() {
        localPawn = (PhotonNetwork.LocalPlayer.TagObject as GameObject).GetComponent<Pawn>();
    }

}
