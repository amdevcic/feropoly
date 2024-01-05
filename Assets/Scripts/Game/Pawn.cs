using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;


public class Pawn : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    const int STARTING_MONEY = 1500;
    public int Money { get; private set; }
    public int space;
    private PhotonView _photonView;
    public UnityEvent moneyChanged;
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject;
    }

    private void Awake() 
    {
        _photonView = GetComponent<PhotonView>();
        space = 0;
        Money = STARTING_MONEY;
    }

    public void MoveTo(Vector3 dest, int space)
    {
        _photonView.RPC(nameof(MoveToRPC), RpcTarget.All, new object[] { dest, space });
    }

    [PunRPC]
    public void MoveToRPC(Vector3 dest, int space)
    {
        Debug.Log($"tile moves to space {space}");
        transform.position = dest;
        this.space = space;
    }

    [PunRPC]
    private void PayRPC(int moneyToPay) 
    {
        Money -= moneyToPay;
        moneyChanged.Invoke();
    }

    public void PayMoney(int moneyToPay, Pawn other) 
    {
        photonView.RPC(nameof(PayRPC), RpcTarget.All, new object[] { moneyToPay });
        // if other is null, pay the bank
        if (other) 
        {
            other.GetMoney(moneyToPay);
            UIManager.Instance.Log($"<color=orange>{name}</color> je platio <color=green>{moneyToPay}€</color>" 
                + (other ? $" igraču <color=orange>{other.name}</color>" : "") + ".");
        }
    }

    [PunRPC]
    void GetMoneyRPC(int paidMoney) 
    {
        Money += paidMoney;
        moneyChanged.Invoke();
    }

    public void GetMoney(int paidMoney)
    {
        photonView.RPC(nameof(GetMoneyRPC), RpcTarget.All, new object[] { paidMoney });
    }

    public void ChangeOwner(int tileViewId)
    {
        photonView.RPC(nameof(ChangeOwnerRPC), RpcTarget.All, new object[] { tileViewId });
    }

    [PunRPC]
    private void ChangeOwnerRPC(int tileViewId) 
    {
        GameObject tile = PhotonView.Find(tileViewId).gameObject;
        tile.GetComponent<Property>().Owner = this;
    }
}
