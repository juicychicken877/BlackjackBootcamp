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
    private List<PlayerHand> _playingHands;
    private PlayerHand _currPlayerHand;

    // Hands that match the rules of the game.
    public List<PlayerHand> PlayingHands {
        get => _playingHands;
    }
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

        ChipManager.Instance.SetChipFieldHandlers(_playerHands);
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
        if (_currPlayerHand != null && _currPlayerHand.State == HandState.Active) {
            _currPlayerHand.ChangeState(HandState.Inactive);
        }

        if (_currPlayerHand == null) {
            _currPlayerHand = _playerHands[0];
        } else {
            // Last hand.
            if (_currPlayerHand.Index == _playerHands.Count - 1) {
                _currPlayerHand = null;
            } else {
                // Next hand.
                _currPlayerHand = _playerHands[_currPlayerHand.Index + 1];
            }
        }
        
        // Set current hand active.
        if (_currPlayerHand != null) {
            // No money in a chip field (Optional)
            if (_currPlayerHand.ChipField.ChipCount <= 0) {
                NextHand();
            }
            _currPlayerHand?.ChangeState(HandState.Active);
        }
    }
    public void DisableBetting() {
        foreach (var hand in _playerHands) {
            hand.ChipField.Visuals.ChangeActionBtnsActive(false);
            hand.ChipField.Visuals.DisableInteractions();
        }
    }
}
