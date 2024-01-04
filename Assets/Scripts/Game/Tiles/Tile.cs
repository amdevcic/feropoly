using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Tile : MonoBehaviour
{
    public string Name;
    virtual public void OnActivate(Pawn player) {}
    virtual public void OnPass(Pawn player) {}
    private void OnDrawGizmos() {
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(0.1f, 0.01f, 0.1f));
    }
}
