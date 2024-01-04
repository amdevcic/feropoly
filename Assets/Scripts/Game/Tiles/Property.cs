using UnityEngine;

public enum PropertyColor {
    BROWN, LIGHTBLUE, MAGENTA, ORANGE, RED, YELLOW, GREEN, BLUE
}

[System.Serializable]
public class Property : Tile
{
    public int Price;
    public PropertyColor Color;
    public Pawn owner {get; set;}

    public override void OnActivate(Pawn player) {
        if (!this.owner) {
            // player buys property
            player.money -= this.Price;
            this.owner = player;
        } else {
            // player pays owner
            player.money -= this.Price;
            owner.money += this.Price;
        }
    }
}