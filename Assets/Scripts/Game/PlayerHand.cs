using System.Collections.Generic;
using UnityEngine;
using BlackjackNamespace;
using System.Threading.Tasks;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private PlayerHandVisual _visuals;
    [SerializeField] private ChipField _chipField;

    private List<CardSO> _cards;
    public HandState _state;
    private int _score;

    public HandState State {
        get => _state;
    }

    public int Score {
        get => _score;
    }

    public ChipField ChipField {
        get => _chipField;
    }

    public async Task AddCard(CardSO cardSO, GameAction actionType) {
        _cards ??= new();

        _cards.Add(cardSO);
        await _visuals.AddCard(cardSO, _cards.Count-1, actionType);

        HandleScore();
    }

    public void RemoveCard(int index) {
        // Out of bounds
        if (index > _cards.Count - 1) return;

        CardSO cardSO = GetCardSO(index);

        if (cardSO != null) {
            _cards.Remove(cardSO);
            _visuals.RemoveCard(index);

            HandleScore();
        }
    }

    public CardSO GetCardSO(int index) {
        // Out of bounds
        if (index > _cards.Count-1) {
            return null;
        } else {
            return _cards[index];
        }
    }

    public void ChangeState(HandState newState) {
        _state = newState;
        _visuals.UpdateVisuals(newState);
    }

    public bool HasBlackjack() {
        return GetCardSO(0).Value + GetCardSO(1).Value == 21;
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

    public List<GameAction> GetAvaliableGameActions() {
        // Player can always hit and stand.
        List<GameAction> avaliableActions = new() {
            GameAction.Hit,
            GameAction.Stand,
        };

        if (_cards.Count == 2) {
            avaliableActions.Add(GameAction.DoubleDown);

            if (GetCardSO(0).Value == GetCardSO(1).Value) {
                avaliableActions.Add(GameAction.Split);
            }
        }

        return avaliableActions;
    }
}
