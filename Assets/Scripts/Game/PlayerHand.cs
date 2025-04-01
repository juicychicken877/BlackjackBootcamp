using System.Collections.Generic;
using UnityEngine;
using BlackjackNamespace;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private PlayerHandVisual _visuals;
    [SerializeField] private ChipField _chipField;

    private List<CardSO> _cards;
    // A list of aces which value has been decreased to 1
    private List<CardSO> _1ValueAces;

    private int _index;
    private int _score;

    public int Index {
        get => _index;
        set => _index = value;
    }

    public int Score {
        get => _score;
    }

    public PlayerHandVisual Visuals {
        get => _visuals;
    }

    public ChipField ChipField {
        get => _chipField;
    }

    public void AddCard(CardSO cardSO, GameAction actionType) {
        _cards ??= new();
        _1ValueAces ??= new();

        _cards.Add(cardSO);
        _visuals.AddCard(cardSO, _cards.Count, actionType);

        HandleScore(cardSO.Value);
    }

    public void RemoveCard(int index) {
        // Out of bounds
        if (index > _cards.Count - 1) return;

        CardSO cardSO = _cards[index];

        if (cardSO != null) {
            _score -= cardSO.Value;
            _visuals.UpdateScore(_score);
            _cards.Remove(cardSO);
            _visuals.RemoveCard(cardSO);
        }
    }

    public CardSO GetCard(int index) {
        // Out of bounds
        if (index > _cards.Count-1) {
            return null;
        } else {
            return _cards[index];
        }
    }

    public bool HasBlackjack() {
        if (_cards.Count == 2) {
            if ((_cards[0].IsAce && _cards[1].Value == 10) || (_cards[0].Value == 10 && _cards[1].IsAce)) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
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

    public List<GameAction> GetAvaliableGameActions() {
        // Player can always hit and stand.
        List<GameAction> avaliableActions = new() {
            GameAction.Hit,
            GameAction.Stand
        };

        if (_cards.Count == 2) {
            avaliableActions.Add(GameAction.DoubleDown);

            if (_cards[0].Value == _cards[1].Value) {
                avaliableActions.Add(GameAction.Split);
            }
        }

        return avaliableActions;
    }
}
