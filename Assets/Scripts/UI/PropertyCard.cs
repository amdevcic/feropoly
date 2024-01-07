using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PropertyCard : MonoBehaviour
{
    [SerializeField] private Sprite _propertyBrown;
    [SerializeField] private Sprite _propertyLightBlue;
    [SerializeField] private Sprite _propertyMagenta;
    [SerializeField] private Sprite _propertyOrange;
    [SerializeField] private Sprite _propertyRed;
    [SerializeField] private Sprite _propertyYellow;
    [SerializeField] private Sprite _propertyGreen;
    [SerializeField] private Sprite _propertyBlue;
    [SerializeField] private Sprite _utilityWater;
    [SerializeField] private Sprite _utilityElectric;
    [SerializeField] private Image _propertyImage;
    [SerializeField] private Image _railroadImage;
    [SerializeField] private Image _utilityImage;
    [SerializeField] private GameObject _propertyGameObject;
    [SerializeField] private GameObject _railroadGameObject;
    [SerializeField] private GameObject _utilityGameObject;
    [SerializeField] private TMP_Text _propertyName;
    [SerializeField] private TMP_Text _propertyRentPrice;
    [SerializeField] private TMP_Text _propertyBuyPrice;
    [SerializeField] private TMP_Text _railroadName;
    [SerializeField] private TMP_Text _railroadRentPrice;
    [SerializeField] private TMP_Text _utilityName;


    public void SetUp(string name, int[] rentPrices, int housePrice, PropertyColor color, int cardType)
    {
        string temp = "";
        switch(cardType){
            case 0:
                switch(color) {
                    case PropertyColor.BROWN:
                        _propertyImage.sprite = _propertyBrown;
                        break;
                    case PropertyColor.LIGHTBLUE:
                        _propertyImage.sprite = _propertyLightBlue;
                        break;
                    case PropertyColor.MAGENTA:
                        _propertyImage.sprite = _propertyMagenta;
                        break;
                    case PropertyColor.ORANGE:
                        _propertyImage.sprite = _propertyOrange;
                        break;
                    case PropertyColor.RED:
                        _propertyImage.sprite = _propertyRed;
                        break;
                    case PropertyColor.YELLOW:
                        _propertyImage.sprite = _propertyYellow;
                        break;
                    case PropertyColor.GREEN:
                        _propertyImage.sprite = _propertyGreen;
                        break;
                    case PropertyColor.BLUE:
                        _propertyImage.sprite = _propertyBlue;
                        break;
                }
                _propertyName.text = name;
                foreach(int rent in rentPrices){
                    temp += "€" + rent.ToString() + "\n";
                }
                _propertyRentPrice.text = temp;
                _propertyBuyPrice.text = $"Svaka knjiga stoji €{housePrice}\nSvaki laptop €{housePrice}\nplus 4 knjige";
                _propertyGameObject.SetActive(true);
                _railroadGameObject.SetActive(false);
                _utilityGameObject.SetActive(false);
                break;
            case 1:
                _railroadName.text = name;
                foreach(int rent in rentPrices){
                    temp += "€" + rent.ToString() + "\n";
                }
                _railroadRentPrice.text = temp;
                _propertyGameObject.SetActive(false);
                _railroadGameObject.SetActive(true);
                _utilityGameObject.SetActive(false);
                break;
            case 2:
                switch(color) {
                    case PropertyColor.WATER:
                        _utilityImage.sprite = _utilityWater;
                        break;
                    case PropertyColor.ELECTRICITY:
                        _utilityImage.sprite = _utilityElectric;
                        break;
                }
                _utilityName.text = name;
                _propertyGameObject.SetActive(false);
                _railroadGameObject.SetActive(false);
                _utilityGameObject.SetActive(true);
                break;
        }
    }
}
