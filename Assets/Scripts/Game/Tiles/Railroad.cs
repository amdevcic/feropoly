using UnityEngine;

public class Railroad : Tile
{
    public int BuyPrice = 200;
    public int RentPrice = 50;
    public Pawn Owner {get; set;}
    public Railroad[] railroadFamily;

    public override void OnActivate(Pawn player) {
        if (!this.Owner) {
            // player buys property
            UIManager.Instance.SetupPropertyBuy($"Želite li kupiti zavod za {BuyPrice} €?", this, player, 2);
        } else if(this.Owner != player) {
            // player pays owner
            int family = 1;

            foreach(Railroad r in railroadFamily) {
                if(r.Owner == this.Owner) family++;
            }
            int rentPrice = this.RentPrice * family;
            Debug.Log($"player pays price {rentPrice} to other");
            player.PayMoney(rentPrice, this.Owner);
        }
        else {
        }
    }

    public void BuyRailroad(Pawn player) {
        player.PayMoney(this.BuyPrice, null);
        player.ChangeRailroadOwner(this._photonView.ViewID);
    }
}
