using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameVisualManager _visuals;
    [SerializeField] private Dealer _dealer;
    [SerializeField] private HandsManager _handsManager;

    public enum GameAction { 
        Hit,
        Stand,
        DoubleDown,
        Split,
        None
    }

    public delegate void ActionHandler();

    private void Awake() {
        _visuals.ActionBtnClicked += HandleGameActionAsync;
        _visuals.NextRoundBtnClicked += NextRound;
    }

    private async void HandleGameActionAsync(object sender, GameVisualManager.ActionBtnClickedEventArgs e) {
        GameAction gameAction = e.action;

        _visuals.SetActionButtons(false);

        switch (gameAction) {
            case GameAction.Hit: {
                await _dealer.HandleHitAsync(_handsManager.CurrPlayerHand, () => {
                    // Function called when a player busts a hand.
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
                await _handsManager.SplitHand(_handsManager.CurrPlayerHand, _dealer.DelayBetweenCardsMiliseconds);

                AfterGameAction();
            }
            break;
            case GameAction.DoubleDown: {
                bool busted = false;

                await _dealer.HandleDoubleDownAsync(_handsManager.CurrPlayerHand, () => {
                    // Function called when a player busts a hand.
                    _handsManager.NextHand();
                    busted = true;
                });

                if (!busted) _handsManager.NextHand();

                AfterGameAction();
            }
            break;
        }
    }

    private async void NextRound(object sender, System.EventArgs e) {
        _visuals.SetNextRoundBtn(false);

        _handsManager.NewHands();

        await _dealer.DealFirstCardsAsync(_handsManager.PlayerHands, _handsManager.DealerHand);

        AfterFirstCards();
    }

    // Method invoked after dealing the intiial hands (set of 2 cards).
    private void AfterFirstCards() {
        // Check for blackjack
        if (_handsManager.DealerHand.HasBlackjack()) {
            _handsManager.DealerHand.ShowHiddenCard();

            _visuals.SetNextRoundBtn(true);
        } else {
            // If not, continue the game
            _handsManager.NextHand();

            _visuals.SetActionButtons(_handsManager.CurrPlayerHand.GetAvaliableGameActions());
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

            _visuals.SetActionButtons(_handsManager.CurrPlayerHand.GetAvaliableGameActions());
        } else {
            // Dealer turn.
            _visuals.SetActionButtons(false);

            await _dealer.DrawUntil17Async(_handsManager.DealerHand);

            _visuals.SetNextRoundBtn(true);
        }
    }

}
