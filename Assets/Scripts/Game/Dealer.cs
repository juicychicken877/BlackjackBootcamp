using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    [SerializeField] private int _delayBetweenCardsMiliseconds = 200;
    [SerializeField] private Shoe _shoe;

    public async void DealFirstCards(List<Hand> playerHands, Hand dealerHand, GameManager.Callback AfterFirstCardsMethod) {
        if (_shoe.CurrentShoe == null || _shoe.CurrentShoe.Count == 0) {
            _shoe.NewShoe();
        }
        // Deal first card.
        foreach (var hand in playerHands) {
            hand.AddCard(_shoe.NextCard());
            await Task.Delay(_delayBetweenCardsMiliseconds);
        }

        dealerHand.AddCard(_shoe.NextCard());
        await Task.Delay(_delayBetweenCardsMiliseconds);

        // Second card.
        foreach (var hand in playerHands) {
            hand.AddCard(_shoe.NextCard());
            await Task.Delay(_delayBetweenCardsMiliseconds);
        }

        dealerHand.AddCard(_shoe.NextCard());

        AfterFirstCardsMethod();
    }

}
