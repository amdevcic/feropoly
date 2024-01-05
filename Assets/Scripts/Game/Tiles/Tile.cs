using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
[RequireComponent(typeof(PhotonView))]
public abstract class Tile : MonoBehaviourPunCallbacks
{
    public string Name;
    protected PhotonView _photonView;
    virtual public void OnActivate(Pawn player) {}
    virtual public void OnPass(Pawn player) {}
    private void OnDrawGizmos() {
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(0.1f, 0.01f, 0.1f));
    }
    protected void Awake() 
    {
        this.gameObject.AddComponent<PhotonView>();
        _photonView = PhotonView.Get(this);
        //_photonView = this.GetComponent<PhotonView>();
    }
}
