using System.Collections;
using System.Collections.Generic;
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

    private List<Card> _cards;

    public Card AddCard(CardSO cardSO) {
        _cards ??= new();

        GameObject newCard = Instantiate(cardSO.Prefab);
        
        Vector3 newCardPos = new Vector3(_cards.Count * _nextCardOffsetX, _cards.Count * _nextCardOffsetY, _cards.Count * _nextCardOffsetZ);
        
        newCard.transform.SetParent(_cardsParentTransform, false);
        newCard.transform.localPosition = newCardPos;

        Card newCardScript = newCard.GetComponent<Card>();

        _cards.Add(newCardScript);

        return newCardScript;
    }
    public void Clear() {
        UpdateScore(0);

        // Remove cards.
        if (_cards != null) {
            foreach (var card in _cards) {
                Destroy(card.gameObject);
            }
        }

        _cards?.Clear();
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
}
