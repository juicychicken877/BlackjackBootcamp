using BlackjackNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static InsuranceBet;
using static InsuranceBetVisual;

public class InsuranceBet : MonoBehaviour
{
    [SerializeField] private InsuranceBetVisual _visuals;

    private List<Set> _sets;

    public event EventHandler EndOfInsuranceBet;

    // Object binding a checkbox to certain player hand.
    public class Set {
        public InsuranceChoice InsuranceChoice { get; set; }
        public PlayerHand PlayerHand { get; set; }
        public Set(PlayerHand hand, InsuranceChoice checkbox) {
            this.InsuranceChoice = checkbox;
            this.PlayerHand = hand;
        }
    }

    public InsuranceBetVisual Visuals {
        get => _visuals;
    }

    private void Awake() {
        _visuals.CloseBetsBtnClick += (sender, e) => {
            // End insurance bet.
            EndOfInsuranceBet?.Invoke(this, EventArgs.Empty);
        };
    }

    public void StartInsuranceBet(List<PlayerHand> hands) {
        _sets = _visuals.GenerateVisuals(hands);

        ChipManager.Instance.SetInsuranceHandlers(_sets.Select(set => set.InsuranceChoice).ToList());
    }

    public void HandleBlackjackCase() {
        List<Set> insuredSets = _sets.FindAll(set => set.InsuranceChoice.IsInsured == true);
        List<Set> uninsuredSets = _sets.FindAll(set => set.InsuranceChoice.IsInsured == false);

        // For insured hands, just return insurance money back to chip stack (balance).
        foreach (var set in insuredSets) {
            set.InsuranceChoice.ReturnHandler(set.InsuranceChoice, set.InsuranceChoice.Chips);
        }

        // For uninsured hands, take everything.
        foreach (var set in uninsuredSets) {
            set.InsuranceChoice.ClearChips();
            set.PlayerHand.ChipField.ClearChips();
            set.PlayerHand.ChangeState(HandState.Lost);
        }
    }

    public void HandleNoBlackjackCase() {
        foreach (var set in _sets) {
            set.InsuranceChoice.ClearChips();
        }
    }
}
