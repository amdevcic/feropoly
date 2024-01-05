using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private const int CHANCE_CARDS = 16;
    private const int CHEST_CARDS = 16;

    public static DeckManager Instance { get; private set; }

    public List<Card> testCards;
    public List<Card> testDiscard;
    private List<int> chanceCards;
    private List<int> communityChestCards;
    private List<int> chanceDiscard;
    private List<int> communityChestDiscard;

    public void DrawTest()
    {
        if (testCards.Count == 0)
        {
            testCards = testDiscard;
            testDiscard.Clear();
        }

        Card card = testCards[0];
        testCards.RemoveAt(0);

        Debug.Log($"Izvukao kartu {card.text}");

        // aktiviraj karticu
        card.pullEvent.Invoke();

        testDiscard.Add(card);
    }

    public void DrawChance()
    {
        if (chanceCards.Count == 0)
        {
            chanceCards = chanceDiscard;
            chanceDiscard.Clear();
            Shuffle(chanceCards);
        }

        int card = chanceCards[0];
        chanceCards.RemoveAt(0);

        // aktiviraj karticu

        chanceDiscard.Add(card);
    }

    public void DrawCommunityChest()
    {
        if (communityChestCards.Count == 0)
        {
            communityChestCards = communityChestDiscard;
            communityChestDiscard.Clear();
            Shuffle(communityChestCards);
        }
        
        int card = communityChestCards[0];
        communityChestCards.RemoveAt(0);

        // aktiviraj karticu

        communityChestDiscard.Add(card);
    }

    private static void Shuffle(List<int> list) {
        var count = list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = Random.Range(i, count);
            var tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }

    private void Start() 
    {
        chanceCards = Enumerable.Range(0, 50).ToList<int>();
        communityChestCards = Enumerable.Range(0, 50).ToList<int>();

        Shuffle(chanceCards);
        Shuffle(communityChestCards);

        chanceDiscard = new List<int>();
        communityChestDiscard = new List<int>();
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
