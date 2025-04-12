using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChipVisualManager : MonoBehaviour
{
    [SerializeField] private UIDocument _gameViewUI;
    [SerializeField] private string _btnSelectedClassName;

    // Chip buttons.
    private List<Button> _chipMenuBtns;
    private Button _currSelectedBtn;
    private Label _balanceLabel;
    public Button CurrSelectedBtn {
        get => _currSelectedBtn;
    }

    public int CurrSelectedBtnValue {
        get => Convert.ToInt32(_currSelectedBtn.text);
    }

    private void OnEnable() {
        VisualElement root = _gameViewUI.rootVisualElement;

        // Get chip menu buttons by name.
        List<string> btnIds = new() { "100", "50", "25", "10", "5", "1" };

        _chipMenuBtns = new();

        foreach (string id in btnIds) {
            Button btn = root.Q<Button>(id);

            _chipMenuBtns.Add(btn);
        }

        _balanceLabel = root.Q<Label>("Balance");

        // Set click events.
        foreach (Button btn in _chipMenuBtns) {
            int value = Convert.ToInt32(btn.text);

            btn.clicked += () => {
                SelectBtn(btn);
            };
        }
    }

    public void UpdateChipMenu(bool enabled) {
        _currSelectedBtn?.RemoveFromClassList(_btnSelectedClassName);
        _currSelectedBtn = null;

        foreach (Button btn in _chipMenuBtns) {
            btn.SetEnabled(enabled);
        }
    }

    public void UpdateChipMenu(float balance) {
        foreach (Button btn in _chipMenuBtns) {
            float btnValue = (float)Convert.ToDouble(btn.text);

            btn.SetEnabled(balance >= btnValue);
        }
    }

    private void SelectBtn(Button btn) {
        _currSelectedBtn?.RemoveFromClassList(_btnSelectedClassName);

        btn.AddToClassList(_btnSelectedClassName);

        _currSelectedBtn = btn;
    }

    public void UpdateBalance(float balance) {
        // Update balance label.
        _balanceLabel.text = $"Balance: {balance}";

        UpdateChipMenu(balance);
    }
}
