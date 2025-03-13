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

    public void NewHands() {
        _dealerHand.Clear();

        _playerHands = _visuals.CreateNewHands(_playerHands, _playerHandCount);
    }

    public void SplitHand(PlayerHand playerHand) {
        // Set index+1 for every hand that comes after playerHand
        for (int i=_playerHands.Count-1; i>=playerHand.Index+1; i--) {
            PlayerHand hand = _playerHands.Find(playerHand => playerHand.Index == i);

            if (hand != null) {
                hand.Index += 1;
            }
        }

        // Create new hand.
        PlayerHand newPlayerHand = _visuals.NewPlayerHand();
        newPlayerHand.Index = playerHand.Index + 1;
        _playerHands.Insert(newPlayerHand.Index, newPlayerHand);

        // Set order
        _visuals.SetPlayerHandIndex(newPlayerHand);
        
        // Move second card to new hand
        newPlayerHand.AddCard(playerHand.GetCard(1));
        playerHand.RemoveCard(1);

        _currPlayerHand = playerHand;
    }

    public void NextHand() {
        // Set previous hand inactive.
        if (_currPlayerHand != null) _currPlayerHand.SetHandActive(false);

        if (_currPlayerHand == null) {
            _currPlayerHand = _playerHands[0];
        } else {
            // All player hands were played.
            if (_currPlayerHand.Index == _playerHands.Count - 1) {
                _currPlayerHand = null;
            } else {
                _currPlayerHand = _playerHands[_currPlayerHand.Index + 1];
            }
        }

        // Set current hand active.
        if (_currPlayerHand != null) {
            _currPlayerHand.SetHandActive(true);
        }
    }

}
