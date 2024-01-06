using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance{ get; private set; }
    const int MAX_TILES = 40;
    const int MAX_PLAYERS = 8;
    const int JAIL_INDEX = 9;
    Tile[] tiles;
    public Transform tileContainer;
    public Tile jailTile;

    private void Start() {
        tiles = tileContainer.GetComponentsInChildren<Tile>();
    }

    public Pawn getPlayerPawn(Player player)
    {
        return (player.TagObject as GameObject).GetComponent<Pawn>();
    }

    public void MovePlayerSpaces(Player player, int spaces) 
    {
        Pawn pawn = getPlayerPawn(player);
        int playerSpace = pawn.Space;
        int newSpace = (playerSpace + spaces) % tiles.Length;
        
        pawn.MoveTo(tiles[newSpace].transform.position, newSpace);

        for (int i=playerSpace+1; i<=newSpace; i++)
        {
            tiles[i].OnPass(pawn);
        }
        tiles[newSpace].OnActivate(pawn);
    }

    public void MovePlayerToJail(Player player)
    {
        Pawn pawn = getPlayerPawn(player);
        pawn.MoveTo(jailTile.transform.position, JAIL_INDEX);
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
