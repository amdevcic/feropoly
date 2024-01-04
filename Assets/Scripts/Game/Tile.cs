using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropertyColor {
    BROWN, ORANGE, MAGENTA
}

[System.Serializable]
public abstract class Tile
{
    public string Name;
    virtual public void OnActivate(Pawn player) {}
    virtual public void OnPass(Pawn player) {}

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

public class Chance : Tile
{
    public override void OnActivate(Pawn player)
    {
        // pull chance card
    }
}

public class Go : Tile
{
    public override void OnPass(Pawn player) 
    {
        player.money += 200;
    }
}

public class Railroad : Tile
{
    public override void OnActivate(Pawn player) 
    {
        // move player
    }
}
