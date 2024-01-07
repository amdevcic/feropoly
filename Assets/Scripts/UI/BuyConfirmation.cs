using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyConfirmation : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    private Tile _tileRef;
    private Pawn _playerRef;
    private int _buyFunction;

    public void SetUp(string text, Tile tile, Pawn player, int buyFunction)
    {
        _text.text = text;
        _tileRef = tile;
        _playerRef = player;
        _buyFunction = buyFunction;
    }


    public void onClickConfirm()
    {
        switch(_buyFunction){
            case 0:
                ((Property)_tileRef).BuyProperty(_playerRef);
                UIManager.Instance.HidePropertyBuyConfirmation();
                break;
            case 1:
                ((Property)_tileRef).BuyHouse(_playerRef);
                UIManager.Instance.HidePropertyBuyConfirmation();
                break;
            case 2:
                ((Railroad)_tileRef).BuyRailroad(_playerRef);
                UIManager.Instance.HidePropertyBuyConfirmation();
                break;
            case 3:
                ((Utilities)_tileRef).BuyUtilities(_playerRef);
                UIManager.Instance.HidePropertyBuyConfirmation();
                break;
        }
    }

    public void onClickCancel()
    {
        UIManager.Instance.HidePropertyBuyConfirmation();
    }
}
