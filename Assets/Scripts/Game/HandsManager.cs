using System.Collections.Generic;
using UnityEngine;

public class HandsManager : MonoBehaviour
{
    [SerializeField] private HandsVisualManager _visuals;
    [SerializeField] private DealerHand _dealerHand;
    [SerializeField][Range(1, 3)] private int _playerHandCount = 3;

    private List<PlayerHand> _playerHands;
    private PlayerHand _currPlayerHand;

    public List<PlayerHand> PlayerHands {
        get => _playerHands;
    }
    public DealerHand DealerHand {
        get => _dealerHand;
    }
    public PlayerHand CurrPlayerHand {
        get => _currPlayerHand;
    }

    public void CreateHands() {
        _playerHands ??= new();

        _playerHands = _visuals.CreateNewHands(_playerHandCount);
    }

    public void ClearAllHands() {
        // Clear player hands.
        foreach (var hand in _playerHands) {
            hand.Clear();
        }

        _dealerHand.Clear();
    }

    public void NextHand() {
        // Set previous hand inactive.
        if (_currPlayerHand != null) _currPlayerHand.SetHandActive(false);

        if (_currPlayerHand == null) {
            _currPlayerHand = _playerHands[0];
        } else {
            // All player hands were played.
            if (_currPlayerHand.Index == _playerHandCount - 1) {
                Debug.Log("All hands played");
                _currPlayerHand = null;
            } else {
                _currPlayerHand = _playerHands[_currPlayerHand.Index + 1];
            }
        }

        // Set current hand active.
        if (_currPlayerHand != null) _currPlayerHand.SetHandActive(true);
    }

}
