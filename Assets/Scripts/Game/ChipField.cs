using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipField : MonoBehaviour, IChipHolder
{
    [SerializeField] private ChipFieldVisual _visuals;

    private Stack<float> _chipStack;
    private float _chips;

    public ChipManager.ChipFieldClickHandler ClickHandler;
    public ChipManager.ChipReturnHandler ChipReturnHandler;

    public float Chips {
        get => _chips;
    }

    public ChipFieldVisual Visuals {
        get => _visuals;
    }

    private void Awake() {
        _visuals.InteractableBtnClick += (sender, e) => {
            ClickHandler?.Invoke(this);
        };

        _visuals.ClearFieldBtnClick += (sender, e) => {
            ChipReturnHandler?.Invoke(this, _chips);

            _visuals.UpdateVisuals(_chips);
        };

        _visuals.UndoBtnClick += (sender, e) => {
            ChipReturnHandler?.Invoke(this, _chipStack.Peek());

            _visuals.UpdateVisuals(_chips);
        };
    }

    public void AddChips(float chips) {
        _chipStack ??= new();

        _chips += chips;

        _chipStack.Push(chips);

        _visuals.UpdateVisuals(_chips);
    }

    public void PopChips() {
        if (_chipStack != null) {
            _chips -= _chipStack.Peek();
            _chipStack.Pop();
            _visuals.UpdateVisuals(_chips);
        }
    }

    public void ClearChips() {
        _chips = 0;

        _visuals.UpdateVisuals(_chips);
    }
}
