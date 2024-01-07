using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : Tile
{
    public int BuyPrice = 150;
    public int[] RentPrices = {4, 10};
    public Pawn Owner {get; set;}
    public Utilities[] utilityFamily;
    public PropertyColor utilityType;
    public string _name;

    public override void OnActivate(Pawn player) {
        if (!this.Owner) {
            // player buys property
            UIManager.Instance.SetupPropertyBuy($"Želite li kupiti {_name} za {BuyPrice} €?", this, player, 3);
            UIManager.Instance.SetupPropertyCard(Name, new int[]{}, 0, utilityType, 2);
        } else if(this.Owner != player) {
            // player pays owner
            int family = 0;

            foreach(Utilities u in utilityFamily) {
                if(u.Owner == this.Owner) family++;
            }
            int rentPrice = this.RentPrices[family] * player.DiceRoll;
            Debug.Log($"player pays price {rentPrice} to other");
            player.PayMoney(rentPrice, this.Owner);
        }
        else {
        }
    }

    public void BuyUtilities(Pawn player) {
        player.PayMoney(this.BuyPrice, null);
        player.ChangeUtilitiesOwner(this._photonView.ViewID);
    }
}
