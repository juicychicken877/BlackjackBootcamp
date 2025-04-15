using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BlackjackNamespace;
public class HandsManager : MonoBehaviour
{
    [SerializeField] private HandsVisualManager _visuals;
    [SerializeField] private DealerHand _dealerHand;
    [SerializeField][Range(1, 6)] private int _playerHandCount = 3;

    private List<PlayerHand> _playingHands;
    private List<PlayerHand> _playerHands;
    private PlayerHand _currPlayerHand;

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
        _playingHands?.Clear();

        ChipManager.Instance.SetChipFieldHandlers(_playerHands);
    }

    public void SetPlayingHands() {
        _playingHands ??= new();

        // Playing hands are those with money in chip fields.
        foreach (var hand in _playerHands) {
            if (hand.ChipField.Chips > 0) {
                _playingHands.Add(hand);
            }
        }
    }

    public async Task SplitHand(PlayerHand playerHand, int delayBetweenCards) {
        await Task.Delay(delayBetweenCards);

        // Create new hand.
        PlayerHand newPlayerHand = _visuals.NewPlayerHand();

        _playerHands.Insert(_playerHands.IndexOf(playerHand)+1, newPlayerHand);
        _playingHands.Insert(_playingHands.IndexOf(playerHand)+1, newPlayerHand);

        ChipManager.Instance.HandleSplit(newPlayerHand, playerHand);

        // Set order.
        _visuals.SetPlayerHandPos(newPlayerHand, _playerHands.IndexOf(newPlayerHand));

        await Task.Delay(delayBetweenCards);

        // Move second card to new hand.
        await newPlayerHand.AddCard(playerHand.GetCardSO(1), GameAction.Split);
        playerHand.RemoveCard(1);

        _currPlayerHand = playerHand;
    }

    public void NextHand() {
        // Set previous hand inactive.
        if (_currPlayerHand != null && _currPlayerHand.State == HandState.Active) {
            _currPlayerHand.ChangeState(HandState.Inactive);
        }

        if (_currPlayerHand == null) {
            // First hand.
            _currPlayerHand = _playingHands[0];
        } else {
            int currHandIndex = _playingHands.IndexOf(_currPlayerHand);
            // Last hand.
            if (currHandIndex == _playingHands.Count - 1) {
                _currPlayerHand = null;
            } else {
                // Next hand.
                _currPlayerHand = _playingHands[currHandIndex + 1];
            }
        }
        
        if (_currPlayerHand != null) {
            _currPlayerHand.ChangeState(HandState.Active);
        }
    }
}
