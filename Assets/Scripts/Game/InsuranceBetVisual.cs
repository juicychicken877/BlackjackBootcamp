using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsuranceBetVisual : MonoBehaviour
{
    [SerializeField] private GameObject _insuranceChoicePrefab;
    [SerializeField] private Transform _canvas;
    [SerializeField] private Transform _btnSetPos;
    [SerializeField] private Button _closeBetsBtn;

    public event EventHandler CloseBetsBtnClick;

    private void Awake() {
        _closeBetsBtn.onClick.AddListener(() => {
            CloseBetsBtnClick?.Invoke(this, EventArgs.Empty);
        });
    }

    public List<InsuranceBet.Set> GenerateVisuals(List<PlayerHand> playerHands) {
        _closeBetsBtn.gameObject.SetActive(true);

        List<InsuranceBet.Set> sets = new();

        // Generate buttons for every hand.
        foreach (PlayerHand hand in playerHands) {
            Vector3 pos = hand.gameObject.transform.position;

            Vector3 checkboxPos = new(pos.x, _btnSetPos.position.y, pos.z);

            GameObject checkboxObj = Instantiate(_insuranceChoicePrefab);

            checkboxObj.transform.SetParent(_canvas, false);
            checkboxObj.transform.localPosition = checkboxPos;

            InsuranceChoice insuranceChoice = checkboxObj.GetComponent<InsuranceChoice>();
            insuranceChoice.ChangeInsuranceValue(ChipManager.Instance.GetInsuranceValue(hand));

            sets.Add(new(hand, insuranceChoice));
        }

        return sets;
    }

    public void HideVisuals() {
        _closeBetsBtn.gameObject.SetActive(false);

        foreach (var insuranceChoice in _canvas.GetComponentsInChildren<InsuranceChoice>()) {
            Destroy(insuranceChoice.gameObject);
        }
    }
}
