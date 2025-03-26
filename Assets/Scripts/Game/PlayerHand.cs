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

    public void AddCard(CardSO cardSO, GameManager.GameAction actionType) {
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

    public void Clear() {
        _cards?.Clear();
        _1ValueAces?.Clear();

        _score = 0;
        _visuals.UpdateScore(_score);
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
        List<GameManager.GameAction> avaliableActions = new();

        if (_cards.Count < 2) {
            return avaliableActions;
        }

        // Player can always hit and stand.
        avaliableActions.Add(GameManager.GameAction.Hit);
        avaliableActions.Add(GameManager.GameAction.Stand);

        // What can a player do with this hand?
        if (_cards.Count == 2) {
            avaliableActions.Add(GameManager.GameAction.DoubleDown);

            if (_cards[0].Value == _cards[1].Value) {
                avaliableActions.Add(GameManager.GameAction.Split);
            }
        }

        return avaliableActions;
    }
}
