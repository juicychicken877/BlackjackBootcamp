using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using BlackjackNamespace;

public class GameVisualManager : MonoBehaviour
{
    [SerializeField] private UIDocument _gameViewUI;

    // Action buttons
    private Button _hitBtn;
    private Button _standBtn;
    private Button _splitBtn;
    private Button _doubleDownBtn;
    // Other buttons
    private Button _nextRoundBtn;
    private Button _dealBtn;

    public event EventHandler<ActionBtnClickedEventArgs> ActionBtnClicked;
    public event EventHandler NextRoundBtnClicked;
    public event EventHandler DealBtnClicked;

    public class ActionBtnClickedEventArgs : EventArgs {
        public GameAction Action;
    }


    private void OnEnable() {
        VisualElement root = _gameViewUI.rootVisualElement;

        // Find buttons.
        _hitBtn = root.Q<Button>("HitBtn");
        _standBtn = root.Q<Button>("StandBtn");
        _splitBtn = root.Q<Button>("SplitBtn");
        _doubleDownBtn = root.Q<Button>("DoubleDownBtn");

        _nextRoundBtn = root.Q<Button>("NextRoundBtn");
        _dealBtn = root.Q<Button>("DealBtn");

        // Invoke an event once clicked.
        _hitBtn.clicked += () => { ActionBtnClicked?.Invoke(this, new() { Action = GameAction.Hit }); };
        _standBtn.clicked += () => { ActionBtnClicked?.Invoke(this, new() { Action = GameAction.Stand }); };
        _splitBtn.clicked += () => { ActionBtnClicked?.Invoke(this, new() { Action = GameAction.Split }); };
        _doubleDownBtn.clicked += () => { ActionBtnClicked?.Invoke(this, new() { Action = GameAction.DoubleDown }); };

        _nextRoundBtn.clicked += () => { NextRoundBtnClicked?.Invoke(this, EventArgs.Empty); };
        _dealBtn.clicked += () => { DealBtnClicked?.Invoke(this, EventArgs.Empty); };
    }

    public void SetActionBtns(PlayerHand currPlayerHand) {
        List<GameAction> avaliableGameActions = currPlayerHand.GetAvaliableGameActions();

        avaliableGameActions = ChipManager.Instance.CorrectAvaliableGameActions(currPlayerHand, avaliableGameActions);

        _hitBtn.SetEnabled(avaliableGameActions.Contains(GameAction.Hit));
        _standBtn.SetEnabled(avaliableGameActions.Contains(GameAction.Stand));
        _splitBtn.SetEnabled(avaliableGameActions.Contains(GameAction.Split));
        _doubleDownBtn.SetEnabled(avaliableGameActions.Contains(GameAction.DoubleDown));
    }

    public void SetActionBtns(bool active) {
        _hitBtn.SetEnabled(active);
        _standBtn.SetEnabled(active);
        _splitBtn.SetEnabled(active);
        _doubleDownBtn.SetEnabled(active);
    }

    public void SetNextRoundBtn(bool visible) {
        if (visible) _nextRoundBtn.style.display = DisplayStyle.Flex;
        else _nextRoundBtn.style.display = DisplayStyle.None;
    }

    public void SetDealBtn(bool visible, bool enabled) {
        // Set visibility.
        if (visible) _dealBtn.style.display = DisplayStyle.Flex;
        else _dealBtn.style.display = DisplayStyle.None;

        _dealBtn.SetEnabled(enabled);
    }
}
