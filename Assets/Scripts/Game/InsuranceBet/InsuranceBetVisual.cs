using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsuranceBetVisual : MonoBehaviour
{
    [SerializeField] private GameObject _insuranceBetBtnSetPrefab;
    [SerializeField] private Transform _btnSetPos;
    [SerializeField] private Button _closeBetsBtn;

    private List<InsuranceBetBtnSet> _btnSets;

    private void Awake() {
        _closeBetsBtn.onClick.AddListener(() => {
            Debug.Log("Close bets btn clicked");
        });
    }

    public void GenerateVisuals(List<PlayerHand> playerHands) {
        _closeBetsBtn.gameObject.SetActive(true);

        _btnSets?.Clear();
        _btnSets ??= new();

        // Generate buttons for every hand.
        foreach (PlayerHand hand in playerHands) {
            Vector3 pos = hand.gameObject.transform.position;

            Vector3 btnSetPos = new(pos.x, _btnSetPos.position.y, pos.z);

            GameObject newSetObj = Instantiate(_insuranceBetBtnSetPrefab);

            newSetObj.transform.SetParent(this.gameObject.transform, false);
            newSetObj.transform.localPosition = btnSetPos;

            _btnSets.Add(newSetObj.GetComponent<InsuranceBetBtnSet>());
        }
    }
}
