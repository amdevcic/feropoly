using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyConfirmation : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    private Property _propertyRef;
    private Pawn _playerRef;

    public void SetUp(string text, Property property, Pawn player)
    {
        _text.text = text;
        _propertyRef = property;
        _playerRef = player;
    }

    public void onClickConfirm()
    {
        _propertyRef.BuyProperty(_playerRef);
        UIManager.Instance.hidePropertyBuyConfirmation();
    }

    public void onClickCancel()
    {
        UIManager.Instance.hidePropertyBuyConfirmation();
    }
}
