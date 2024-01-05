using UnityEngine;

public enum CardType { CHANCE, COMMUNITY_CHEST }
public class Chance : Tile
{
    public CardType cardType;
    public override void OnActivate(Pawn player)
    {
        switch (cardType)
        {
            case CardType.CHANCE:
            DeckManager.Instance.DrawChance();
            break; 

            case CardType.COMMUNITY_CHEST:
            DeckManager.Instance.DrawCommunityChest();
            break;

            default:
            break;
        }
    }
}