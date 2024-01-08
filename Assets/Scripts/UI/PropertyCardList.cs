using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PropertyCardList : MonoBehaviour
{
    [SerializeField] PropertyCard _propertyCard;
    [SerializeField] TMP_Text _ownerName;

    public void SetUp(string name, int[] rentPrices, int housePrice, PropertyColor color, int cardType)
    {
        _propertyCard.SetUp(name, rentPrices, housePrice, color, cardType);
    }
    public void SetName(string name)
    {
        _ownerName.text = name;
    }
}
