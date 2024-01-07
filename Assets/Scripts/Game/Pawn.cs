using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using System.Collections;
using System;
using System.Linq;


[RequireComponent(typeof(PhotonView))]
public class Pawn : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    const int STARTING_MONEY = 1500;
    public int Money { get; private set; }
    public byte DoublesRolled { get; set; }
    public byte GetOutOfJailCards { get; set; }
    // public int Space { get; set; }
    public Tile tile;
    public bool InJail { get; set; }
    public PhotonView PhotonView { get; private set; }
    public UnityEvent moneyChanged;
    public int DiceRoll { get; set; }
    private System.Collections.Generic.Queue<Tuple<Vector3, Vector3>> animationQueue;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject;
        name = info.Sender.NickName;
    }

    private void Awake() 
    {
        PhotonView = GetComponent<PhotonView>();
        tile = Go.Instance;
        Money = STARTING_MONEY;
        animationQueue = new System.Collections.Generic.Queue<Tuple<Vector3, Vector3>>();
    }

    public void MoveTo(int space)
    {
        PhotonView.RPC(nameof(MoveToRPC), RpcTarget.All, new object[] { space });
    }

    [PunRPC]
    public void MoveToRPC(int space)
    {
        Debug.Log($"Player moves to space {space}");
        
        animationQueue.Enqueue(new Tuple<Vector3, Vector3>(tile.transform.position, BoardManager.Instance.Tiles[space].transform.position));
        this.tile = BoardManager.Instance.Tiles[space];

        if (animationQueue.Count == 1)
        {
            StartCoroutine(AnimateMovement(transform.position, tile.transform.position));
        }
    }

    public IEnumerator AnimateMovement(Vector3 start, Vector3 end)
    {
        float y = transform.position.y;
        for (float i=0.0f; i<1.0f; i+=Time.deltaTime * 5)
        {
            Vector3 pos = Vector3.Lerp(start, end, i);
            transform.position = new Vector3(pos.x, y+Mathf.Sin(i*Mathf.PI)*0.1f, pos.z);
            yield return null;
        }
        
        transform.position = end;
        animationQueue.Dequeue();

        if (animationQueue.Count > 0)
        {
            var tuple = animationQueue.First();
            StartCoroutine(AnimateMovement(tuple.Item1, tuple.Item2));
        }
        else
        {
            BoardManager.Instance.EndMovement(this);
        }
        yield return null;
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
        Debug.Log($"tile {tileViewId} changed to owner {this.PhotonView.ViewID}");
    }

        public void BuyHouse(int tileViewId, int rentIndex)
    {
        photonView.RPC(nameof(BuyHouseRPC), RpcTarget.All, new object[] { tileViewId, rentIndex });
    }

    [PunRPC]
    private void BuyHouseRPC(int tileViewId, int rentIndex) 
    {
        GameObject tile = PhotonView.Find(tileViewId).gameObject;
        tile.GetComponent<Property>().rentIndex = rentIndex;
        tile.GetComponent<Property>().ShowHouses(rentIndex);
        Debug.Log($"player {this.PhotonView.ViewID} bought house on tile {tileViewId}");
    }

    public void ChangeRailroadOwner(int tileViewId)
    {
        photonView.RPC(nameof(ChangeRailroadOwnerRPC), RpcTarget.All, new object[] { tileViewId });
    }

    [PunRPC]
    private void ChangeRailroadOwnerRPC(int tileViewId) 
    {
        GameObject tile = PhotonView.Find(tileViewId).gameObject;
        tile.GetComponent<Railroad>().Owner = this;
        Debug.Log($"tile {tileViewId} changed to owner {this.PhotonView.ViewID}");
    }

    public void ChangeUtilitiesOwner(int tileViewId)
    {
        photonView.RPC(nameof(ChangeUtilitiesOwnerRPC), RpcTarget.All, new object[] { tileViewId });
    }

    [PunRPC]
    private void ChangeUtilitiesOwnerRPC(int tileViewId) 
    {
        GameObject tile = PhotonView.Find(tileViewId).gameObject;
        tile.GetComponent<Utilities>().Owner = this;
        Debug.Log($"tile {tileViewId} changed to owner {this.PhotonView.ViewID}");
    }
}
