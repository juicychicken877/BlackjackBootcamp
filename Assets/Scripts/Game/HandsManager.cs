using System.Collections.Generic;
using UnityEngine;

public class HandsManager : MonoBehaviour
{
    [SerializeField] private GameObject _handPrefab;

    [Tooltip("Parent transform of new hand objects")]
    [SerializeField] private Transform _handsParentTransform;
    [SerializeField] private int _handCount = 3;

    private List<Hand> _hands;

    public List<Hand> Hands {
        get => _hands;
    }

    public void CreateHands() {
        _hands ??= new();
        // Create playing hands
        for (int i = 0; i < _handCount; i++) {
            NewHand();
        }
    }

    public void NewHand() {
        GameObject newHand = Instantiate(_handPrefab);

        newHand.transform.SetParent(_handsParentTransform, false);

        Hand newHandScript = newHand.GetComponent<Hand>();

        _hands.Add(newHandScript);
    }

}
