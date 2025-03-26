using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    [SerializeField] private int _delayBetweenCardsMiliseconds = 200;
    [SerializeField] private Shoe _shoe;
    public int DelayBetweenCardsMiliseconds {
        get => _delayBetweenCardsMiliseconds;
    }

    public async Task DealFirstCardsAsync(List<PlayerHand> playerHands, DealerHand dealerHand) {
        if (_shoe.CurrentShoe == null || _shoe.CurrentShoe.Count == 0) {
            _shoe.NewShoe();
        }
        // Deal first card.
        foreach (var hand in playerHands) {
            hand.AddCard(_shoe.NextCard(), GameManager.GameAction.None);
            await Task.Delay(_delayBetweenCardsMiliseconds);
        }

        dealerHand.AddCard(_shoe.NextCard(), false);
        await Task.Delay(_delayBetweenCardsMiliseconds);

        // Second card.
        foreach (var hand in playerHands) {
            hand.AddCard(_shoe.NextCard(), GameManager.GameAction.None);
            await Task.Delay(_delayBetweenCardsMiliseconds);
        }

        // Hidden card.
        dealerHand.AddCard(_shoe.NextCard(), true);
    }

    public async Task DrawUntil17Async(DealerHand dealerHand) {
        await Task.Delay(_delayBetweenCardsMiliseconds);

        // Show hidden card.
        dealerHand.ShowHiddenCard();

        await Task.Delay(_delayBetweenCardsMiliseconds);

        // Draw until 17
        while (dealerHand.Score < 17) {
            dealerHand.AddCard(_shoe.NextCard(), false);

            await Task.Delay(_delayBetweenCardsMiliseconds);
        }
    }

    public async Task HandleHitAsync(PlayerHand playerCurrHand, GameManager.ActionHandler BustHandler) {
        await Task.Delay(_delayBetweenCardsMiliseconds);

        playerCurrHand.AddCard(_shoe.NextCard(), GameManager.GameAction.Hit);

        // Player busted.
        if (playerCurrHand.Score > 21) {
            BustHandler();
        }

        await Task.Delay(_delayBetweenCardsMiliseconds);
    }

    public async Task HandleDoubleDownAsync(PlayerHand playerCurrHand, GameManager.ActionHandler BustHandler) {
        await Task.Delay(_delayBetweenCardsMiliseconds);

        playerCurrHand.AddCard(_shoe.NextCard(), GameManager.GameAction.DoubleDown);

        // Player busted.
        if (playerCurrHand.Score > 21) {
            BustHandler();
        }

        await Task.Delay(_delayBetweenCardsMiliseconds);
    }
}
