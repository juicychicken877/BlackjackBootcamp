using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BlackjackNamespace;

public class Dealer : MonoBehaviour
{
    [SerializeField] private int _delayBetweenCardsMs = 500;
    [SerializeField] private Shoe _shoe;

    public int DelayBetweenCardsMs {
        get => _delayBetweenCardsMs;
    }

    public async Task DealFirstCardsAsync(List<PlayerHand> playerHands, DealerHand dealerHand) {
        if (_shoe.CurrentShoe == null || _shoe.CurrentShoe.Count == 0) {
            _shoe.NewShoe();
        }
        // Deal first card.
        foreach (var hand in playerHands) {
            hand.AddCard(_shoe.NextCard(), GameAction.None);
            await Task.Delay(_delayBetweenCardsMs);
        }

        dealerHand.AddCard(_shoe.NextCard(), false);
        await Task.Delay(_delayBetweenCardsMs);

        // Second card.
        foreach (var hand in playerHands) {
            hand.AddCard(_shoe.NextCard(), GameAction.None);
            await Task.Delay(_delayBetweenCardsMs);
        }

        // Hidden card.
        dealerHand.AddCard(_shoe.NextCard(), true);
    }

    public async Task DrawUntil17Async(DealerHand dealerHand) {
        await Task.Delay(_delayBetweenCardsMs);

        // Show hidden card.
        dealerHand.ShowHiddenCard();

        await Task.Delay(_delayBetweenCardsMs);

        // Draw until 17
        while (dealerHand.Score < 17) {
            dealerHand.AddCard(_shoe.NextCard(), false);

            await Task.Delay(_delayBetweenCardsMs);
        }
    }

    public async Task HandleHitAsync(PlayerHand playerCurrHand, GameManager.ActionHandler BustHandler) {
        await Task.Delay(_delayBetweenCardsMs);

        playerCurrHand.AddCard(_shoe.NextCard(), GameAction.Hit);

        // Player busted.
        if (playerCurrHand.Score > 21) {
            BustHandler();
        }

        await Task.Delay(_delayBetweenCardsMs);
    }

    public async Task HandleDoubleDownAsync(PlayerHand playerCurrHand, GameManager.ActionHandler BustHandler) {
        ChipManager.Instance.HandleDoubleDown(playerCurrHand);

        await Task.Delay(_delayBetweenCardsMs);

        playerCurrHand.AddCard(_shoe.NextCard(), GameAction.DoubleDown);

        // Player busted.
        if (playerCurrHand.Score > 21) {
            BustHandler();
        }

        await Task.Delay(_delayBetweenCardsMs);
    }

    public async Task HandlePlayerBlackjackAsync(PlayerHand playerCurrHand) {
        await Task.Delay(_delayBetweenCardsMs);

        ChipManager.Instance.HandleBlackjack(playerCurrHand, 1.5f);

        await Task.Delay(_delayBetweenCardsMs);
    }
}
