using UnityEngine;

public class Chance : Tile
{
    public override void OnActivate(Pawn player)
    {
        DeckManager.Instance.DrawTest();
    }
}