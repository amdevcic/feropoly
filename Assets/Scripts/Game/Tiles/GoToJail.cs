using UnityEngine;

public class GoToJail : Tile
{
    public override void OnActivate(Pawn player) {
        Debug.Log("player goes to jail");
        GameManager.Instance.SendActivePlayerToJail();
    }
}
