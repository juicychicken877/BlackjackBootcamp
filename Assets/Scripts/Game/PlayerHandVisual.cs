using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BlackjackNamespace;
using System.Threading.Tasks;

public class PlayerHandVisual : MonoBehaviour
{
    [SerializeField] private Transform _cardsParentTransform;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [Tooltip("Visuals indicating the state of a hand")]
    [SerializeField] private SpriteRenderer _activeCircle;
    [SerializeField] private Sprite _blueCircle;
    [SerializeField] private Sprite _greenCircle;
    [SerializeField] private Sprite _redCircle;
    [SerializeField] private Sprite _grayCircle;
    [Tooltip("Offsets for card object placing")]
    [SerializeField] private Vector3 _doubleDownCardAddOffset;
    [SerializeField] private Vector3 _firstCardPos;
    [SerializeField] private Vector3 _nextCardOffset;

    public List<Card> CardObjs {
        get => _cardsParentTransform.GetComponentsInChildren<Card>().ToList();
    }

    public async Task AddCard(CardSO cardSO, int newCardIndex, GameAction actionType) {
        // Create new card.
        GameObject newCard = Instantiate(cardSO.Prefab);
        Card newCardScript = newCard.GetComponent<Card>();

        // Calculate new position.
        Vector3 newCardLocalPos = new(newCardIndex * _nextCardOffset.x + _firstCardPos.x, newCardIndex * _nextCardOffset.y + _firstCardPos.y, newCardIndex * _nextCardOffset.z + _firstCardPos.z);
        // If the action is double down, rotate the card 90 degrees in Z and add additional offset
        if (actionType == GameAction.DoubleDown) {
            newCardLocalPos += _doubleDownCardAddOffset;
            newCard.transform.eulerAngles = new Vector3(0, 0, 90);
        }

        // Set parent.
        newCard.transform.SetParent(_cardsParentTransform, false);

        // Start animating.
        if (actionType == GameAction.Hit || actionType == GameAction.DoubleDown || actionType == GameAction.None) {
            Vector3 newCardWorldPos = _cardsParentTransform.TransformPoint(newCardLocalPos);

            await newCardScript.Visuals.Animator.Animate(Shoe.Instance.transform.position, newCardWorldPos, newCard.transform);
        }
    }

    public void RemoveCard(int index) {
        // Find a card that has a reference to the CardSO in PlayerHand
        List<Card> cards = _cardsParentTransform.GetComponentsInChildren<Card>().ToList();

        Card card = cards[index];

        if (card != null) {
            Destroy(card.gameObject);
        }
    }

    public void UpdateVisuals(HandState handState) {
        switch (handState) {
            case HandState.Active: {
                _activeCircle.sprite = _blueCircle;
            } break;
            case HandState.Won: {
                _activeCircle.sprite = _greenCircle;
            } break;
            case HandState.Inactive: {
                _activeCircle.sprite = _grayCircle;
            } break;
            case HandState.Lost: {
                _activeCircle.sprite = _redCircle;
            } break;
        }
    }

    public void UpdateScore(int score, bool isSoft) {
        if (score == 0) {
            _scoreText.text = "";
        } else {
            _scoreText.text = isSoft ? $"{score}/{(score - 10)}" : $"{score}";
        }
    }
}
