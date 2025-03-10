using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Dealer _dealer;
    [SerializeField] private HandsManager _handsManager;

    public void Start() {
        _handsManager.CreateHands();

        _dealer.DealFirstCards(_handsManager.Hands);
    }

}
