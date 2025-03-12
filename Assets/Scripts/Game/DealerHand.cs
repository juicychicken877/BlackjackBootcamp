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

    public Card HiddenCard {
        get => _hiddenCard;
    }

    public void AddCard(CardSO cardSO, bool hidden) {
        _cards ??= new();

        HandleScore(cardSO.Value);

        _cards.Add(cardSO);

        Card newCardObj = _visuals.AddCard(cardSO, hidden);

        if (hidden) _hiddenCard = newCardObj;
    }

    public void Clear() {
        _score = 0;
        _cards?.Clear();
        _hiddenCard = null;

        if (_1ValueAces != null) {
            _1ValueAces.Clear();
        } else {
            _1ValueAces = new();
        }

        _visuals.Clear();
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

        // _visuals.UpdateScore(_score);
    }
}
