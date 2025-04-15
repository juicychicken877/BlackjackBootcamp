using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DealerHand : MonoBehaviour
{
    [SerializeField] private DealerHandVisual _visuals;

    private List<CardSO> _cards;
    private Card _hiddenCard;
    private int _score;

    public int Score {
        get => _score;
    }

    public async Task AddCard(CardSO cardSO, bool hidden) {
        _cards ??= new();

        _cards.Add(cardSO);
        Card newCardObj = await _visuals.AddCard(cardSO, _cards.Count-1, hidden);

        if (hidden) _hiddenCard = newCardObj;
        if (!hidden) HandleScore();
    }

    public void Clear() {
        _score = 0;
        _hiddenCard = null;

        _cards?.Clear();
        _visuals.Clear();
    }

    public bool HasBlackjack() {
        return GetCardSO(0).Value + GetCardSO(1).Value == 21;
    }

    public CardSO GetCardSO(int index) {
        // Out of bounds
        if (index > _cards.Count - 1) {
            return null;
        } else {
            return _cards[index];
        }
    }

    public void ShowHiddenCard() {
        _hiddenCard.Visuals.Turn(CardVisual.ImagePos.Front);

        HandleScore();
    }

    private void HandleScore() {
        int newScore = 0;

        // Aces with value 1
        List<Card> aces1 = new();
        List<Card> cardObjs = _visuals.CardObjs;

        foreach (var card in cardObjs) {
            CardSO cardSO = card.CardSO;
            newScore += cardSO.Value;

            if (newScore > 21) {
                List<Card> allAces = cardObjs.FindAll(card => card.CardSO.IsAce);

                foreach (var ace in allAces) {
                    // If ace's value wasnt reduced to 1.
                    if (!aces1.Contains(ace)) {
                        aces1.Add(ace);
                        newScore -= 10;
                        break;
                    }
                }
            }
        }

        _score = newScore;
        bool isSoft = cardObjs.FindAll(card => card.CardSO.IsAce).Count != aces1.Count;
        _visuals.UpdateScore(_score, isSoft);
    }
}
