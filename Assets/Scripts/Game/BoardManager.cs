using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance{ get; private set; }
    const int MAX_TILES = 40;
    const int MAX_PLAYERS = 8;
    Tile[] tiles;
    Pawn[] players;
    public GameObject tileContainer;

    private void Start() {
        tiles = tileContainer.GetComponentsInChildren<Tile>();
        Debug.Log(tiles[4].Name);
    }

    public void UpdatePlayers(Pawn[] pawns)
    {
        players = pawns;
    }

    public void MovePlayerSpaces(int player, int spaces) 
    {
        int playerSpace = players[player].space;
        Debug.Log($"player {player} moves {spaces} spaces to {playerSpace + spaces}, total spaces: {tiles.Length}");
        players[player].MoveTo(tiles[playerSpace + spaces].transform.position, playerSpace + spaces);
    }

    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }


}
