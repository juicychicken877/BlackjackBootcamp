using BlackjackNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChipManager : MonoBehaviour
{
    [SerializeField] private ChipVisualManager _visuals;
    [SerializeField] private float _balance = 1000;

    private static ChipManager s_instance;

    public delegate void ChipFieldClickHandler(ChipField chipField);
    public delegate void TakeInsuranceHandler(InsuranceChoice insuranceChoice);

    public delegate void ChipReturnHandler(IChipHolder chipHolder, float chipsToReturn);

    public ChipVisualManager Visuals {
        get => _visuals;
    }

    public static ChipManager Instance {
        get => s_instance;
    }

    public float Balance {
        get => _balance;
    }

    private void Start() {
        if (s_instance != null) {
            Destroy(s_instance);
            s_instance = null;
        }
        s_instance = this;

        _visuals.UpdateBalance(_balance);
    }

    public void SetChipFieldHandlers(List<PlayerHand> playerHands) {
        foreach (var hand in playerHands) {
            hand.ChipField.ClickHandler = ChipFieldClick;
            hand.ChipField.ChipReturnHandler = ReturnChips;
        }
    }

    public void SetInsuranceHandlers(List<InsuranceChoice> insuranceChoices) {
        foreach (var insuranceChoice in insuranceChoices) {
            insuranceChoice.TakeInsuranceHandler = InsureHand;
            insuranceChoice.ReturnHandler = ReturnChips;
        }
    }

    private void InsureHand(InsuranceChoice insuranceChoice) {
        // Can afford insurance.
        if (CanAffordChips(insuranceChoice.InsuranceValue)) {
            insuranceChoice.AddChips(insuranceChoice.InsuranceValue);

            _balance -= insuranceChoice.InsuranceValue;

            // Change visuals.
            _visuals.UpdateBalance(_balance);
            insuranceChoice.ChangeInsured(true);
        } else {
            // Cannot afford insurance
            insuranceChoice.ChangeInsured(false);
        }
    }

    private void ChipFieldClick(ChipField chipField) {
        if (_visuals.CurrSelectedBtn != null) {
            int chipValue = _visuals.CurrSelectedBtnValue;

            if (CanAffordChips(chipValue)) {
                chipField.AddChips(chipValue);

                chipField.Visuals.ChangeActionBtnsActive(true);

                _balance -= chipValue;

                _visuals.UpdateBalance(_balance);
            }
        }
    }

    public float GetInsuranceValue(PlayerHand hand) {
        return hand.ChipField.ChipCount / 2;
    }

    private bool CanAffordChips(float chip) {
        return _balance >= chip;
    }

    // Gets avaliable game actions for a hand and corrects them based on affordability (avaliable chips).
    public List<GameAction> CorrectAvaliableGameActions(PlayerHand playerHand, List<GameAction> avaliableHandGameActions) {
        if (_balance < playerHand.ChipField.ChipCount) {
            avaliableHandGameActions.Remove(GameAction.DoubleDown);
            avaliableHandGameActions.Remove(GameAction.Split);
        }

        return avaliableHandGameActions;
    }

    // Double the amount of chips in chip field.
    public void HandleDoubleDown(PlayerHand playerHand) {
        float handValue = playerHand.ChipField.ChipCount;
        playerHand.ChipField.AddChips(handValue);

        _balance -= handValue;
    }

    // Place chips at the new hands' chip field.
    public void HandleSplit(PlayerHand newHand, PlayerHand initialHand) {
        float handValue = initialHand.ChipField.ChipCount;

        newHand.ChipField.AddChips(handValue);

        _balance -= handValue;
    }

    // Blackjack ratio percent means e.g 3 to 2 (150% = 1.5), 6 to 5 (120% = 1.2)
    public void HandleBlackjack(PlayerHand playerHand, float blackjackRatioPercent) {
        int blackjackValue = Convert.ToInt32(playerHand.ChipField.ChipCount * blackjackRatioPercent);

        // Add the offset.
        playerHand.ChipField.AddChips(blackjackValue);

        playerHand.Visuals.UpdateVisuals(HandState.Won);
    }

    // Give chips 1 to 1.
    public void HandleWin(PlayerHand playerHand) {
        playerHand.ChipField.AddChips(playerHand.ChipField.ChipCount);

        playerHand.Visuals.UpdateVisuals(HandState.Won);
    }

    public void HandleGameResults(List<PlayerHand> hands, DealerHand dealerHand) {
        foreach (var hand in hands) {
            // If not busted.
            if (hand.ChipField.ChipCount > 0) {
                // Win
                if (hand.Score > dealerHand.Score || dealerHand.Score > 21) {
                    HandleWin(hand);
                }
                // Loss
                else if (hand.Score < dealerHand.Score) {
                    HandleLoss(hand);
                }
            }
        }
    }

    // Take chips from field.
    public void HandleLoss(PlayerHand playerHand) {
        playerHand.ChipField.ClearChips();

        playerHand.Visuals.UpdateVisuals(HandState.Lost);
    }

    public void CollectAllChips(List<PlayerHand> hands) {
        foreach (var hand in hands) {
            ReturnChips(hand.ChipField, hand.ChipField.ChipCount);
        }
    }

    private void ReturnChips(IChipHolder chipHolder, float amount) {
        _balance += amount;

        // If returned all chips.
        if (chipHolder.ChipCount == amount) {
            chipHolder.ClearChips();
        } else {
            chipHolder.PopChips();
        }

        _visuals.UpdateBalance(_balance);
    }
}
