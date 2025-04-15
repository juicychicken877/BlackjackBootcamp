using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BlackjackNamespace;

public class Dealer : MonoBehaviour
{
    [SerializeField] private int _actionDelay = 500;
    [SerializeField] private Shoe _shoe;

    public int ActionDelay {
        get => _actionDelay;
    }

    public async Task DealFirstCardsAsync(List<PlayerHand> playerHands, DealerHand dealerHand) {
        if (_shoe.CurrentShoe == null || _shoe.CurrentShoe.Count <= 20) {
            _shoe.NewShoe();
        }
        // Deal first card.
        foreach (var hand in playerHands) {
            await hand.AddCard(_shoe.NextCard(), GameAction.None);
        }

        await dealerHand.AddCard(_shoe.NextCard(), false);

        // Second card.
        foreach (var hand in playerHands) {
            await hand.AddCard(_shoe.NextCard(), GameAction.None);
        }

        await dealerHand.AddCard(_shoe.NextCard(), true);
    }

    public async Task DrawUntil17Async(DealerHand dealerHand) {
        await Task.Delay(_actionDelay);

        // Show hidden card.
        dealerHand.ShowHiddenCard();

        await Task.Delay(_actionDelay);

        // Draw until 17
        while (dealerHand.Score < 17) {
            await dealerHand.AddCard(_shoe.NextCard(), false);
        }
    }

    public async Task HandleHitAsync(PlayerHand playerCurrHand, GameManager.ActionHandler BustHandler) {
        await playerCurrHand.AddCard(_shoe.NextCard(), GameAction.Hit);

        // Player busted.
        if (playerCurrHand.Score > 21) {
            BustHandler();
        }
    }

    public async Task HandleDoubleDownAsync(PlayerHand playerCurrHand, GameManager.ActionHandler BustHandler) {
        ChipManager.Instance.HandleDoubleDown(playerCurrHand);

        await playerCurrHand.AddCard(_shoe.NextCard(), GameAction.DoubleDown);

        // Player busted.
        if (playerCurrHand.Score > 21) {
            BustHandler();
        }
    }

    public async Task HandlePlayerBlackjackAsync(PlayerHand playerCurrHand) {
        await Task.Delay(_actionDelay);

        ChipManager.Instance.HandleBlackjack(playerCurrHand, 1.5f);
    }
}
