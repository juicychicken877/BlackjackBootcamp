using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private CardSO _cardSO;
    [SerializeField] private Sprite _backIamge;

    private SpriteRenderer _mySpriteRenderer;
    private Sprite _frontImage;

    public enum ImagePos {
        [Tooltip("Side of a card where a value is visible")]
        Front,
        [Tooltip("Side of a card where a value is NOT visible (hidden)")]
        Back
    }

    public CardSO CardSO {
        get => _cardSO;
    }

    private void Awake() {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();

        _frontImage = _mySpriteRenderer.sprite;
    }

    public void Turn(ImagePos pos) {
        switch (pos) {
            case ImagePos.Front: {
                _mySpriteRenderer.sprite = _frontImage;
            } break;
            case ImagePos.Back: {
                _mySpriteRenderer.sprite = _backIamge;
            } break;
        }
    }
}
