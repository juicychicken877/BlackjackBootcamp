using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipField : MonoBehaviour, IChipHolder
{
    [SerializeField] private ChipFieldVisual _visuals;

    private Stack<float> _chipStack;
    private float _chipCount;

    public ChipManager.ChipFieldClickHandler ClickHandler;
    public ChipManager.ChipReturnHandler ChipReturnHandler;

    public float ChipCount {
        get => _chipCount;
    }

    public ChipFieldVisual Visuals {
        get => _visuals;
    }

    private void Awake() {
        _visuals.InteractableBtnClick += (sender, e) => {
            ClickHandler?.Invoke(this);
        };

        _visuals.ClearFieldBtnClick += (sender, e) => {
            ChipReturnHandler?.Invoke(this, _chipCount);

            _visuals.UpdateVisuals(_chipCount);
        };

        _visuals.UndoBtnClick += (sender, e) => {
            ChipReturnHandler?.Invoke(this, _chipStack.Peek());

            _visuals.UpdateVisuals(_chipCount);
        };
    }

    public void AddChips(float chips) {
        _chipStack ??= new();

        _chipCount += chips;

        _chipStack.Push(chips);

        _visuals.UpdateVisuals(_chipCount);
    }

    public void PopChips() {
        if (_chipStack != null) {
            _chipCount -= _chipStack.Peek();
            _chipStack.Pop();
            _visuals.UpdateVisuals(_chipCount);
        }
    }

    public void ClearChips() {
        _chipCount = 0;

        _visuals.UpdateVisuals(_chipCount);
    }
}
