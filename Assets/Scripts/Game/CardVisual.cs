using UnityEngine;
using static Card;

public class CardVisual : MonoBehaviour
{
    [SerializeField] private CardAnimator _animator;
    [SerializeField] private Sprite _backImage;

    private SpriteRenderer _mySpriteRenderer;
    private Sprite _frontImage;

    public CardAnimator Animator {
        get => _animator;
    }

    public enum ImagePos {
        [Tooltip("Side of a card where a value is visible")]
        Front,
        [Tooltip("Side of a card where a value is NOT visible (hidden)")]
        Back
    }

    private void Awake() {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();

        _frontImage = _mySpriteRenderer.sprite;
    }

    public void Turn(ImagePos pos) {
        switch (pos) {
            case ImagePos.Front: {
                _mySpriteRenderer.sprite = _frontImage;
            }
            break;
            case ImagePos.Back: {
                _mySpriteRenderer.sprite = _backImage;
            }
            break;
        }
    }
}
