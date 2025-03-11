using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandVisual : HandVisual
{
    [SerializeField] private Transform _cardsParentTransform;
    [SerializeField] private float _nextCardOffsetX = 1.5f;
    [SerializeField] private float _nextCardOffsetY = 2.5f;
    [SerializeField] private float _nextCardOffsetZ = -0.05f;

    private List<Card> _cards;

    public override void AddCard(CardSO cardSO) {
        _cards ??= new();

        GameObject newCard = Instantiate(cardSO.Prefab);
        
        Vector3 newCardPos = new Vector3(_cards.Count * _nextCardOffsetX, _cards.Count * _nextCardOffsetY, _cards.Count * _nextCardOffsetZ);
        
        newCard.transform.SetParent(_cardsParentTransform, false);
        newCard.transform.localPosition = newCardPos;

        Card newCardScript = newCard.GetComponent<Card>();

        _cards.Add(newCardScript);
    }
}
