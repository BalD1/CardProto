using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardManager : Singleton<CardManager>
{
    [SerializeField]
    private Card _cardPrefab = null;

    [SerializeField]
    private Transform _cardParent = null;

    [SerializeField]
    private Card_SO[] _deck = null;

    private Queue<Card_SO> _drawPile = null;
    private List<Card> _hand = null;
    private int _handSize = 5;
    private int _handMaxSize = 7;

    private int currentAliveCardsCount = 0;
    private int lastCardsCount;

    private delegate void D_HandIsEmptied();
    private D_HandIsEmptied _HandIsEmptied;

    private void Start()
    {
        _drawPile = new Queue<Card_SO>();
        _hand = new List<Card>();
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.C))
        //{
        //    SpawnCard(GetCardToSpawn());
        //}
    }

    private Card_SO GetCardToSpawn()
    {
        if(_drawPile.Count == 0) 
            ResetDrawPile();

        return _drawPile.Dequeue();
    }

    private void ResetDrawPile()
    {
        _drawPile.Clear();
        List<Card_SO> toShuffleList = new List<Card_SO>();

        foreach (Card_SO card in _deck)
        {
            toShuffleList.Add(card);
        }
        
        //Shuffle list of cards
        for (int i = 0; i < toShuffleList.Count; i++)
        {
            int randomIndex = Random.Range(0, toShuffleList.Count);

            Card_SO c = toShuffleList[randomIndex];
            toShuffleList[randomIndex] = toShuffleList[i];
            toShuffleList[i] = c;
        }

        //Enqueu to draw pile
        foreach (Card_SO shuffledCard in toShuffleList)
        {
            _drawPile.Enqueue(shuffledCard);
        }
    }

    /// <summary>
    /// Draws the hand size number of card
    /// </summary>
    public void DrawHand()
    {
        for (int i = 0; i < _handSize; i++)
        {
            SpawnCard(GetCardToSpawn());
        }
    }
    public void DrawHandWithLastSize()
    {
        for (int i = 0; i < lastCardsCount; i++)
        {
            SpawnCard(GetCardToSpawn());
        }

        lastCardsCount = 0;
    }

    /// <summary>
    /// Removes all cards in the hand
    /// </summary>
    public void ClearHand()
    {
        Card[] hand = _hand.ToArray();
        foreach (Card card in hand)
        {
            RemoveCard(card);
        }
    }

    /// <summary>
    /// Removes a card from the hand if possible
    /// </summary>
    public void RemoveCard(Card card)
    {
        if(_hand.Contains(card))
        {
            _hand.Remove(card);
            card.StartDestoyCard();
        }
    }

    public void OnCardDestroyed()
    {
        currentAliveCardsCount--;
        if (currentAliveCardsCount <= 0)
        {
            _HandIsEmptied?.Invoke();
            _HandIsEmptied -= DrawHand;
        }
    }

    /// <summary>
    /// Adds a card to the hand if possible
    /// </summary>
    public void SpawnCard(Card_SO data)
    {
        if (currentAliveCardsCount >= _handMaxSize)
            return;

        Card card = Instantiate(_cardPrefab, _cardParent);
        card.InitCard(data);
        _hand.Add(card);
        currentAliveCardsCount++;
    }

    public void ShuffleSingleCard(Card cardToRemove, Card_SO cardToAdd = null)
    {
        if (cardToRemove == null) return;

        RemoveCard(cardToRemove);

        if (cardToAdd == null) cardToAdd = GetCardToSpawn();

        SpawnCard(cardToAdd);
    }

    public void ShuffleCurrentHand()
    {
        lastCardsCount = currentAliveCardsCount;

        ClearHand();

        _HandIsEmptied += DrawHandWithLastSize;
    }

    public void ShuffleAllCards()
    {
        ClearHand();
        _HandIsEmptied += DrawHand;
    }
}