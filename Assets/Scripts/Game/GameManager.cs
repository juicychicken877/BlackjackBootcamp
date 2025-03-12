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

    private void Start() {
        _handsManager.CreateHands();
    }

    private void HandleGameAction(object sender, GameVisualManager.ActionBtnClickedEventArgs e) {
        GameAction gameAction = e.action;

        switch (gameAction) {
            case GameAction.Hit: {
                _dealer.HandleHit(_handsManager.CurrPlayerHand, () => {
                    // Function called when a player busts a hand.
                    _handsManager.NextHand();
                });

                if (_handsManager.CurrPlayerHand != null) {
                    _visuals.SetActionButtons(_handsManager.CurrPlayerHand.GetAvaliableGameActions());
                }
            }
            break;
            case GameAction.Stand: {
                _handsManager.NextHand();

                if (_handsManager.CurrPlayerHand != null) {
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
                _dealer.HandleSplit();
            }
            break;
            case GameAction.DoubleDown: {
                _dealer.HandleDoubleDown(_handsManager.CurrPlayerHand);

                _handsManager.NextHand();

                if (_handsManager.CurrPlayerHand != null) {
                    _visuals.SetActionButtons(_handsManager.CurrPlayerHand.GetAvaliableGameActions());
                }
            }
            break;
        }
    }

    private void NextRound(object sender, System.EventArgs e) {
        _visuals.SetNextRoundBtn(false);

        _handsManager.ClearAllHands();

        _dealer.DealFirstCards(_handsManager.PlayerHands, _handsManager.DealerHand, () => { AfterFirstCards(); });
    }

    private void AfterFirstCards() {
        _handsManager.NextHand();

        _visuals.SetActionButtons(_handsManager.CurrPlayerHand.GetAvaliableGameActions());
    }

}
