using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    public event EventHandler<ActionBtnClickedEventArgs> ActionBtnClicked;
    public event EventHandler NextRoundBtnClicked;

    public class ActionBtnClickedEventArgs : EventArgs {
        public GameManager.GameAction action;
    }


    private void OnEnable() {
        var root = _gameViewUI.rootVisualElement;

        // Find buttons.
        _hitBtn = root.Q<Button>("HitBtn");
        _standBtn = root.Q<Button>("StandBtn");
        _splitBtn = root.Q<Button>("SplitBtn");
        _doubleDownBtn = root.Q<Button>("DoubleDownBtn");
        _nextRoundBtn = root.Q<Button>("NextRoundBtn");

        // Invoke an event once clicked.
        _hitBtn.clicked += () => { ActionBtnClicked?.Invoke(this, new() { action = GameManager.GameAction.Hit }); };
        _standBtn.clicked += () => { ActionBtnClicked?.Invoke(this, new() { action = GameManager.GameAction.Stand }); };
        _splitBtn.clicked += () => { ActionBtnClicked?.Invoke(this, new() { action = GameManager.GameAction.Split }); };
        _doubleDownBtn.clicked += () => { ActionBtnClicked?.Invoke(this, new() { action = GameManager.GameAction.DoubleDown }); };
        _nextRoundBtn.clicked += () => { NextRoundBtnClicked?.Invoke(this, EventArgs.Empty); };
    }

    public void SetActionButtons(List<GameManager.GameAction> avaliableGameActions) {
        _hitBtn.SetEnabled(avaliableGameActions.Contains(GameManager.GameAction.Hit));
        _standBtn.SetEnabled(avaliableGameActions.Contains(GameManager.GameAction.Stand));
        _splitBtn.SetEnabled(avaliableGameActions.Contains(GameManager.GameAction.Split));
        _doubleDownBtn.SetEnabled(avaliableGameActions.Contains(GameManager.GameAction.DoubleDown));
    }

    public void SetActionButtons(bool active) {
        _hitBtn.SetEnabled(active);
        _standBtn.SetEnabled(active);
        _splitBtn.SetEnabled(active);
        _doubleDownBtn.SetEnabled(active);
    }

    public void SetNextRoundBtn(bool active) {
        _nextRoundBtn.SetEnabled(active);
    }
}
