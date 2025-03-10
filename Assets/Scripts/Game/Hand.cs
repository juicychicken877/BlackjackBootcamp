using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private HandVisual _visuals;
    private List<CardSO> _cards;

    public void AddCard(CardSO cardSO) {
        _cards ??= new();

        _cards.Add(cardSO);
        _visuals.AddCard(cardSO.Prefab);
    }
}
