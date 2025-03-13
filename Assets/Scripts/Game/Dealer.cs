using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    [SerializeField] private int _delayBetweenCardsMiliseconds = 200;
    [SerializeField] private Shoe _shoe;

    public async void DealFirstCards(List<PlayerHand> playerHands, DealerHand dealerHand, GameManager.Callback AfterFirstCardsMethod) {
        if (_shoe.CurrentShoe == null || _shoe.CurrentShoe.Count == 0) {
            _shoe.NewShoe();
        }
        // Deal first card.
        foreach (var hand in playerHands) {
            hand.AddCard(_shoe.NextCard());
            await Task.Delay(_delayBetweenCardsMiliseconds);
        }

        dealerHand.AddCard(_shoe.NextCard(), false);
        await Task.Delay(_delayBetweenCardsMiliseconds);

        // Second card.
        foreach (var hand in playerHands) {
            hand.AddCard(_shoe.NextCard());
            await Task.Delay(_delayBetweenCardsMiliseconds);
        }

        // Hidden card.
        dealerHand.AddCard(_shoe.NextCard(), true);

        AfterFirstCardsMethod();
    }

    public async void DrawUntil17(DealerHand dealerHand, GameManager.Callback AfterDrawingHandler) {
        // Show hidden card.
        dealerHand.ShowHiddenCard();

        await Task.Delay(_delayBetweenCardsMiliseconds);

        // Draw until 17
        while (dealerHand.Score < 17) {
            dealerHand.AddCard(_shoe.NextCard(), false);

            await Task.Delay(_delayBetweenCardsMiliseconds);
        }

        AfterDrawingHandler();
    }

    public void HandleHit(PlayerHand playerCurrHand, GameManager.Callback BustHandler) {
        playerCurrHand.AddCard(_shoe.NextCard());

        // Player Busted
        if (playerCurrHand.Score > 21) {
            BustHandler();
        }
    }

    public void HandleDoubleDown(PlayerHand playerCurrHand) {
        playerCurrHand.AddCard(_shoe.NextCard());
    }
}
