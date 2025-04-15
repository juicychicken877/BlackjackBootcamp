using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DealerHandVisual : MonoBehaviour
{
    [SerializeField] private Transform _cardsParentTransform;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [Tooltip("Offsets for card object placing")]
    [SerializeField] private Vector3 _firstCardPos;
    [SerializeField] private Vector3 _nextCardOffset;

    public List<Card> CardObjs {
        get => _cardsParentTransform.GetComponentsInChildren<Card>().ToList();
    }

    public async Task<Card> AddCard(CardSO cardSO, int newCardIndex, bool hidden) {
        // Create new hand.
        GameObject newCard = Instantiate(cardSO.Prefab);
        Card newCardScript = newCard.GetComponent<Card>();

        // Calculate new position.
        Vector3 newCardLocalPos = new(newCardIndex * _nextCardOffset.x + _firstCardPos.x, newCardIndex * _nextCardOffset.y + _firstCardPos.y, newCardIndex * _nextCardOffset.z + _firstCardPos.z);

        if (hidden) newCardScript.Visuals.Turn(CardVisual.ImagePos.Back);

        // Set parent.
        newCard.transform.SetParent(_cardsParentTransform, false);

        // Start animating.
        Vector3 newCardWorldPos = _cardsParentTransform.TransformPoint(newCardLocalPos);
        await newCardScript.Visuals.Animator.Animate(Shoe.Instance.transform.position, newCardWorldPos, newCard.transform);

        return newCardScript;
    }

    public void Clear() {
        UpdateScore(0, false);

        foreach (Card card in _cardsParentTransform.GetComponentsInChildren<Card>()) {
            Destroy(card.gameObject);
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
