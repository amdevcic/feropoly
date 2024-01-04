using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pawn : MonoBehaviourPunCallbacks
{
    public int money;
    public int space;
    private PhotonView _photonView;

    private void Awake() 
    {
        _photonView = GetComponent<PhotonView>();
        space = -1;
    }

    public void MoveTo(Vector3 dest, int space)
    {
        _photonView.RPC(nameof(MoveToRPC), RpcTarget.All, new object[] { dest, space });
    }

    [PunRPC]
    public void MoveToRPC(Vector3 dest, int space)
    {
        transform.position = dest;
        this.space = space;
    }
}
