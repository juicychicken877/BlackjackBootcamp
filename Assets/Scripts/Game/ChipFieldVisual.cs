using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChipFieldVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _chipCountText;
    [SerializeField] private GameObject _chipActionBtns;
    [SerializeField] private Button _clearFieldBtn;
    [SerializeField] private Button _undoBtn;
    [SerializeField] private Button _interactableBtn;

    public event EventHandler InteractableBtnClick;
    public event EventHandler ClearFieldBtnClick;
    public event EventHandler UndoBtnClick;

    private void Awake() {
        _interactableBtn.onClick.AddListener(() => {
            InteractableBtnClick?.Invoke(this, EventArgs.Empty);
        });

        _clearFieldBtn.onClick.AddListener(() => {
            ClearFieldBtnClick?.Invoke(this, EventArgs.Empty);
        });

        _undoBtn.onClick.AddListener(() => {
            UndoBtnClick?.Invoke(this, EventArgs.Empty);
        });
    }

    public void UpdateVisuals(int chipCount) {
        if (chipCount == 0) {
            _chipCountText.text = "";

            ChangeActionBtnsActive(false);
        } else {
            _chipCountText.text = chipCount.ToString();
        }
    }

    public void ChangeActionBtnsActive(bool active) {
        _chipActionBtns.SetActive(active);
    }

    public void DisableInteractions() {
        _interactableBtn.interactable = false;
    }
}
