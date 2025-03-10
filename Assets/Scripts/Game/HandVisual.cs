using System.Collections.Generic;
using UnityEngine;

public class HandVisual : MonoBehaviour
{
    [Tooltip("Parent transform of new card objects")]
    [SerializeField] private Transform _cardsParentTransform;

    private List<GameObject> _hands;

    public void AddCard(GameObject cardPrefab) {
        _hands ??= new();

        GameObject newCard = Instantiate(cardPrefab);
        newCard.transform.SetParent(_cardsParentTransform, false);

        _hands.Add(newCard);
    }
}
