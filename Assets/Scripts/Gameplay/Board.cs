using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Romix.Gameplay;
using Romix.Model;
using Romix.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    [SerializeField] private Sprite cardBackSprite;
    [SerializeField] private Transform gridCanvasTransform;
    [SerializeField] private Timer timer;
    [SerializeField] private TextMeshProUGUI unveiledCardsAmountText;
    [SerializeField] private TextMeshProUGUI matchedCardsAmountText; 
    [SerializeField] public Card cardPrefab;
    [SerializeField] private float cardAppearsAfter = 0.66f;
    [SerializeField] private int cardsTotalAmount = 12;
    [SerializeField] private AudioClip rightAudioClip;
    [SerializeField] private AudioClip wrongAudioClip;


    private DifficultyData difficulty;
    private Stack<Card> cardStack;
    private EventSystem eventSystem;
    private List<Card> cards;

    public Action GameWon;
    public Action GameLost;
    public int UnveiledCardsAmount { get; private set; }

    public int MatchedCardsAmount { get; private set; }

    private void Start()
    {
        Input.multiTouchEnabled = false;
        eventSystem = EventSystem.current;
    }
    
    public void StartMatch()
    {
        StartCoroutine(ShowCards());
    }

    public void Setup(DifficultyData difficulty)
    {
        // Cleanup variables
        cards = new List<Card>();
        cardStack = new Stack<Card>();
        UnveiledCardsAmount = 0;
        MatchedCardsAmount = 0;
        this.difficulty = difficulty;

        // Cleanup grid transform
        gridCanvasTransform.ClearChildren();
        
        // Setup time & texts
        timer.SetStartTime(difficulty.TimeAvailable);
        timer.TimeReachedZero -= () => GameLost?.Invoke();
        timer.TimeReachedZero += () => GameLost?.Invoke();
        timer.gameObject.SetActive(false);
        timer.ResetTimer();
        unveiledCardsAmountText.SetText("0");
        matchedCardsAmountText.SetText("0");
        
        PrepareAndShuffleDeck();
    }

    private void PrepareAndShuffleDeck()
    {
        // Prepare deck
        cardsTotalAmount = difficulty.AmountOfPairs * 2;
        for (int i = 0; i < difficulty.AmountOfPairs; i++)
        {
            var cardData = difficulty.AvailableCards[i];
            for (int j = 0; j < 2; j++)
            {
                var card = Instantiate(cardPrefab);
                card.Initialize(cardData.Type, cardBackSprite, cardData.Sprite);
                card.CardRevealed += OnCardRevealed;
                card.Button.interactable = false;
                card.gameObject.SetActive(false);
                cards.Add(card);
            }
        }

        // Shuffle deck
        for (int i = 0; i < cards.Count; i++)
        {
            Card temp;
            int rnd = Random.Range(i, cards.Count);
            temp = cards.ElementAt(rnd);
            cards[rnd] = cards.ElementAt(i);
            cards[i] = temp;
            cards[i].transform.SetParent(gridCanvasTransform);
        }
    }
    
    private void OnCardRevealed(Card card)
    {
        UnveiledCardsAmount++;
        unveiledCardsAmountText.SetText(UnveiledCardsAmount.ToString());
        
        card.Button.interactable = false;

        if (!cardStack.Any())
        {
           cardStack.Push(card);
           card.Button.interactable = false;
        }
        else
        {
            var match = cardStack.Pop();
            if (card.Equals(match))
            {
                MatchedCardsAmount ++;
                matchedCardsAmountText.SetText(MatchedCardsAmount.ToString());
                match.PlayCorrectAnimation();
                card.PlayCorrectAnimation();
                AudioManager.Instance.PlaySound(rightAudioClip);

                if (MatchedCardsAmount * 2 == cardsTotalAmount)
                {
                    GameWon?.Invoke();
                }
            }
            else
            {
                card.PlayWrongAnimation();
                match.PlayWrongAnimation();
                AudioManager.Instance.PlaySound(wrongAudioClip);
                
                StartCoroutine(ToggleShowAndResetCardPair(card, match));
            }
        }

        eventSystem.SetSelectedGameObject(card.gameObject);
    }
    
    private void ResetCardPair(Card first, Card second)
    {
        first.Button.interactable = true;
        second.Button.interactable = true;
    }

    private void MakeCardsInteractable()
    {
        foreach (var card in cards)
        {
            card.Button.interactable = true;
        }
    }
    
    private IEnumerator ToggleShowAndResetCardPair(Card first, Card second)
    {
        yield return new WaitForSeconds(cardAppearsAfter/2);
        first.ToggleShow();
        second.ToggleShow();
        ResetCardPair(first, second);
    }
    
    private IEnumerator ShowCards()
    {
        foreach (var card in cards)
        {
            yield return new WaitForSeconds(cardAppearsAfter);
            card.gameObject.SetActive(true);
        }

        MakeCardsInteractable();
        timer.gameObject.SetActive(true);
        timer.StartTimer();
    }
}
