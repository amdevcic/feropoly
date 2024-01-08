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
    const int JAIL_INDEX = 10;
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

    public void MovePlayerSpaces(Player player, int spaces, bool backwards=false) 
    {
        Pawn pawn = getPlayerPawn(player);
        int playerSpace = Array.IndexOf(Tiles, pawn.tile);
        Debug.Log(playerSpace);
        int backwardsModifier = backwards ? -1 : 1;

        int sp=playerSpace;
        for (int i=1; i<=spaces; i++)
        {
            sp = (playerSpace+i*backwardsModifier)%Tiles.Length;
            Tiles[sp].OnPass(pawn);
            pawn.MoveTo(sp);
        }
    }

    public void EndMovement(Pawn pawn)
    {
        if(pawn.photonView.IsMine)
            pawn.tile.OnActivate(pawn);
    }

    public void MovePlayerToJail(Player player)
    {
        Pawn pawn = getPlayerPawn(player);
        // pawn.MoveTo(JAIL_INDEX, jailTile.transform.position);
        pawn.MoveTo(JAIL_INDEX);
    }

    public void MovePlayerTo(Player player, int index)
    {
        Pawn pawn = getPlayerPawn(player);
        pawn.MoveTo(index);
    }

    public void BankruptPlayer(Pawn player)
    {
        foreach(Tile tile in Tiles) {
            switch (tile)
            {
                case Property property:
                    property.ResetTile(player);
                    break;
                case Railroad railroad:
                    railroad.ResetTile(player);
                    break;
                case Utilities utilities:
                    utilities.ResetTile(player);
                    break;
                default:
                    break;
            }
        }
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
