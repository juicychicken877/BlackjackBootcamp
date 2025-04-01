using BlackjackNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChipManager : MonoBehaviour
{
    [SerializeField] private ChipVisualManager _visuals;
    [SerializeField] private int _balance = 1000;

    private static ChipManager s_instance;

    public delegate void ChipFieldClickHandler(ChipField chipField);
    public delegate void ChipReturnHandler(ChipField chipField, int chipsToReturn);

    public ChipVisualManager Visuals {
        get => _visuals;
    }

    public static ChipManager Instance {
        get => s_instance;
    }

    public int Balance {
        get => _balance;
    }

    private void Start() {
        if (s_instance != null) {
            Destroy(s_instance);
            s_instance = null;
        }
        s_instance = this;

        _visuals.UpdateVisuals(_balance);
    }

    public void SetChipFieldHandlers(List<PlayerHand> playerHands) {
        foreach (var hand in playerHands) {
            hand.ChipField.ClickHandler = ChipFieldClick;
            hand.ChipField.ChipReturnHandler = ReturnChips;
        }
    }

    private void ChipFieldClick(ChipField chipField) {
        if (_visuals.CurrSelectedBtn != null) {
            int chipValue = _visuals.CurrSelectedBtnValue;

            if (CanAffordChips(chipValue)) {
                chipField.AddChips(chipValue);

                chipField.Visuals.ChangeActionBtnsActive(true);

                _balance -= chipValue;

                _visuals.UpdateVisuals(_balance);
            }
        }
    }

    private bool CanAffordChips(int chip) {
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
        int handValue = playerHand.ChipField.ChipCount;
        playerHand.ChipField.AddChips(handValue);

        _balance -= handValue;
    }

    // Place chips at the new hands' chip field.
    public void HandleSplit(PlayerHand newHand, PlayerHand initialHand) {
        int handValue = initialHand.ChipField.ChipCount;

        newHand.ChipField.AddChips(handValue);

        _balance -= handValue;
    }

    // Blackjack ratio percent means e.g 3 to 2 (150% = 1.5), 6 to 5 (120% = 1.2)
    public void HandleBlackjack(PlayerHand playerHand, float blackjackRatioPercent) {
        float blackjackValue = playerHand.ChipField.ChipCount * blackjackRatioPercent;

        Debug.Log($"Blackjack! {blackjackValue} chips won!");

        // Add the offset.
        playerHand.ChipField.AddChips(playerHand.ChipField.ChipCount - Convert.ToInt32(blackjackValue));

        // Add winnings to balance.
        ReturnChips(playerHand.ChipField, playerHand.ChipField.ChipCount);
    }

    // Give chips 1 to 1.
    public void HandleWin(PlayerHand playerHand) {
        playerHand.ChipField.AddChips(playerHand.ChipField.ChipCount);

        Debug.Log($"{playerHand.ChipField.ChipCount} chips won!");

        // Add winnings to balance.
        ReturnChips(playerHand.ChipField, playerHand.ChipField.ChipCount);
    }

    // Take chips from field.
    public void HandleLoss(PlayerHand playerHand) {
        Debug.Log($"{playerHand.ChipField.ChipCount} chips lost!");

        playerHand.ChipField.ClearChips();
    }

    public void HandlePush(PlayerHand playerHand) {
        Debug.Log($"Push");

        ReturnChips(playerHand.ChipField, playerHand.ChipField.ChipCount);
    }

    private void ReturnChips(ChipField chipField, int chipCount) {
        _balance += chipCount;

        // If returned all chips.
        if (chipField.ChipCount == chipCount) {
            chipField.ClearChips();
        } else {
            chipField.PopChips();
        }

        _visuals.UpdateVisuals(_balance);
    }
}
