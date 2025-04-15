using UnityEngine;
using UnityEngine.UI;

public class InsuranceChoice : MonoBehaviour, IChipHolder
{
    [SerializeField] private InsuranceChoiceVisual _visuals;

    private float _chipCount;
    private float _insuranceValue;
    private bool _isInsured;

    public ChipManager.TakeInsuranceHandler TakeInsuranceHandler;
    public ChipManager.ChipReturnHandler ReturnHandler;

    public float Chips {
        get => _chipCount;
    }

    public bool IsInsured {
        get => _isInsured;
    }

    public float InsuranceValue {
        get => _insuranceValue;
        set => _insuranceValue = value;
    }

    private void Awake() {
        _visuals.Checkbox.onValueChanged.AddListener((state) => {
            // If checked true, take insurance.
            if (state == true) {
                TakeInsuranceHandler(this);
            } else {
                // If unchecked, return insurance chips.
                ReturnHandler(this, _chipCount);

                ChangeInsured(false);
            }
        });
    }

    public void ChangeInsured(bool insured) {
        _isInsured = insured;

        if (_visuals.Checkbox.isOn != insured)
            _visuals.Checkbox.isOn = insured;
    }

    public void ChangeInsuranceValue(float insuranceValue) {
        InsuranceValue = insuranceValue;

        _visuals.ChangeInsuranceValueText(insuranceValue);
    }

    public void AddChips(float amount) {
        _chipCount += amount;
    }

    public void ClearChips() {
        _chipCount = 0;
    }

    public void PopChips() {
        _chipCount = 0;
    }
}
