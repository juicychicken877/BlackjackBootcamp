using BlackjackNamespace;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameVisualManager _visuals;
    [SerializeField] private Dealer _dealer;
    [SerializeField] private HandsManager _handsManager;
    [SerializeField] private ChipManager _chipManager;
    [SerializeField] private InsuranceBet _insuranceBet;

    private GameSettings _gameSettings;

    public delegate void ActionHandler();

    private void Awake() {
        _visuals.ActionBtnClicked += HandleGameActionAsync;
        _visuals.NextRoundBtnClicked += NextRound;
        _visuals.DealBtnClicked += DealCards;

        _insuranceBet.EndOfInsuranceBet += (sender, e) => {
            ResolveInsuranceBet();
        };

        // Set new game settings.
        NewGameSettings(new(
            true
        ));
    }

    private async void HandleGameActionAsync(object sender, GameVisualManager.ActionBtnClickedEventArgs e) {
        GameAction gameAction = e.Action;

        _visuals.SetActionBtns(false);

        switch (gameAction) {
            case GameAction.Hit: {
                await _dealer.HandleHitAsync(_handsManager.CurrPlayerHand, () => {
                    // Function called when a player busts a hand.
                    _chipManager.HandleLoss(_handsManager.CurrPlayerHand);

                    _handsManager.NextHand();
                });

                AfterGameAction();
            }
            break;
            case GameAction.Stand: {
                _handsManager.NextHand();

                AfterGameAction();
            }
            break;
            case GameAction.Split: {
                await _handsManager.SplitHand(_handsManager.CurrPlayerHand, _dealer.DelayBetweenCardsMs);

                AfterGameAction();
            }
            break;
            case GameAction.DoubleDown: {
                bool busted = false;

                await _dealer.HandleDoubleDownAsync(_handsManager.CurrPlayerHand, () => {
                    // Function called when a player busts a hand.
                    _chipManager.HandleLoss(_handsManager.CurrPlayerHand);

                    _handsManager.NextHand();
                    busted = true;
                });

                if (!busted) _handsManager.NextHand();

                AfterGameAction();
            }
            break;
        }
    }

    private void NextRound(object sender, System.EventArgs e) {
        // Collect chips from previous game.
        if (_handsManager.PlayerHands != null) {
            _chipManager.CollectAllChips(_handsManager.PlayerHands);
        }

        // Set blackjack game action buttons
        _visuals.SetNextRoundBtn(false);
        _visuals.SetDealBtn(true, true);

        // Update Chip Manager and Visuals
        _chipManager.Visuals.UpdateChipMenu(_chipManager.Balance);
        _chipManager.Visuals.UpdateBalance(_chipManager.Balance);

        // Create new hands and setup click handlers for chip fields.
        _handsManager.NewHands();
    }

    private async void DealCards(object sender, System.EventArgs e) {
        // Playing hands are those with money in chip fields (Optional).
        List<PlayerHand> playingHands = _handsManager.PlayerHands.FindAll(hand => hand.ChipField.ChipCount > 0);

        // No money in chip fields.
        if (playingHands.Count == 0) {
            return;
        }

        // Turn off chip menu buttons and blackjack game action buttons, also disable betting.
        _chipManager.Visuals.UpdateChipMenu(false);
        _visuals.SetDealBtn(false, false);
        _handsManager.DisableBetting();

        // Deal and start game.
        await _dealer.DealFirstCardsAsync(playingHands, _handsManager.DealerHand);

        AfterFirstCards();
    }

    private void ResolveInsuranceBet() {
        // Dealer has blackjack, award insurance bets and move on.
        if (_handsManager.DealerHand.HasBlackjack()) {
            _handsManager.DealerHand.ShowHiddenCard();
            _insuranceBet.HandleBlackjackCase();
            _visuals.SetNextRoundBtn(true);
        } else {
            // Dealer has no blackjack, start blackjack game.
            _insuranceBet.HandleNoBlackjackCase();
            _handsManager.NextHand();
            AfterGameAction();
        }

        _insuranceBet.Visuals.HideVisuals();
    }

    // Method invoked after dealing the initial hands (set of 2 cards).
    private async void AfterFirstCards() {
        if (_handsManager.DealerHand.HasAceFront()) {
            _insuranceBet.StartInsuranceBet(_handsManager.PlayerHands);
        } else {
            // Check for dealer's blackjack (10 front).
            if (_handsManager.DealerHand.HasBlackjack()) {
                _handsManager.DealerHand.ShowHiddenCard();

                // Handle losses.
                foreach (var hand in _handsManager.PlayerHands) {
                    _chipManager.HandleLoss(hand);
                }

                _visuals.SetNextRoundBtn(true);
            } else {
                // If not, continue the game.
                _handsManager.NextHand();

                await CheckForPlayerBlackjack();

                _visuals.SetActionBtns(_handsManager.CurrPlayerHand);
            }
        }
    }

    private async Task CheckForPlayerBlackjack() {
        if (_handsManager.CurrPlayerHand.HasBlackjack()) {
            await _dealer.HandlePlayerBlackjackAsync(_handsManager.CurrPlayerHand);
            _handsManager.NextHand();
            AfterGameAction();
        }
    }

    // Method invoked mostly after calling NextHand() and after splitting.
    private async void AfterGameAction() {
        // Next player hand.
        if (_handsManager.CurrPlayerHand != null) {
            // If it was splitted - CurrPlayerHand doesnt have 2nd card.
            if (_handsManager.CurrPlayerHand.GetCard(1) == null) {
                await _dealer.HandleHitAsync(_handsManager.CurrPlayerHand, null);
            }

            await CheckForPlayerBlackjack();

            if (_handsManager.CurrPlayerHand) 
                _visuals.SetActionBtns(_handsManager.CurrPlayerHand);
        } else {
            // Dealer turn.
            _visuals.SetActionBtns(false);

            List<PlayerHand> handsLeft = _handsManager.PlayerHands.FindAll(hand => hand.State == HandState.Inactive);

            if (handsLeft.Count > 0) {
                await _dealer.DrawUntil17Async(_handsManager.DealerHand);
            } else {
                // If all hands either won or lost, just show hidden card.
                _handsManager.DealerHand.ShowHiddenCard();
            }

            _chipManager.HandleGameResults(_handsManager.PlayerHands, _handsManager.DealerHand);

            _visuals.SetNextRoundBtn(true);
        }
    }

    public void NewGameSettings(GameSettings newGameSettings) {
        _gameSettings = newGameSettings;
    }

}
