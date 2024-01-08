using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyHouses : MonoBehaviour
{
    private GameObject _book1;
    private GameObject _book2;
    private GameObject _book3;
    private GameObject _book4;
    private GameObject _laptop;

    public void DisplayHouses(int count)
    {
        if(count > 5 || count < 0) return;
        HideAll();
        switch(count) {
            case 4:
                _book4.SetActive(true);
                _book3.SetActive(true);
                _book2.SetActive(true);
                _book1.SetActive(true);
                break;
            case 3:
                _book3.SetActive(true);
                _book2.SetActive(true);
                _book1.SetActive(true);
                break;
            case 2:
                _book2.SetActive(true);
                _book1.SetActive(true);
                break;
            case 1:
                _book1.SetActive(true);
                break;
            case 5:
                _laptop.SetActive(true);
                break;
        }
    }
    private void HideAll()
    {
        _book1.SetActive(false);
        _book2.SetActive(false);
        _book3.SetActive(false);
        _book4.SetActive(false);
        _laptop.SetActive(false);
    }
    private void Awake() 
    { 
        _book1 = transform.Find("books1").gameObject;
        _book2 = transform.Find("books2").gameObject;
        _book3 = transform.Find("books3").gameObject;
        _book4 = transform.Find("books4").gameObject;
        _laptop = transform.Find("laptop").gameObject;
        HideAll();
    }
}
