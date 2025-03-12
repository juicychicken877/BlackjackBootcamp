using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class HandsVisualManager : MonoBehaviour
{
    [SerializeField] private GameObject _handPrefab;
    [SerializeField] private Transform _playerHandsParentTransform;

    private List<PlayerHand> _playerHands;

    private void DeletePreviousHands() {
        if (_playerHands.Count == 0) {
            Debug.Log("No hands to delete");
        } else {
            foreach (var hand in _playerHands) {
                Destroy(hand.gameObject);
            }
        }
    }

    public List<PlayerHand> CreateNewHands(int playerHandCount) {
        if (_playerHands != null) {
            DeletePreviousHands();
        } else {
            _playerHands = new();
        }

        // Create new hand objects and return them to logic manager.
        for (int i = 0; i < playerHandCount; i++) {
            PlayerHand newHand = NewPlayerHand();

            _playerHands.Add(newHand);
        }

        return _playerHands;
    }

    public PlayerHand NewPlayerHand() {
        GameObject newHand = Instantiate(_handPrefab);

        newHand.transform.SetParent(_playerHandsParentTransform, false);

        PlayerHand newHandScript = newHand.GetComponent<PlayerHand>();
        newHandScript.Index = _playerHands.Count;

        return newHandScript;
    }
}
