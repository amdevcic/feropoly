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

    [SerializeField]
    private List<Card> chanceCards;

    [SerializeField]
    private List<Card> communityChestCards;

    private List<Card> chanceDiscard;
    private List<Card> communityChestDiscard;

    public void DrawChance()
    {
        if (chanceCards.Count == 0)
        {
            chanceCards = chanceDiscard;
            chanceDiscard.Clear();
            Shuffle(chanceCards);
        }

        Card card = chanceCards[0];
        chanceCards.RemoveAt(0);

        UIManager.Instance.Log($"Izvukao kartu: <color=yellow>{card.text}</color>");

        // aktiviraj karticu
        card.pullEvent.Invoke();

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
        
        Card card = communityChestCards[0];
        communityChestCards.RemoveAt(0);

        UIManager.Instance.Log($"Izvukao kartu: <color=yellow>{card.text}</color>");

        // aktiviraj karticu
        card.pullEvent.Invoke();

        communityChestDiscard.Add(card);
    }

    private static void Shuffle(List<Card> list) {
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

        Shuffle(chanceCards);
        Shuffle(communityChestCards);

        chanceDiscard = new List<Card>();
        communityChestDiscard = new List<Card>();
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
