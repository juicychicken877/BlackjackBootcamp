using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BlackjackNamespace;

public class PlayerHandVisual : MonoBehaviour
{
    [SerializeField] private Transform _cardsParentTransform;
    [Tooltip("A circle or UI element that indicates that this hand is now currently played")]
    [SerializeField] private SpriteRenderer _activeCircle;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _inactiveSprite;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Vector3 _doubleDownCardAddOffset;
    [SerializeField] private float _nextCardOffsetX = 1.5f;
    [SerializeField] private float _nextCardOffsetY = 2.5f;
    [SerializeField] private float _nextCardOffsetZ = -0.05f;

    public Card AddCard(CardSO cardSO, int cardCount, GameAction actionType) {
        // Calculate new position.
        Vector3 newCardLocalPos = new((cardCount-1) * _nextCardOffsetX, (cardCount - 1) * _nextCardOffsetY, (cardCount - 1) * _nextCardOffsetZ);

        //Vector3 newCardWorldPos = _cardsParentTransform.TransformPoint(newCardLocalPos);

        // Create new card.
        GameObject newCard = Instantiate(cardSO.Prefab);
        Card newCardScript = newCard.GetComponent<Card>();

        // Set parent.
        newCard.transform.SetParent(_cardsParentTransform, false);

        // If the action is double down, rotate the card 90 degrees in Z and add additional offset
        if (actionType == GameAction.DoubleDown) {
            newCardLocalPos += _doubleDownCardAddOffset;
            newCard.transform.eulerAngles = new Vector3(0, 0, 90);
        }

        newCard.transform.localPosition = newCardLocalPos;
        //// Start animating.
        //newCardScript.Visuals.Animator.StartDrawingAnimation(Shoe.Instance.transform.position, newCardWorldPos, newCard.transform.position);

        return newCardScript;
    }

    public void RemoveCard(CardSO cardSO) {
        // Find a card that has a reference to the CardSO in PlayerHand
        List<Card> cards = _cardsParentTransform.GetComponentsInChildren<Card>().ToList();

        Card card = cards.Find(card => card.CardSO.GetInstanceID() == cardSO.GetInstanceID());

        if (card != null) {
            Destroy(card.gameObject);
        }
    }

    public void SetHandActive(bool active) {
        _activeCircle.sprite = active ? _activeSprite : _inactiveSprite;
    }
    public void UpdateScore(int score) {
        if (score == 0) {
            _scoreText.text = "";
        } else {
            _scoreText.text = score.ToString();
        }
    }
}
