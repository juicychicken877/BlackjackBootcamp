using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsuranceChoiceVisual : MonoBehaviour
{
    [SerializeField] private Toggle _checkbox;
    [SerializeField] private TextMeshProUGUI _insuranceValueText;

    public Toggle Checkbox {
        get => _checkbox;
    }

    public void ChangeInsuranceValueText(float insuranceValue) {
        _insuranceValueText.text = $"Insurance Value: {insuranceValue}";
    }
}
