using System;
using System.Collections.Generic;
using UnityEngine;

public class InsuranceBet : MonoBehaviour
{
    [SerializeField] private InsuranceBetVisual _visuals;

    public event EventHandler<EndOfInsuranceBetEventArgs> EndOfInsuranceBet;

    public class EndOfInsuranceBetEventArgs : EventArgs {
        public List<PlayerHand> InsuredHands;
    } 

    public void StartBet(List<PlayerHand> playerHands) {
        _visuals.GenerateVisuals(playerHands); 
    }
}
