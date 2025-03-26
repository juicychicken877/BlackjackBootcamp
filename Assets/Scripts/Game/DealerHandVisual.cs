using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DealerHandVisual : MonoBehaviour
{
    [SerializeField] private Transform _cardsParentTransform;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public Card AddCard(CardSO cardSO, bool hidden) {
        GameObject newCard = Instantiate(cardSO.Prefab);
        Card newCardScript = newCard.GetComponent<Card>();

        //Vector3 newCardWorldPos = _cardsParentTransform.TransformPoint(newCard.transform.position);

        if (hidden) newCardScript.Visuals.Turn(CardVisual.ImagePos.Back);

        // Set parent.
        newCard.transform.SetParent(_cardsParentTransform, false);
        
        //// Start animating.
        //newCardScript.Visuals.Animator.StartDrawingAnimation(Shoe.Instance.transform.position, newCardWorldPos, newCard.transform.position);

        return newCardScript;
    }

    public void Clear() {
        UpdateScore(0);

        foreach (Card card in _cardsParentTransform.GetComponentsInChildren<Card>()) {
            Destroy(card.gameObject);
        }
    }

    public void UpdateScore(int score) {
        if (score == 0) {
            _scoreText.text = "";
        } else {
            _scoreText.text = score.ToString();
        }
    }
}
