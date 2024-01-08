using UnityEngine;
using Photon.Pun;

public class Railroad : Tile
{
    public int BuyPrice = 200;
    public int RentPrice = 50;
    public Pawn Owner {get; set;}
    public Railroad[] railroadFamily;
    public PropertyCardList propertyCard;

    public override void OnActivate(Pawn player) {
        if (!this.Owner) {
            // player buys property
            if(player.Money < BuyPrice) return;
            UIManager.Instance.SetupPropertyBuy($"Želite li kupiti zavod za {BuyPrice} €?", this, player, 2);
            UIManager.Instance.SetupPropertyCard(Name, new int[]{RentPrice, RentPrice*2, RentPrice*3, RentPrice*4}, 0, PropertyColor.RED, 1);
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

    new protected void Awake() 
    {
        _photonView = this.GetComponent<PhotonView>();
        propertyCard.SetUp(Name, new int[]{RentPrice, RentPrice*2, RentPrice*3, RentPrice*4}, 0, PropertyColor.RED, 1);
    }

    public void ResetTile(Pawn player) {
        if(player == this.Owner) {
            this.Owner = null;
            this.propertyCard.SetName("");
        }
    }
}
