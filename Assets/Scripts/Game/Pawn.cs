using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;


[RequireComponent(typeof(PhotonView))]
public class Pawn : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    const int STARTING_MONEY = 1500;
    public int Money { get; private set; }
    public byte DoublesRolled { get; set; }
    public int Space { get; set; }
    private PhotonView _photonView;
    public UnityEvent moneyChanged;
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject;
        name = info.Sender.NickName;
    }

    private void Awake() 
    {
        _photonView = GetComponent<PhotonView>();
        Space = 0;
        Money = STARTING_MONEY;
    }

    public void MoveTo(Vector3 dest, int space)
    {
        _photonView.RPC(nameof(MoveToRPC), RpcTarget.All, new object[] { dest, space });
    }

    [PunRPC]
    public void MoveToRPC(Vector3 dest, int space)
    {
        Debug.Log($"player moves to space {space}");
        transform.position = dest;
        this.Space = space;
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
        }
        UIManager.Instance.Log($"<color=orange>{name}</color> je platio <color=green>{moneyToPay}€</color>" 
                + (other ? $" igraču <color=orange>{other.name}</color>" : "") + ".");
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
        Debug.Log($"tile {tileViewId} changed to owner {this._photonView.ViewID}");
    }
}
