using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private HandVisual _visuals;

    private List<CardSO> _cards;
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

        _score += cardSO.Value;

        _cards.Add(cardSO);
        _visuals.AddCard(cardSO);
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
