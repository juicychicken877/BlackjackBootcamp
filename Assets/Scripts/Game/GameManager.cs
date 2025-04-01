using BlackjackNamespace;
using NUnit.Framework;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameVisualManager _visuals;
    [SerializeField] private Dealer _dealer;
    [SerializeField] private HandsManager _handsManager;
    [SerializeField] private ChipManager _chipManager;
    [SerializeField] private InsuranceBet _insuranceBet;

    public delegate void ActionHandler();

    private void Awake() {
        _visuals.ActionBtnClicked += HandleGameActionAsync;
        _visuals.NextRoundBtnClicked += NextRound;
        _visuals.DealBtnClicked += DealCards;
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
        // Set blackjack game action buttons
        _visuals.SetNextRoundBtn(false);
        _visuals.SetDealBtn(true, true);

        // Update Chip Manager and Visuals
        _chipManager.Visuals.SetChipMenu(true);
        _chipManager.Visuals.UpdateVisuals(_chipManager.Balance);

        // Create new hands and setup click handlers for chip fields.
        _handsManager.NewHands();
        _chipManager.SetChipFieldHandlers(_handsManager.PlayerHands);
    }

    private async void DealCards(object sender, System.EventArgs e) {
        // No money in chip fields.
        if (_handsManager.GetPlayingHands().Count == 0) {
            Debug.Log("Bet some money cuh");
            return;
        }

        // Turn off chip menu buttons and blackjack game action buttons, also disable betting.
        _chipManager.Visuals.SetChipMenu(false);
        _visuals.SetDealBtn(false, false);
        _handsManager.DisableBetting();

        // Deal and start game.
        await _dealer.DealFirstCardsAsync(_handsManager.GetPlayingHands(), _handsManager.DealerHand);

        AfterFirstCards();
    }

    private async Task GameResults() {
        DealerHand dealerHand = _handsManager.DealerHand;

        foreach (var hand in _handsManager.GetPlayingHands()) {
            // If not busted.
            if (hand.ChipField.ChipCount > 0) {
                // Win
                if (hand.Score > dealerHand.Score || dealerHand.Score > 21) {
                    _chipManager.HandleWin(hand);
                }
                // Loss
                else if (hand.Score < dealerHand.Score) {
                    _chipManager.HandleLoss(hand);
                }
                // Push
                else if (hand.Score == dealerHand.Score) {
                    _chipManager.HandlePush(hand);
                }
            }

            await Task.Delay(_dealer.DelayBetweenCardsMs);
        }
    }

    private void StartInsuranceBet() {
        _insuranceBet.StartBet(_handsManager.PlayerHands);

        // End of insurance bet.
        _insuranceBet.EndOfInsuranceBet += (sender, e) => {
            if (_handsManager.DealerHand.HasBlackjack()) {
                // Dealer has blackjack, award insurance bets and move on.
                _handsManager.DealerHand.ShowHiddenCard();

                // Award insurance bets.

                _visuals.SetNextRoundBtn(true);
            } else {
                // Dealer has no blackjack, start blackjack game.
                _handsManager.NextHand();

                _visuals.SetActionBtns(_handsManager.CurrPlayerHand);
            }
        };
    }

    // Method invoked after dealing the initial hands (set of 2 cards).
    private async void AfterFirstCards() {
        if (_handsManager.DealerHand.HasAceFront()) {
            StartInsuranceBet();
        } else {
            // If not, continue the game
            _handsManager.NextHand();

            await CheckForPlayerBlackjack();

            _visuals.SetActionBtns(_handsManager.CurrPlayerHand);
        }
    }

    private async Task CheckForPlayerBlackjack() {
        if (_handsManager.CurrPlayerHand.HasBlackjack()) {
            await _dealer.HandlePlayerBlackjackAsync(_handsManager.CurrPlayerHand);
            _handsManager.NextHand();
            AfterGameAction();
        }
    }

    // Method invoked after hitting, standing, splitting or doubling down.
    private async void AfterGameAction() {
        // Next player hand.
        if (_handsManager.CurrPlayerHand != null) {
            // If it was splitted - CurrPlayerHand doesnt have 2nd card.
            if (_handsManager.CurrPlayerHand.GetCard(1) == null) {
                await _dealer.HandleHitAsync(_handsManager.CurrPlayerHand, null);
            }

            await CheckForPlayerBlackjack();

            _visuals.SetActionBtns(_handsManager.CurrPlayerHand);
        } else {
            // Dealer turn.
            _visuals.SetActionBtns(false);

            if (_handsManager.GetPlayingHands().Count > 0) {
                await _dealer.DrawUntil17Async(_handsManager.DealerHand);
            } else {
                // If all hands busted, just show hidden card.
                _handsManager.DealerHand.ShowHiddenCard();
            }

            await GameResults();

            _visuals.SetNextRoundBtn(true);
        }
    }

}
