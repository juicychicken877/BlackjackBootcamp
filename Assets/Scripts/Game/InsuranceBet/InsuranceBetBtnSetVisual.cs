using System;
using UnityEngine;
using UnityEngine.UI;

public class InsuranceBetBtnSetVisual : MonoBehaviour
{
    [SerializeField] private Button _yesBtn;
    [SerializeField] private Button _noBtn;

    public event EventHandler YesBtnClicked;
    public event EventHandler NoBtnClicked;

    public void YesBtnClick() {
        YesBtnClicked?.Invoke(this, EventArgs.Empty);
    }

    public void NoBtnClick() {
        NoBtnClicked?.Invoke(this, EventArgs.Empty);
    }
}
