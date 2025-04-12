using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

namespace BlackjackNamespace {
    public class GameSettings {
        private bool _isCashGame;

        public bool IsCashGame {
            get => _isCashGame;
            set => _isCashGame = value;
        }

        public GameSettings(bool isCashGame) {
            this._isCashGame = isCashGame;
        }
    }
    public enum GameAction {
        Hit,
        Stand,
        DoubleDown,
        Split,
        None
    }

    public enum HandState {
        [Tooltip("A hand which is inactive or was played")]
        Inactive,
        [Tooltip("Hand currently played")]
        Active,
        [Tooltip("Busted or lost to the dealer")]
        Lost,
        [Tooltip("Blackjack or won with the dealer")]
        Won
    }
}