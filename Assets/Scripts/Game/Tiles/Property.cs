using UnityEngine;
using Photon.Pun;

public enum PropertyColor {
    BROWN, LIGHTBLUE, MAGENTA, ORANGE, RED, YELLOW, GREEN, BLUE
}

[System.Serializable]
public class Property : Tile
{
    public int BuyPrice;
    public int[] RentPrices;
    public int rentIndex;
    public PropertyColor Color;
    public Pawn Owner {get; set;}

    public override void OnActivate(Pawn player) {
        if (!this.Owner) {
            // player buys property
            player.PayMoney(this.BuyPrice, null);
            player.ChangeOwner(this._photonView.ViewID);
        } else if(this.Owner != player) {
            // player pays owner
            Debug.Log($"player pays price {this.RentPrices[rentIndex]} to other");
            player.PayMoney(this.RentPrices[rentIndex], this.Owner);
        }
    }

}