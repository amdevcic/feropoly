using UnityEngine;

public class Go : Tile
{
    public static Go Instance;
    public override void OnPass(Pawn player) 
    {
        player.GetMoney(200);
    }
    new private void Awake() 
    { 
        base.Awake();
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