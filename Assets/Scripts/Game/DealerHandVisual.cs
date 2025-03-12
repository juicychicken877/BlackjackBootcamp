using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealerHandVisual : MonoBehaviour
{
    [SerializeField] private Transform _cardsParentTransform;

    private List<Card> _cards;

    public Card AddCard(CardSO cardSO, bool hidden) {
        _cards ??= new();

        GameObject newCard = Instantiate(cardSO.Prefab);

        Card newCardScript = newCard.GetComponent<Card>();

        if (hidden) newCardScript.Turn(Card.ImagePos.Back);

        _cards.Add(newCardScript);

        newCard.transform.SetParent(_cardsParentTransform, false);

        return newCardScript;
    }

    public void Clear() {
        // Remove cards.
        if (_cards != null) {
            foreach (var card in _cards) {
                Destroy(card.gameObject);
            }
        }

        _cards?.Clear();
    }
}
