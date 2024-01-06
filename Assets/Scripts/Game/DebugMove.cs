using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugMove : MonoBehaviour
{
    [SerializeField] TMP_InputField _indexText;
    [SerializeField] GameManager _manager;

    public void onClick()
    {
        Debug.Log("Moved turn player to field " + _indexText.text);
        _manager.MovePlayerTo(int.Parse(_indexText.text));
    }
}