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
            UIManager.Instance.SetupPropertyBuy($"Želite li kupiti posjed za {BuyPrice} €?", this, player);
        } else if(this.Owner != player) {
            // player pays owner
            Debug.Log($"player pays price {this.RentPrices[rentIndex]} to other");
            player.PayMoney(this.RentPrices[rentIndex], this.Owner);
        }
    }

    public void BuyProperty(Pawn player) {
        player.PayMoney(this.BuyPrice, null);
        player.ChangeOwner(this._photonView.ViewID);
    }

}