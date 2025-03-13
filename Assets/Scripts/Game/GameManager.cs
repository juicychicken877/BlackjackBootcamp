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
        Split
    }

    public delegate void Callback();

    private void Awake() {
        _visuals.ActionBtnClicked += HandleGameAction;
        _visuals.NextRoundBtnClicked += NextRound;
    }

    private void HandleGameAction(object sender, GameVisualManager.ActionBtnClickedEventArgs e) {
        GameAction gameAction = e.action;

        switch (gameAction) {
            case GameAction.Hit: {
                // Action
                _dealer.HandleHit(_handsManager.CurrPlayerHand, () => {
                    // Function called when a player busts a hand.
                    _handsManager.NextHand();
                });

                // Next player hand.
                if (_handsManager.CurrPlayerHand != null) {
                    // If it was splitted - CurrPlayerHand doesnt have 2nd card
                    if (_handsManager.CurrPlayerHand.GetCard(1) == null) {
                        _dealer.HandleHit(_handsManager.CurrPlayerHand, null);
                    }

                    _visuals.SetActionButtons(_handsManager.CurrPlayerHand.GetAvaliableGameActions());
                } else {
                    // Dealer turn.
                    _visuals.SetActionButtons(false);

                    _dealer.DrawUntil17(_handsManager.DealerHand, () => {
                        // Function called when drawing ends.
                        _visuals.SetNextRoundBtn(true);
                    });
                }
            }
            break;
            case GameAction.Stand: {
                // Action
                _handsManager.NextHand();

                // Next player hand.
                if (_handsManager.CurrPlayerHand != null) {
                    // If it was splitted - CurrPlayerHand doesnt have 2nd card
                    if (_handsManager.CurrPlayerHand.GetCard(1) == null) {
                        _dealer.HandleHit(_handsManager.CurrPlayerHand, null);
                    }

                    _visuals.SetActionButtons(_handsManager.CurrPlayerHand.GetAvaliableGameActions());
                } else {
                    // Dealer's turn
                    _visuals.SetActionButtons(false);

                    _dealer.DrawUntil17(_handsManager.DealerHand, () => {
                        // Function called when drawing ends.
                        _visuals.SetNextRoundBtn(true);
                    });
                }
            }
            break;
            case GameAction.Split: {
                // Split hand
                _handsManager.SplitHand(_handsManager.CurrPlayerHand);

                // Hit first hand
                _dealer.HandleHit(_handsManager.CurrPlayerHand, null);

                // Show avaliable game actions
                _visuals.SetActionButtons(_handsManager.CurrPlayerHand.GetAvaliableGameActions());
            }
            break;
            case GameAction.DoubleDown: {
                // Action
                _dealer.HandleDoubleDown(_handsManager.CurrPlayerHand);
                _handsManager.NextHand();

                // Next player hand.
                if (_handsManager.CurrPlayerHand != null) {
                    // If it was splitted - CurrPlayerHand doesnt have 2nd card
                    if (_handsManager.CurrPlayerHand.GetCard(1) == null) {
                        _dealer.HandleHit(_handsManager.CurrPlayerHand, null);
                    }

                    _visuals.SetActionButtons(_handsManager.CurrPlayerHand.GetAvaliableGameActions());
                } else {
                    // Dealer turn.
                    _visuals.SetActionButtons(false);

                    _dealer.DrawUntil17(_handsManager.DealerHand, () => {
                        // Function called when drawing ends.
                        _visuals.SetNextRoundBtn(true);
                    });
                }
            }
            break;
        }
    }

    private void NextRound(object sender, System.EventArgs e) {
        _visuals.SetNextRoundBtn(false);

        _handsManager.NewHands();

        _dealer.DealFirstCards(_handsManager.PlayerHands, _handsManager.DealerHand, () => { AfterFirstCards(); });
    }

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

}
