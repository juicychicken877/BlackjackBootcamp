using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private CardSO _cardSO;

    public CardSO CardSO {
        get => _cardSO;
    }
}
