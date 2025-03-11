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
    }

    private void HandleGameAction(object sender, GameVisualManager.ActionBtnClickedEventArgs e) {
        GameAction gameAction = e.action;
    }

    private void Start() {
        _handsManager.CreateHands();

        _dealer.DealFirstCards(_handsManager.PlayerHands, _handsManager.DealerHand, () => { AfterFirstCards(); });
    }

    private void AfterFirstCards() {
        _handsManager.NextHand();

        _visuals.SetActionButtons(_handsManager.CurrentPlayerHand.GetAvaliableGameActions());
    }

}
