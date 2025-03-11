using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealerHandVisual : HandVisual
{
    [SerializeField] private Transform _cardsParentTransform;

    private List<Card> _cards;

    public override void AddCard(CardSO cardSO) {
        _cards ??= new();

        GameObject newCard = Instantiate(cardSO.Prefab);

        newCard.transform.SetParent(_cardsParentTransform, false);

        Card newCardScript = newCard.GetComponent<Card>();

        _cards.Add(newCardScript);
    }
}
