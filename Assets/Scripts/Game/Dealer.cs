using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    [SerializeField] private Shoe _shoe;

    public void DealFirstCards(List<Hand> hands) {
        if (_shoe.CurrentShoe == null || _shoe.CurrentShoe.Count == 0) {
            _shoe.NewShoe();
        }
        // Deal first card
        foreach (var hand in hands) {
            hand.AddCard(_shoe.NextCard());
        }
    }

}
