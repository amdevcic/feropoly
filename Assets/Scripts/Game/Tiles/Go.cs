using UnityEngine;

public class Go : Tile
{
    public override void OnPass(Pawn player) 
    {
        player.GetMoney(200);
    }
}