using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private CardVisual _visuals;
    [SerializeField] private CardSO _cardSO;

    public CardVisual Visuals {
        get => _visuals;
    }

    public CardSO CardSO {
        get => _cardSO;
    }
}
