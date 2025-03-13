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
            newHand.Index = i;
        }

        return playerHands;
    }

    public PlayerHand NewPlayerHand() {
        GameObject newHand = Instantiate(_handPrefab);
        newHand.transform.SetParent(_playerHandsParentTransform, false);
        return newHand.GetComponent<PlayerHand>();
    }

    public void SetPlayerHandIndex(PlayerHand playerHand) {
        GameObject playerHandObj = playerHand.gameObject;

        playerHandObj.transform.SetSiblingIndex(playerHand.Index);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_playerHandsParentTransform.GetComponent<RectTransform>());
    }
}
