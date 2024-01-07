using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tax : Tile
{
    public int TaxPrice;

    public override void OnActivate(Pawn player) {
        Debug.Log($"player pays tax of {TaxPrice}");
        player.PayMoney(TaxPrice, null);
    }
}
