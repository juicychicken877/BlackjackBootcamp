using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandVisual : MonoBehaviour
{
    [SerializeField] private Transform _cardsParentTransform;
    [Tooltip("A circle or UI element that indicates that this hand is now currently played")]
    [SerializeField] private SpriteRenderer _activeCircle;
    [SerializeField] private Color _activeHandColor;
    [SerializeField] private Color _inactiveHandColor;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private float _nextCardOffsetX = 1.5f;
    [SerializeField] private float _nextCardOffsetY = 2.5f;
    [SerializeField] private float _nextCardOffsetZ = -0.05f;

    public Card AddCard(CardSO cardSO, int cardCount) {
        // Calculate new position.
        Vector3 newCardPos = new((cardCount-1) * _nextCardOffsetX, (cardCount - 1) * _nextCardOffsetY, (cardCount - 1) * _nextCardOffsetZ);

        // Create new card.
        GameObject newCard = Instantiate(cardSO.Prefab);

        newCard.transform.SetParent(_cardsParentTransform, false);
        newCard.transform.localPosition = newCardPos;

        Card newCardScript = newCard.GetComponent<Card>();

        return newCardScript;
    }

    public void RemoveCard(CardSO cardSO) {
        // Find a card that has a reference to the CardSO in PlayerHand
        Card card = GetCards().Find(card => card.CardSO.GetInstanceID() == cardSO.GetInstanceID());

        if (card != null) {
            Destroy(card.gameObject);
        }
    }

    public void Clear() {
        UpdateScore(0);

        foreach (Card card in _cardsParentTransform.GetComponentsInChildren<Card>()) {
            Destroy(card.gameObject);
        }
    }

    public void SetHandActive(bool active) {
        _activeCircle.color = active ? _activeHandColor : _inactiveHandColor;
    }
    public void UpdateScore(int score) {
        if (score == 0) {
            _scoreText.text = "";
        } else {
            _scoreText.text = score.ToString();
        }
    }

    private List<Card> GetCards() {
        return _cardsParentTransform.GetComponentsInChildren<Card>().ToList();
    }
}
