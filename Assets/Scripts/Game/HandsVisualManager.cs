using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class HandsVisualManager : MonoBehaviour
{
    [SerializeField] private GameObject _handPrefab;
    [SerializeField] private Transform _playerHandsParentTransform;

    private void DeletePreviousHands() {
        List<PlayerHand> children = _playerHandsParentTransform.GetComponentsInChildren<PlayerHand>().ToList();

        if (children != null) {
            foreach (PlayerHand hand in children) {
                Destroy(hand.gameObject);
            }
        }
    }

    public List<PlayerHand> CreateNewHands(List<PlayerHand> playerHands, int playerHandCount) {
        playerHands ??= new();
        playerHands?.Clear();

        DeletePreviousHands();

        // Create new hand objects and return them to logic manager.
        for (int i = 0; i < playerHandCount; i++) {
            PlayerHand newHand = NewPlayerHand();
            playerHands.Add(newHand);
        }

        return playerHands;
    }

    public PlayerHand NewPlayerHand() {
        GameObject newHand = Instantiate(_handPrefab);
        newHand.transform.SetParent(_playerHandsParentTransform, false);
        return newHand.GetComponent<PlayerHand>();
    }

    public void SetPlayerHandPos(PlayerHand playerHand, int index) {
        GameObject playerHandObj = playerHand.gameObject;

        playerHandObj.transform.SetSiblingIndex(index);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_playerHandsParentTransform.GetComponent<RectTransform>());
    }
}
