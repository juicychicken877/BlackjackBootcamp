using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BlackjackNamespace;
public class HandsManager : MonoBehaviour
{
    [SerializeField] private HandsVisualManager _visuals;
    [SerializeField] private DealerHand _dealerHand;
    [SerializeField][Range(1, 6)] private int _playerHandCount = 3;

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

    public async Task SplitHand(PlayerHand playerHand, int delayBetweenCards) {
        await Task.Delay(delayBetweenCards);

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

        ChipManager.Instance.HandleSplit(newPlayerHand, playerHand);

        // Set order
        _visuals.SetPlayerHandIndex(newPlayerHand);

        await Task.Delay(delayBetweenCards);

        // Move second card to new hand
        newPlayerHand.AddCard(playerHand.GetCard(1), GameAction.Split);
        playerHand.RemoveCard(1);


        _currPlayerHand = playerHand;
    }

    public void NextHand() {
        // Set previous hand inactive.
        if (_currPlayerHand != null) _currPlayerHand.Visuals.SetHandActive(false);

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

        // If chip count of current hand is 0 then go to next one.
        if (_currPlayerHand != null && _currPlayerHand.ChipField.ChipCount == 0) 
            NextHand();

        // Set current hand active.
        if (_currPlayerHand != null) {
            _currPlayerHand.Visuals.SetHandActive(true);
        }
    }

    public void DisableBetting() {
        foreach (var hand in _playerHands) {
            hand.ChipField.Visuals.ChangeActionBtnsActive(false);
            hand.ChipField.Visuals.DisableInteractions();
        }
    }

    // Get the number of hands with money in chip fields.
    public List<PlayerHand> GetPlayingHands() {
        List<PlayerHand> playingHands = new();

        foreach (var hand in _playerHands) {
            if (hand.ChipField.ChipCount > 0) playingHands.Add(hand);
        }
        return playingHands;
    }
}
