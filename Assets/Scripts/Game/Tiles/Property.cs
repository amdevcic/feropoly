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
    public int HousePrice;
    public int rentIndex;
    public PropertyColor Color;
    public Pawn Owner {get; set;}
    public Property[] propertyFamily;

    public override void OnActivate(Pawn player) {
        if (!this.Owner) {
            // player buys property
            UIManager.Instance.SetupPropertyBuy($"Želite li kupiti posjed za {BuyPrice} €?", this, player, 0);
        } else if(this.Owner != player) {
            // player pays owner
            Debug.Log($"player pays price {this.RentPrices[rentIndex]} to other");
            player.PayMoney(this.RentPrices[rentIndex], this.Owner);
        }
        else {
            if(this.rentIndex < 5) {
                bool checkOtherProperties = true;
                foreach(Property p in propertyFamily) {
                    if(p.Owner != this.Owner || p.rentIndex < this.rentIndex) checkOtherProperties = false;
                }
                if(checkOtherProperties) {
                    UIManager.Instance.SetupPropertyBuy($"Želite li kupiti kuću za {HousePrice} €?", this, player, 1);
                }
            }
        }
    }

    public void BuyProperty(Pawn player) {
        player.PayMoney(this.BuyPrice, null);
        player.ChangeOwner(this._photonView.ViewID);
    }

    public void BuyHouse(Pawn player) {
        player.PayMoney(this.HousePrice, null);
        player.BuyHouse(this._photonView.ViewID, rentIndex + 1);
    }

}