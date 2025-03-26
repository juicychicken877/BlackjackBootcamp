using System.Collections.Generic;
using UnityEngine;

public class DealerHand : MonoBehaviour
{
    [SerializeField] private DealerHandVisual _visuals;

    private List<CardSO> _cards;
    private List<CardSO> _1ValueAces;
    private Card _hiddenCard;
    private int _score;

    public int Score {
        get => _score;
    }

    public void AddCard(CardSO cardSO, bool hidden) {
        _cards ??= new();
        _1ValueAces ??= new();

        _cards.Add(cardSO);
        Card newCardObj = _visuals.AddCard(cardSO, hidden);

        if (hidden) _hiddenCard = newCardObj;
        if (!hidden) HandleScore(cardSO.Value);
    }

    public void Clear() {
        _score = 0;
        _visuals.UpdateScore(_score);
        
        _hiddenCard = null;
        
        _cards?.Clear();
        _1ValueAces?.Clear();

        _visuals.Clear();
    }

    public bool HasBlackjack() {
        if (_cards[0] != null && _hiddenCard != null) {
            // If has blackjack
            if (_cards[0].Value + _hiddenCard.CardSO.Value == 21) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    public void ShowHiddenCard() {
        _hiddenCard.Visuals.Turn(CardVisual.ImagePos.Front);

        HandleScore(_hiddenCard.CardSO.Value);
    }

    private void HandleScore(int cardValue) {
        _score += cardValue;

        // If over 21 and is soft
        if (_score > 21) {
            foreach (var cardSO in _cards) {
                if (cardSO.IsAce && _1ValueAces.Find(ace => ace.GetInstanceID() == cardSO.GetInstanceID()) == null) {
                    _score -= 10;
                    _1ValueAces.Add(cardSO);
                    break;
                }
            }
        }

        _visuals.UpdateScore(_score);
    }
}
