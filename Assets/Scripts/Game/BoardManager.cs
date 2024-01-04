using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    const int MAX_TILES = 40;
    Tile[] tiles;
    public GameObject tileContainer;

    private void Start() {
        tiles = tileContainer.GetComponentsInChildren<Tile>();
        Debug.Log(tiles[4].Name);
    }
}
