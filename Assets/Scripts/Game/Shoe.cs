using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Shoe : MonoBehaviour
{
    [SerializeField] private List<CardSO> _cardsSO;
    [SerializeField] private int _numberOfDecks = 2;

    private List<CardSO> _currentShoe;

    public int NumberOfDecks {
        get => _numberOfDecks;
        set => _numberOfDecks = value;
    }

    public List<CardSO> CurrentShoe {
        get => _currentShoe;
    }

    public void NewShoe() {
        List<CardSO> newShoe = new();

        // Add cards to shoe - every card * _numberOfDecks;
        for (int i = 0; i < _numberOfDecks; i++) {
            foreach (var card in _cardsSO) {
                newShoe.Add(card);
            }
        }

        // Shuffle
        int n = newShoe.Count;
        System.Random random = new();

        while (n > 1) {
            n--;
            int k = random.Next(n + 1);
            (newShoe[n], newShoe[k]) = (newShoe[k], newShoe[n]);
        }

        _currentShoe = newShoe;
    }

    public CardSO NextCard() {
        if (_currentShoe.Count == 0) {
            Debug.Log("Shoe is empty");
            return null;
        } else {
            CardSO nextCard = _currentShoe[0];

            _currentShoe.RemoveAt(0);

            return nextCard;
        }
    }
}
