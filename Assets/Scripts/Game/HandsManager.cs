using System.Collections.Generic;
using UnityEngine;

public class HandsManager : MonoBehaviour
{
    [SerializeField] private HandsVisualManager _visuals;
    [SerializeField] private Hand _dealerHand;
    [SerializeField] private int _playerHandCount = 3;

    private List<Hand> _playerHands;
    private Hand _currHand;

    public List<Hand> PlayerHands {
        get => _playerHands;
    }
    public Hand DealerHand {
        get => _dealerHand;
    }
    public Hand CurrentPlayerHand {
        get => _currHand;
    }

    public void CreateHands() {
        _playerHands ??= new();

        _playerHands = _visuals.CreateNewHands(_playerHandCount);
    }

    public void NextHand() {
        if (_currHand == null) {
            _currHand = _playerHands[0];
        } else {
            _currHand = _playerHands[_currHand.Index + 1];
        }

        Debug.Log($"Current Hand: {_currHand.gameObject.name}");
    }

}
