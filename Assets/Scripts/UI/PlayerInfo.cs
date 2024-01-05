using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public TMP_Text playerName;
    public TMP_Text playerMoney;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Pawn playerPawn = BoardManager.Instance.getPlayerPawn(info.Sender);

        playerName.text = info.Sender.NickName;
        playerMoney.text = playerPawn.Money.ToString() + "€";
        playerPawn.moneyChanged.AddListener(() => playerMoney.text = playerPawn.Money.ToString() + "€");

        transform.SetParent(GameObject.Find("PlayerInfoContainer").transform);
    }
}
