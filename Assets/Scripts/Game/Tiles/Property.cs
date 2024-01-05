using UnityEngine;

public enum PropertyColor {
    BROWN, LIGHTBLUE, MAGENTA, ORANGE, RED, YELLOW, GREEN, BLUE
}

[System.Serializable]
public class Property : Tile
{
    public int Price;
    public PropertyColor Color;
    public Pawn Owner {get; set;}

    public override void OnActivate(Pawn player) {
        if (!this.Owner) {
            // player buys property
            player.PayMoney(this.Price, null);
            this.Owner = player;
        } else {
            // player pays owner
            player.PayMoney(this.Price, this.Owner);
        }
    }
}