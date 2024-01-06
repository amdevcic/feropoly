using System;
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
    public Tile[] Tiles { get; private set; }
    public Transform tileContainer;
    public Tile jailTile;

    private void Start() {
        Tiles = tileContainer.GetComponentsInChildren<Tile>();
    }

    public Pawn getPlayerPawn(Player player)
    {
        return (player.TagObject as GameObject).GetComponent<Pawn>();
    }

    public void MovePlayerSpaces(Player player, int spaces) 
    {
        Pawn pawn = getPlayerPawn(player);
        int playerSpace = Array.IndexOf(Tiles, pawn.tile);
        Debug.Log(playerSpace);
        
        int sp=playerSpace;
        for (int i=1; i<=spaces; i++)
        {
            sp = (playerSpace+i)%Tiles.Length;
            Tiles[sp].OnPass(pawn);
            pawn.MoveTo(sp);
        }
        // Tiles[sp].OnActivate(pawn);
    }

    public void EndMovement(Pawn pawn)
    {
        pawn.tile.OnActivate(pawn);
    }

    public void MovePlayerToJail(Player player)
    {
        Pawn pawn = getPlayerPawn(player);
        // pawn.MoveTo(JAIL_INDEX, jailTile.transform.position);
        pawn.MoveTo(JAIL_INDEX);
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
