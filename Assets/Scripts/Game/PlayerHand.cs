using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private PlayerHandVisual _visuals;

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

    public void AddCard(CardSO cardSO) {
        _cards ??= new();

        HandleScore(cardSO.Value);

        _cards.Add(cardSO);

        _visuals.AddCard(cardSO);
    }

    public void Clear() {
        _score = 0;
        _visuals.UpdateScore(_score);
        _cards?.Clear();

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

        _visuals.UpdateScore(_score);
    }

    public void SetHandActive(bool active) {
        _visuals.SetHandActive(active);
    }

    public List<GameManager.GameAction> GetAvaliableGameActions() {
        List<GameManager.GameAction> avaliableActions = new() {
            GameManager.GameAction.Hit,
            GameManager.GameAction.Stand
        };

        if (_cards.Count < 2) {
            return null;
        }

        // What can a player do with this hand?
        // Split requirements - 2 cards with the same value.
        // Double down requirements - 2 cards.
        if (_cards.Count == 2) {
            avaliableActions.Add(GameManager.GameAction.DoubleDown);

            if (_cards[0].Value == _cards[1].Value) {
                avaliableActions.Add(GameManager.GameAction.Split);
            }
        }

        return avaliableActions;
    }
}
