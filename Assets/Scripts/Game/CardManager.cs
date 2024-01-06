using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private Pawn localPawn;
    
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
        // TODO: dodati kućice i hotele
    }

    public void PayAll(int money)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsLocal) continue;
            Pawn pawn = BoardManager.Instance.getPlayerPawn(player);
            localPawn.PayMoney(money, pawn);
        }
    }

    public void GetFromAll(int money)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsLocal) continue;
            Pawn pawn = BoardManager.Instance.getPlayerPawn(player);
            pawn.PayMoney(money, localPawn);
        }
    }

    public void GoToJail()
    {
        GameManager.Instance.SendPlayerToJail();
    }

    public void GetOutOfJailFree()
    {
        localPawn.GetOutOfJailCards++;
    }

    public void GoBackSpaces(int spaces)
    {
        // TODO
    }

    public void GoTo(Tile tile)
    {
        // TODO
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
