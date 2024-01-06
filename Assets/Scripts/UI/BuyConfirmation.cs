using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyConfirmation : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    private Property _propertyRef;
    private Pawn _playerRef;
    private int _buyFunction;

    public void SetUp(string text, Property property, Pawn player, int buyFunction)
    {
        _text.text = text;
        _propertyRef = property;
        _playerRef = player;
        _buyFunction = buyFunction;
    }

    public void onClickConfirm()
    {
        switch(_buyFunction){
            case 0:
                _propertyRef.BuyProperty(_playerRef);
                UIManager.Instance.HidePropertyBuyConfirmation();
                break;
            case 1:
                _propertyRef.BuyHouse(_playerRef);
                UIManager.Instance.HidePropertyBuyConfirmation();
                break;
        }
    }

    public void onClickCancel()
    {
        UIManager.Instance.HidePropertyBuyConfirmation();
    }
}
