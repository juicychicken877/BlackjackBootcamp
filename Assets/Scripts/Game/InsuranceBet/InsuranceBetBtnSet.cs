using UnityEngine;

public class InsuranceBetBtnSet : MonoBehaviour
{
    [SerializeField] private InsuranceBetBtnSetVisual _visuals;

    private void Awake() {
        _visuals.YesBtnClicked += YesBtnClicked;
        _visuals.NoBtnClicked += NoBtnClicked;
    }

    private void NoBtnClicked(object sender, System.EventArgs e) {
        Debug.Log("No Btn Clicked");
    }

    private void YesBtnClicked(object sender, System.EventArgs e) {
        Debug.Log("Yes Btn Clicked");
    }
}
